using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    other.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.red;

    //    PlayerManager playerManager = other.GetComponent<PlayerManager>();
    //    playerManager.FireSoundOn();        

    //    Animator animator = other.GetComponent<Animator>();
                

    //    if (animator)
    //    {
    //        animator.SetLayerWeight(1, 0);
    //        animator.SetTrigger("Damage");
    //    }

    //    if (playerManager)
    //    {
    //        playerManager.FireSoundOn();
    //        Debug.Log("사운드재생");
    //    }

    //    if (other.gameObject.name == "Player")
    //    {
    //        other.GetComponent<CharacterController>().enabled = false;
    //        other.GetComponent<Transform>().position = Vector3.zero;
    //        other.GetComponent<CharacterController>().enabled = true;
    //    }
    //}
}
