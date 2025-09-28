using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testLoad : MonoBehaviour
{
    private IEnumerator Start()
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/ui/prefabs/settinguiprefab.prefab.ab");
        yield return request;// 加载模板

        AssetBundleCreateRequest request1 = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/ui/res/background.png.ab");
        yield return request1;//加载资源

        AssetBundleCreateRequest request2 = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/ui/res/button_150.png.ab");
        yield return request2;//加载资源

        AssetBundleRequest bundleRequest= request.assetBundle.LoadAssetAsync("Assets/BuildResources/UI/Prefabs/SettingUIPrefab.prefab");
        yield return bundleRequest;

        GameObject go = Instantiate(bundleRequest.asset) as GameObject;
        go.transform.SetParent(this.transform);
        go.SetActive(true);
        go.transform.localPosition = Vector3.zero;
    }
}

