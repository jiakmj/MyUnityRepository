using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public bool isOpen = false;
    public bool LastOpenedForward;
    private Animator animator;

    void Start()
    {
     
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        
    }

    public bool IsPlayerInFront(Transform player)
    {
        Vector3 toPlayer = (player.position - transform.position).normalized;
        //�÷��̾�� �� ������ ���͸� ���
        float dotProduct = Vector3.Dot(transform.position, toPlayer);
        //���� ���ϴ� ����� �÷��̾��� ������ ��(��������)
        return dotProduct > 0; //dotProduct > 0 �̸� �÷��̾ �� �տ� ����
    }

    public bool Open(Transform player)
    {

        if (!isOpen) 
        {
            isOpen = true; //���� ���� ���·� ����

            if (IsPlayerInFront(player)) //�÷��̾ �տ� ������ ������ �ִ� ���, �ڿ� ������ ������ �ִ� ���
            {
                animator.SetTrigger("OpenForward"); //������
                LastOpenedForward = true; //���� ���������� ����
            }
            else
            {
                animator.SetTrigger("OpenBackward"); //������
                LastOpenedForward = false; //���� ���������� ����
            }
            return true;
        }
        return false;
    }

    public void CloseForward(Transform player)
    {
        if (isOpen)
        {
            isOpen = false;
            animator.SetTrigger("CloseForward");

        }
    }

    public void CloseBackward(Transform player)
    {
        if (isOpen)
        {
            isOpen = false;
            animator.SetTrigger("CloseBackward");
        }
    }
}
