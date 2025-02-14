using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GoogleDriveAssetBundle : MonoBehaviour
{
    //���ڵ� ��ũ ����
    private string imageFileURL = "https://drive.usercontent.google.com/u/0/uc?id=1p0-Osjx6nmGI5lw-kOQNNr2iO4ZQuJDk&export=download";

    public Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    StartCoroutine("DownLoadImage");
    //}

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("DownLoadImage");
        }
    }

    IEnumerator DownLoadImage()
    {
        //�ش� �ּ�(URL)�� ���� �ؽ��ĸ� ������Ʈ ��û�մϴ�.
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageFileURL);

        //������Ʈ�� �Ϸ�� ������ ����մϴ�.
        yield return www.SendWebRequest();

        //������Ʈ�� ����� �����̶��
        if(www.result == UnityWebRequest.Result.Success)
        {
            //�ٿ���� �ؽ��ĸ� �����ϴ� �ڵ�
            var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            //Texture2D�� UI���� ���� ���� Sprite ���·� ����
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1.0f);
            Debug.Log("�̹����� ���������� �����Խ��ϴ�");

            image.sprite = sprite;
        }
        else
        {
            Debug.LogError("�̹����� �������� ���߽��ϴ�.");
        }
    }
    
}
