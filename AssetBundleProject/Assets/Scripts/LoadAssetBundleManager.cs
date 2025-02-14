using System.Collections;
using System.IO;
using UnityEngine;

public class LoadAssetBundleManager : MonoBehaviour
{
    //��� �ۼ�
    string path = "Assets/Bundles/asset1";
    void Start()
    {
        StartCoroutine(LoadAsync(path));
    }

    IEnumerator LoadAsync(string path)
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(path)); 

        //������Ʈ�� ���� ������
        yield return request;

        //������Ʈ�� ���� �޾ƿ� ���� ������ ������ �����մϴ�.
        AssetBundle bundle = request.assetBundle;

        //���޹��� ������ ���� ������ �ε��մϴ�.
        var prefab = bundle.LoadAsset<GameObject>("BlueSphere");
        Instantiate(prefab);
        //GameObject prefab2 = bundle.LoadAsset<GameObject>("GreenSphere");
        //Instantiate(prefab2);
        //GameObject prefab3 = bundle.LoadAsset<GameObject>("RedSphere");
        //Instantiate(prefab3);

    }

    
}
