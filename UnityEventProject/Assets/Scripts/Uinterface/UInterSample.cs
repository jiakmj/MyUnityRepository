using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

//����Ƽ���� �������ִ� Event, IPointer

//IPointer Interface
//Unity�� EventSystems���� �⺻������ �����Ǵ� �������̽��Դϴ�.
//����ϱ� ���ؼ��� ������ ���� ������ �ʿ��մϴ�.

//Ŭ��, ��ġ, �巡�� ���� �̺�Ʈ�� ������ �� ����մϴ�.

//1. UI ������Ʈ���� Graphic Raycaster ������Ʈ�� �߰��Ǿ� �־�� �մϴ�.
//  �߰������� Raycast Target�� üũ�� �� ���¿��� �մϴ�.

//2. Scene���� Event System ������Ʈ�� �����ؾ� �մϴ�.

//3. ������Ʈ�� ���� �۾� �ÿ��� Collider ������Ʈ�� �߰��Ǿ� �־�� �մϴ�.

//4. Main Camera�� Physics Raycaster ������Ʈ�� �߰��Ǿ� �־�� �մϴ�.



public class UInterSample : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Ŭ���� �����߽��ϴ�.");
    }
    //��� ���
    //1. �� ����� ����� ������Ʈ�� �����մϴ�.
    //2. ���� Event System ������Ʈ�� ��ġ�մϴ�.
    //  ���࿡ ���� ĵ���� ������ �����ߴٸ�, �ڵ����� ��ġ�� �Ǹ� �ƴ� ����� ���� ���� �����մϴ�.
    //3. ������Ʈ�� �ݶ��̴��� �����մϴ�.
    //4. ī�޶� Physics Raycaster ������Ʈ�� �����մϴ�.

    //IPoninterClickHandler
    //�ش� I�� �߰��ϸ� ���콺�� Ŭ�� �Ǵ� ��ġ�� �� �� �� ȣ��Ǵ� �̺�Ʈ
    //������ ���� ��� ȣ��˴ϴ�.

    //IPointerDownHandler
    //������ ������ ȣ��Ǵ� ���콺 Ŭ��/��ġ �̺�Ʈ�Դϴ�.

    //IPointerUpHandler
    //�� ��Ȳ�� ȣ��Ǵ� ���콺 Ŭ��/��ġ �̺�Ʈ�Դϴ�.

    //IBeginDragHandler
    //�巡�� ���� �� ȣ��Ǵ� �̺�Ʈ

    //IEndDragHandler
    //�巡�� ���� �� ȣ��Ǵ� �̺�Ʈ

    //IDragHandler
    //�巡�� ���� ���� ȣ��Ǵ� �̺�Ʈ





}
