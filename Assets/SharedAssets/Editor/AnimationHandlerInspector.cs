using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpriteAnimation))]
public class AnimationHandlerInspector : Editor
{
    static AnimationHandlerInspector()
    {
        var currentBehaviour = EditorApplication.projectWindowItemOnGUI;
        EditorApplication.projectWindowItemOnGUI = (guid, rect) =>
        {
            if (currentBehaviour != null)
            {
                currentBehaviour(guid, rect);
            }

            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            SpriteAnimation obj = AssetDatabase.LoadAssetAtPath<SpriteAnimation>(assetPath);

            if (obj != null)
            {
                if (obj.Frames[0].Sprite == null)
                {
                    return;
                }
                GUI.DrawTexture(new Rect(new Bounds(rect.center, rect.size * 0.5f).min, rect.size * 0.5f), AssetPreview.GetAssetPreview(obj.Frames[0].Sprite));
            }
        };
    }


    public override void OnInspectorGUI()
    {
        EditorUtility.SetDirty(target);
        SpriteAnimation _ = target as SpriteAnimation;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Frame Time (" + _.FrameTime + "/" + _.FrameTimeDivider + "=" + _.FrameTime / _.FrameTimeDivider + "s):");
        _.FrameTime = EditorGUILayout.FloatField(_.FrameTime);
        _.FrameTimeDivider = EditorGUILayout.FloatField(_.FrameTimeDivider);
        EditorGUILayout.EndHorizontal();       
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Frames");

        List<SpriteAnimation.SpriteFrame> frames = _.Frames.ToList();

        for (int n = 0; n < _.FrameCount; n++)
        {
            SpriteAnimation.SpriteFrame i = _.Frames[n];

            EditorGUILayout.BeginHorizontal();
            {
                i.Sprite = EditorGUILayout.ObjectField(i.Sprite, typeof(Sprite), false, GUILayout.Width(90), GUILayout.Height(90)) as Sprite;

                EditorGUILayout.BeginVertical();
                {
                    if (_.FrameCount > 1 && GUILayout.Button("X", GUILayout.MaxWidth(32)))
                    {
                        frames.RemoveAt(n);
                    }
                    if (GUILayout.Button("^", GUILayout.MaxWidth(32)))
                    {
                        frames.RemoveAt(n);
                        frames.Insert(Mathf.Max(0, n - 1), i);
                    }
                    if (GUILayout.Button("v", GUILayout.MaxWidth(32)))
                    {
                        frames.RemoveAt(n);
                        frames.Insert(Mathf.Min(frames.Count, n + 1), i);
                    }
                    if (GUILayout.Button("+", GUILayout.MaxWidth(32)))
                    {
                        frames.Insert(n + 1, new SpriteAnimation.SpriteFrame());
                    }
                }
                EditorGUILayout.EndVertical();

                
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Frame Time Multiplier: ");
                i.FrameTimeMultiplier = EditorGUILayout.FloatField(i.FrameTimeMultiplier);
            }
            EditorGUILayout.EndHorizontal();
            

            if (!_.Frames.SequenceEqual(frames))
            {
                _.Frames = frames.ToArray();
            }

            GUILayout.Space(20);
        }     
    }
}       