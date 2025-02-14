using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetAddressableManager : MonoBehaviour
{
    //AssetReference�� Ư�� Ÿ���� �������� �ʰ� ��巹������ ���ҽ��� �����ϰ� �˴ϴ�.
    //AssetReferenceGameObject�� ��巹���� ���ҽ� �߿��� GameObject�� �ش��ϴ� ���� �����մϴ�.
    //AssetReferenceT�� ���׸��� �̿��� Ư�� ������ ���ҽ��� �����Ѵ�.
    public AssetReferenceGameObject capsule;
    //public AssetReferenceT<GameManager> manager;

    public GameObject go = new GameObject();

    private void Start()
    {
        StartCoroutine("Init");
    }

    private IEnumerator Init()
    {
        var init = Addressables.InitializeAsync();
        yield return init;
    }

    public void OnCreatButtonEnter()
    {
        capsule.InstantiateAsync().Completed += (obj) =>
        {
            go = obj.Result;
        };
    }
    public void OnreleaseButtonEnter()
    {
        Addressables.ReleaseInstance(go); //����

    


    }
}
