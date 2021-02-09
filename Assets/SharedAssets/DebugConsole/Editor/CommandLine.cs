using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CommandLine
{
    /// <summary>
    /// A command line interface that can execute functions with the [ConsoleCommand] attribute
    /// </summary>
    public class CommandLine : EditorWindow
    {
        private static List<CommandInfo> _commands = new List<CommandInfo>();

        /// <summary>
        /// Stores the text of the output box
        /// </summary>
        public string Output = "";

        /// <summary>
        /// Stores the text of the input box
        /// </summary>
        public string Input = "";

        /// <summary>
        /// Stores the current position of the scroll bar of the output box
        /// </summary>
        public Vector2 ScrollPos;

        [MenuItem("Window/Command line")]
        public static void ShowConsole()
        {
            GetWindow<CommandLine>();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Console Output:");
            ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);
            {
                EditorGUILayout.TextArea(Output, GUILayout.ExpandHeight(true));
            }
            EditorGUILayout.EndScrollView();

            //EditorGUILayout.Space();
            EditorGUILayout.LabelField("Input:");

            EditorGUILayout.BeginHorizontal();
            {
                GUI.SetNextControlName("InputField");
                Input = EditorGUILayout.TextField(Input);
                if (GUILayout.Button("Submit", GUILayout.MaxWidth(50))) Submit();
            }
            EditorGUILayout.EndHorizontal();

            if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return)
            {
                _submitNextUpdate = true;
            }

            EditorGUI.FocusTextInControl("InputField");
        }

        bool _submitNextUpdate;
        private void Update()
        {
            if (_submitNextUpdate)
            {
                _submitNextUpdate = false;
                Submit();
            }
        }

        private void OnEnable()
        {
            //Goes through every custom assembly
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().Where(i => i.FullName.StartsWith("Assembly-CSharp")))
            {
                //Goes through every type
                foreach (Type type in assembly.GetTypes())
                {
                    //Goes through every method
                    foreach (MethodInfo method in type.GetMethods())
                    {
                        //Gets the potential console command attribute of the method
                        ConsoleCommandAttribute attribute = method.GetCustomAttributes(typeof(ConsoleCommandAttribute), false).SingleOrDefault() as ConsoleCommandAttribute;

                        //If the attribute exists, register command
                        if (attribute != null)
                        {
                            _commands.Add(new CommandInfo(method, attribute));
                        }
                    }
                }
            }
            Output += "===PROJECT REBUILD. Loaded " + _commands.Count + " commands===" + Environment.NewLine;
        }

        /// <summary>
        /// Evaluates the text of the input field as a command
        /// </summary>
        public void Submit()
        {
            try
            {
                Output += ">>" + Input + Environment.NewLine;

                //If input is empty
                if (string.IsNullOrEmpty(Input))
                {
                    Input = null;
                    return;
                }

                //Splits the input into a list of arguments and clears the input field
                string[] parts = Input.Split(' ');
                Input = null;

                //Forces repainting of the window
                GetWindow<CommandLine>().Repaint();

                //Gets the matching command
                CommandInfo command = _commands
                    .Where(i => i.Name.ToUpper() == parts[0].ToUpper())
                    .FirstOrDefault();
                //Sidenote: We know that parts contains atleast one member here because we know the input string is not blank

                //If the command was undefined
                if (command == null)
                {
                    Output += "No such command! If it should exist, try reopening the window" + Environment.NewLine;
                    return;
                }
                //There are too many or too few arguments
                else if (parts.Length - 1 != command.ArgumentCount)
                {
                    Output += "Command expected " + command.ArgumentCount + " parameters, but got " + (parts.Length - 1) + Environment.NewLine;
                    return;
                }

                //Creates argument list
                List<object> args = new List<object>();

                for (int i = 0; i < command.ArgumentCount; i++)
                {
                    //Adds to list: The inputed arg (i + 1 because item 0 is the command itself) converted to the type according to the method
                    args.Add(ConvertTo(parts[i + 1], command.TargetMethod.GetParameters()[i].ParameterType));
                }

                //Invokes the target method
                object output = command.TargetMethod.Invoke(null, args.ToArray());

                //If the method gave output, append it to the console
                if (output != null)
                {
                    Output += output.ToString() + Environment.NewLine + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("An error occured when executing the command " + ex.Message);
            }
            

        }
        /// <summary>
        /// Attempts to convert a string into another data type
        /// </summary>
        /// <param name="t"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        private object ConvertTo(string input, Type t)
        {
            if (t.IsEnum)
            {
                return Enum.Parse(t, input);
            }

            return Convert.ChangeType(input, t, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }

        /// <summary>
        /// Holds internal information about a console command
        /// </summary>
        private class CommandInfo
        {
            /// <summary>
            /// The method this command targets
            /// </summary>
            public readonly MethodInfo TargetMethod;

            /// <summary>
            /// The name of the command that the user has to type to invoke it
            /// </summary>
            public readonly string Name;

            /// <summary>
            /// The amount of arguments the command has
            /// </summary>
            public readonly int ArgumentCount;

            /// <summary>
            /// The string to show when running help on the command
            /// </summary>
            public string HelpString;

            public CommandInfo(MethodInfo target, ConsoleCommandAttribute attribute)
            {
                //A command cannot be on an instance function
                if (!target.IsStatic)
                {
                    throw new InvalidOperationException("Command registed on non-static method. The method must be static!");
                }

                TargetMethod = target;
                Name = attribute.Name;
                HelpString = attribute.HelpString;
                ArgumentCount = target.GetParameters().Length;
            }

            [ConsoleCommand("Help", "Show info about a command")]
            public static string HelpCommand(string commandName)
            {
                CommandInfo command = _commands
                    .Where(i => i.Name.ToUpper() == commandName.ToUpper())
                    .FirstOrDefault();

                if (command == null)
                {
                    return "No such command with name " + commandName;
                }

                return command.HelpString;

            }

            [ConsoleCommand("List", "Lists every registered command")]
            public static string ListCommand()
            {
                return string.Join(Environment.NewLine, _commands.Select(i => i.Name.ToLower() + ": " + i.HelpString).ToArray());
            }


            [ConsoleCommand("Clear", "Clears the console buffer")]
            public static void ClearCommand()
            {
                GetWindow<CommandLine>().Output = "";
            }

            [ConsoleCommand("Syntax", "Shows the syntax of a command")]
            public static string SyntaxCommand(string commandName)
            {
                CommandInfo command = _commands
                    .Where(i => i.Name.ToUpper() == commandName.ToUpper())
                    .FirstOrDefault();

                return command.Name.ToLower() + " " + string.Join(" ", command.TargetMethod.GetParameters().Select(i => "<" + i.Name + ">").ToArray());
            }


            [ConsoleCommand("Close", "Closes the command window")]
            public static void ExitCommand()
            {
                GetWindow<CommandLine>().Close();
            }
        }
    }
}
