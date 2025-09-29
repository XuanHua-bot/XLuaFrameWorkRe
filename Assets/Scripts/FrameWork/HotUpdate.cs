using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class HotUpdate : MonoBehaviour
{

    internal class DownFileInfo
    {
        public string url;
        public string fileName;
        public DownloadHandler fileData;//下载后的文件数据
    }
    /// <summary>
    /// 下载单个文件
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator DownLoadFile(DownFileInfo info,Action<DownFileInfo>Complete)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(info.url);

        yield return webRequest.SendWebRequest();//等待下载完成

        if (webRequest.isHttpError || webRequest.isNetworkError)
        {
            Debug.LogError("下载文件出错"+info.url);
            yield break;
        }
        info.fileData = webRequest.downloadHandler;//将下载的数据保存到info.fileData中
        Complete?.Invoke(info);//通过回调函数通知调用者下载完成
        webRequest.Dispose();
    }
    /// <summary>
    /// 下载多个文件
    /// </summary>
    /// <param name="info"></param>
    /// <param name="Complete"></param>
    /// <returns></returns>
    IEnumerator DownLoadFile(List<DownFileInfo> infos, Action<DownFileInfo> Complete,Action DownloadAllComplete)
    {
        foreach (DownFileInfo info in infos)
        {
            yield return DownLoadFile(info, Complete);
        }
        DownloadAllComplete?.Invoke();
    }

    /// <summary>
    /// 获取文件信息
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    private List<DownFileInfo>GetFileList(string filedata,string path)
    {
        string content = filedata.Trim().Replace("\r", "");
        string[] files = content.Split('\n');
        List<DownFileInfo> downFileInfos = new List<DownFileInfo>(files.Length);
        for (int i = 0; i < files.Length; i++)
        {
            string[]info = files[i].Split('|');
            DownFileInfo fileInfo = new DownFileInfo();
            fileInfo.fileName = info[1];//0 为prefab
            fileInfo.url = Path.Combine(path, info[1]);
            downFileInfos.Add(fileInfo);


        }
        return downFileInfos;
    }
}
