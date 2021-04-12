#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class BuildAllStages : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        BuildPipeline.BuildAssetBundles("Assets/RoomShuffle/RoomGeneration/Assets/Rooms", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}

#endif