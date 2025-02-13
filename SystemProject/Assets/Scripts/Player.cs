using UnityEngine;
using System;


public class Player : MonoBehaviour
{
    public AudioEvent audioEvent;
    public Item item;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioEvent.OnPlay += PlaySound;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            audioEvent.Play("���� �����");
        }
        if(Input.GetKeyDown(KeyCode.W))
        
        {
            DropItem();
        }
        
    }

    private void DropItem()
    {
        var item_Object = item.gameobject;

        Instantiate(item_Object, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void PlaySound(string soundName)
    {
        Debug.Log(soundName + "�÷��� ���Դϴ�.");
    }

}
