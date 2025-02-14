using System.IO; 
using UnityEditor;

public class AssetBundleBuildsManager
{
    //�����Ϳ� �޴��� ������ִ� ���
    [MenuItem("Asset Bundle/Build")]
    public static void AssetBundleBuiled()
    {
        ////���� ������ ��ġ
        //string directory = "./Bundle";

        ////�ش� ���丮�� �������� �ʴ´ٸ�?
        //if(!Directory.Exists(directory))
        //{
        //    Directory.CreateDirectory(directory);
        //}
        ////�ش� ��ο� ���� ���鿡 ���� ������ ���� �÷����� �����ؼ� ���带 �����ϴ� �ڵ�
        //BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

        BuildPipeline.BuildAssetBundles("Assets/Bundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);


        //�����Ϳ��� �����ִ� ���̾�α� â(Ÿ��Ʋ, ����, Ȯ�� �޼���)
        EditorUtility.DisplayDialog("Asset Bundle Build", "Asset Bundle build completed!!", "complete");
    }





}
