using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUtil
{
    //使用application 访问 会产生gc  所以 保存变量 只需使用一次Application

    // 项目根目录（Assets文件夹的完整路径）
    public static readonly string AssetsPath = Application.dataPath;

    //需要打bundle 的目录 Assets/BuildResources
    public static readonly string BuildResourcesPath = AssetsPath + "/BuildResources";

    //bundle输出目录 StreamingAssets文件夹
    public static readonly string BundleOutPath = Application.streamingAssetsPath;

    //获取unity的相对路径  把完整路径转换为Unity认识的相对路径
    public static string GetUnityPath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return string.Empty;
        return path.Substring(path.IndexOf("Assets"));

        
    }

    /// <summary>
    /// 获取标准路径 （把反斜杠变成正斜杠）
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetStardardPath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return string.Empty;

        return path.Trim().Replace("\\", "/");

    }


}
