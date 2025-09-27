using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildTool : Editor
{
    [MenuItem("Tools/BuildBundle/Build Windows Bundle")]
    static void BundleWindowsBuild()
    {
        Build(BuildTarget.StandaloneWindows);
    }

    [MenuItem("Tools/BuildBundle/Build Android Bundle")]
    static void BundleAndroidBuild()
    {
        Build(BuildTarget.Android);
    }

    [MenuItem("Tools/BuildBundle/Build iOS Bundle")]
    static void BundleIphoneBuild()
    {
        Build(BuildTarget.iOS);
    }
    
    static void Build(BuildTarget target)
    {

        List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();

        //扫描 Assets/BuildResources 文件夹下的所有文件
        string[] files = Directory.GetFiles(PathUtil.BuildResourcesPath,"*",SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith(".meta"))
                continue;
            AssetBundleBuild assetBundle = new AssetBundleBuild();

            string fileName = PathUtil.GetStardardPath(files[i]);
            Debug.Log("file:" + fileName);

            // 设置资源路径（Unity相对路径）
            string assetName = PathUtil.GetUnityPath(fileName);
            assetBundle.assetNames = new string[] { assetName };

            // 设置AssetBundle名称
            string bundleName = fileName.Replace(PathUtil.BuildResourcesPath, "").ToLower();
            bundleName = bundleName.TrimStart('\\', '/').Replace('\\', '/');
            assetBundle.assetBundleName = bundleName + ".ab";
            assetBundleBuilds.Add(assetBundle);
            
        }
        // 删除旧的AssetBundle文件
        if (Directory.Exists(PathUtil.BundleOutPath))
            Directory.Delete(PathUtil.BundleOutPath, true);
        Directory.CreateDirectory(PathUtil.BundleOutPath);
        // 开始构建AssetBundle
        BuildPipeline.BuildAssetBundles(PathUtil.BundleOutPath, assetBundleBuilds.ToArray(), BuildAssetBundleOptions.None, target);
    }
}
