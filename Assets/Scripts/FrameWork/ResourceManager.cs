using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UObject = UnityEngine.Object;


class ResourceManager : MonoBehaviour
{
    internal class BundleInfo
    {
        public string AssetsName;
        public string BundleName;
        public List<string> Dependences;
    }

    //存放bundle 的集合
    private Dictionary<string, BundleInfo> m_BundleInfos = new Dictionary<string, BundleInfo>();

    /// <summary>
    /// 解析版本文件
    /// </summary>
    private void ParseVersionFile()
    {
        //版本文件的路径
        string url = Path.Combine(PathUtil.BundleResourcePath, AppConst.FileListName);
        string[] data = File.ReadAllLines(url);

        //解析文件信息
        for (int i = 0; i < data.Length; i++)//遍历每一行
        {
            BundleInfo bundleInfo = new BundleInfo();
            //分割并保存 AssetsName  BundleName
            string[] info = data[i].Split('|');
            bundleInfo.AssetsName = info[0];
            bundleInfo.BundleName = info[1];
            //本质是数组， 可动态扩容
            bundleInfo.Dependences = new List<string>(info.Length - 2);


            for (int j = 2; j < info.Length; j++)
            {
                bundleInfo.Dependences.Add(info[j]);
            }
            m_BundleInfos.Add(bundleInfo.AssetsName, bundleInfo);//名为key info为值
        }
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="action">完成回调</param>
    /// <returns></returns>
    IEnumerator LoadBundleAsync(string assetName, Action<UObject> action = null)
    {
        string bundleName = m_BundleInfos[assetName].BundleName;
        string bundlePath = Path.Combine(PathUtil.BundleResourcePath, bundleName);
        List<string> dependences = m_BundleInfos[assetName].Dependences;
        if (dependences != null && dependences.Count > 0)
        {
            for (int i = 0; i < dependences.Count; i++)
            {
                yield return LoadBundleAsync(dependences[i]);
            }
        }


        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return request;

        AssetBundleRequest bundleRequest = request.assetBundle.LoadAssetAsync(assetName);
        yield return bundleRequest;

        action?.Invoke(bundleRequest?.asset);// A?B  A为空不执行B A不为空执行B
        Debug.Log("This is ResourceManager.LoadBundleAsync");

    }

    /// <summary>
    /// 编辑器环境加载资源
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="action"></param>
    void EditorLoadAsset(string assetName,Action<UObject> action = null)
    {
        Debug.Log("This is ResourceManager.EditorLoadAsset");
        //AssetDatabase.LoadAssetAtPath打包后无法使用
        UObject obj = UnityEditor.AssetDatabase.LoadAssetAtPath(assetName, typeof(UObject));
        if (obj == null)
        {
            Debug.LogErrorFormat("asset Name is not exist :" + assetName);
        }
        action?.Invoke(obj);
    }




    private void LoadAsset(string assetName, Action<UObject> action)
    {

        if (AppConst.Gamemode == GameMode.EditorMode)
        {
            EditorLoadAsset(assetName, action);
        }
        else
            StartCoroutine(LoadBundleAsync(assetName, action));
    }


   //加载资源类型 以及固定的路径
    public void LoadUI(string assetName,Action<UObject>action = null)
    {
        LoadAsset(PathUtil.GetUIPath(assetName), action);
    }
    public void LoadMusic(string assetName,Action<UObject>action = null)
    {
        LoadAsset(PathUtil.GetMusicPath(assetName), action);
    }
    public void LoadSound(string assetName,Action<UObject>action = null)
    {
        LoadAsset(PathUtil.GetSoundPath(assetName), action);
    }
    public void LoadEffect(string assetName,Action<UObject>action = null)
    {
        LoadAsset(PathUtil.GetEffectPath(assetName), action);
    }
    public void LoadScence(string assetName,Action<UObject>action = null)
    {
        LoadAsset(PathUtil.GetScenesPath(assetName), action);
    }






    //todo 卸载暂时不做

    void Start()
    {

        ParseVersionFile();
        //LoadAsset("Assets/BuildResources/UI/Prefabs/SettingUIPrefab.prefab", OnComplete);
        //LoadAsset(PathUtil.GetUIPath("SettingUIPrefab"), OnComplete);
        //LoadUI("SettingUIPrefab", OnComplete);//省略掉PathUtil判断
        LoadUI("Login/LoginUI", OnComplete);//省略掉PathUtil判断
        LoadUI("TestEditorLoad", OnComplete);//省略掉PathUtil判断


    }

    private void OnComplete(UObject obj)
    {
        GameObject go = Instantiate(obj) as GameObject;
        go.transform.SetParent(this.transform);
        go.SetActive(true);
        go.transform.localPosition = Vector3.zero;
    }
}




