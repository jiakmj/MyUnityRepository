using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    //�ε� Slider UI�� �������� ����
    public Slider loadingSlider;
    public string nextSceneName;

    public void StartLoading(string sceneName) //�ε� �ڷ�ƾ�� �����ϴ� �Լ�
    {
        nextSceneName = sceneName;
        StartCoroutine(LoadLoadingSceneAndNextScene());
    }

    IEnumerator LoadLoadingSceneAndNextScene() //�ε� ���� �ε��ϰ� ���� ����
                                               //�ε�� ������ ����ϴ� �ڷ�ƾ
    {
        //�ε� ���� �񵿱������� �ε�(�ε� ���� ǥ�ÿ����� ����ϴ� ��)
        AsyncOperation loadingSceneOp = SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive); //Additive : ���� ���� ���� �߰��ϴ� ���(���� �� ����)

        loadingSceneOp.allowSceneActivation = false; //�ڵ����� ���� ��ȯ���� �ʵ��� ����

        while (!loadingSceneOp.isDone) //�ε����� �ε�� ������ ���
        {
            if (loadingSceneOp.progress >= 0.9f) //�ε����� ���� �� �ε�� ������ ��� (progress > 0.9 �̻�Ǹ� �غ� �Ϸ� ����)
            {
                loadingSceneOp.allowSceneActivation = true; //�ε��� �غ� �Ϸ�Ǹ� �� Ȱ��ȭ
            }
            yield return null;
        }
        FindLoadingSliderInScene(); //�ε� ������ �ε� Slider�� ã�ƿ���

        AsyncOperation nextSceneOp = SceneManager.LoadSceneAsync(nextSceneName); //���� ���� �񵿱������� �ε�
        while (!nextSceneOp.isDone) //���� �� �ε尡 �Ϸ�� ������ ����ϸ鼺 ����� Slider�� ǥ��
        {
            loadingSlider.value = nextSceneOp.progress; //�ε� ���൵ ������Ʈ (0~1)
            yield return null;
        }
        SceneManager.UnloadSceneAsync("LoadingScene"); //���� ���� ������ ���ε�� ��, �ε����� ���ε�
    }

    void FindLoadingSliderInScene()
    {
        loadingSlider = GameObject.Find("LoadingSlider").GetComponent<Slider>();
    }
    

}
