using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSample : MonoBehaviour
{
    //����Ƽ ������ ����Ŭ(life cycle)
    //����Ƽ������ ���� �ܰ���� ������� �Լ��� �����մϴ�.
    //ex) Awake(���� ��), Start(����), Update(������) ...

    //Ȱ��ȭ �Ǿ��� ���
    private void OnEnable()
    {
        Debug.Log("OnSceneLoaded�� ��ϵǾ����ϴ�");
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    //��Ȱ��ȭ�� ���
    private void OnDisable()
    {
        Debug.Log("OnSceneLoaded�� �����Ǿ����ϴ�.");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"���� �ε�� ���� �̸��� {scene.name}�Դϴ�.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SceneManager.LoadScene("BRP Sample Scene");
            //���� �� ��带 �������� ������ LoadSceneMode�� Single�� ó���˴ϴ�.
            //Single ����� ������ ���� ����Ÿ���� �����մϴ�.
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene("BRP Sample Scene", LoadSceneMode.Additive);
            //LoadSceneMode�� Additive�� ���� ���� �� ���� ���ο� ���� �ߺ��ؼ� �ε��ϴ� ����
            //�翬�� �� �۾��� ������ ��� �⺻ ������Ʈ(Main Camera, Direction Light) � �� �ε�Ǳ� ������ �����ؾ� �մϴ�.
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine("LoadScene");

            //SceneManager.LoadSceneAsync("BRP Sample Scene", LoadSceneMode.Additive);
            //�񵿱���(async) �ε�
            //�Ϲ����� ���� �۾��� ���������� ó���˴ϴ�.
            //���� �ε��� �ٵ� ������ �ٸ� ��ҵ��� �۵����� �ʰ� �˴ϴ�.

        }

    }

    IEnumerator LoadScene()
    {
        yield return SceneManager.LoadSceneAsync("BRP Sample Scene", LoadSceneMode.Additive);
    }    

}
