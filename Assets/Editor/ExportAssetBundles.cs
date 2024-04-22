using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExportAssetBundles 
{
    [MenuItem("Assets/Build AssetBundle")]
    static void ExportResource () {
        
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles",BuildAssetBundleOptions.None,BuildTarget.Android);
    }
}
