using System.Collections;
using System.IO;
using UnityEngine;

public class LoadAssetBundleManager : MonoBehaviour
{
    //경로 작성
    string path = "Assets/Bundles/asset1";
    void Start()
    {
        StartCoroutine(LoadAsync(path));
    }

    IEnumerator LoadAsync(string path)
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(path)); 

        //리퀘스트가 끝날 때까지
        yield return request;

        //리퀘스트를 통해 받아온 에셋 번들의 정보를 적용합니다.
        AssetBundle bundle = request.assetBundle;

        //전달받은 번들을 통해 에셋을 로드합니다.
        var prefab = bundle.LoadAsset<GameObject>("BlueSphere");
        Instantiate(prefab);
        //GameObject prefab2 = bundle.LoadAsset<GameObject>("GreenSphere");
        //Instantiate(prefab2);
        //GameObject prefab3 = bundle.LoadAsset<GameObject>("RedSphere");
        //Instantiate(prefab3);

    }

    
}
