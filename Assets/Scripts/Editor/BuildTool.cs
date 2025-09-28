using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        //文件信息列表
        List<string> bundleInfos = new List<string>();

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

            //获取当前资源的所有依赖文件列表。
            List<string> dependenceInfo = GetDependence(assetName);
            //创建一个信息字符串
            string bundleInfo = assetName + "|" + bundleName+".ab";

            if (dependenceInfo.Count>0)
                bundleInfo = bundleInfo + "|" + string.Join("|", dependenceInfo);

            bundleInfos.Add(bundleInfo);
        }


        // 删除旧的AssetBundle文件
        if (Directory.Exists(PathUtil.BundleOutPath))
            Directory.Delete(PathUtil.BundleOutPath, true);
        Directory.CreateDirectory(PathUtil.BundleOutPath);

        // 开始构建AssetBundle
        BuildPipeline.BuildAssetBundles(PathUtil.BundleOutPath, assetBundleBuilds.ToArray(), BuildAssetBundleOptions.None, target);
        File.WriteAllLines(PathUtil.BundleOutPath + "/" + AppConst.FileListName, bundleInfos);

        AssetDatabase.Refresh();
    }
    /// <summary>
    /// 获取依赖文件列表
    /// </summary>
    /// <param name="curFile"></param>
    /// <returns></returns>
    static List<string>GetDependence(string curFile)
    {
        List<string> dependence = new List<string>();
        string[] files = AssetDatabase.GetDependencies(curFile);//获取 文件的所有依赖
        dependence = files.Where(file => !file.EndsWith(".cs") && !file.Equals(curFile)).ToList();//提出本身路径 和脚本的路径
        return dependence;

    }
}
