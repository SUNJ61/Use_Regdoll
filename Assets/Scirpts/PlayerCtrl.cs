#pragma warning disable IDE0051
//��ó����(��ũ��) ���� : ��� ������ ��Ȱ��ȭ �Ѵ�.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerCtrl : MonoBehaviour
{
    private Animator animator;
    private new Transform transform; //�θ� �ż��� �����, �̹� ���� �� transform �޼ҵ带 �����ش�.
    private Vector3 moveDir;
    [Header("C# Event")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputActionMap mainActionMap;
    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction attackAction;
    private void Start()
    {
        animator = GetComponent<Animator>();
        transform = base.transform;

        # region Invoke Csharp Events ��� (Player Input���۳�Ʈ�� MainAction�� Ȱ���� �̺�Ʈ�� �����Ͽ� ���)
        playerInput = GetComponent<PlayerInput>(); 
        mainActionMap = playerInput.actions.FindActionMap("PlayerActions"); //Player Input���� ActionMap ����
        moveAction = mainActionMap.FindAction("Move"); //ActionMap���� Action����
        attackAction = mainActionMap.FindAction("Attack"); //ActionMap���� Action����

        moveAction.performed += context => //moveAction�� ���� ���� �� ������ ���� �Լ��� �߰��Ѵ�.
        {
            Vector2 dir = context.ReadValue<Vector2>();
            moveDir = new Vector3(dir.x, 0f, dir.y);
            animator.SetFloat("Movement", dir.magnitude);
        };
        moveAction.canceled += context => //moveAction�� ������ �� ������ ���� �Լ��� �߰��Ѵ�.
        {
            moveDir = Vector3.zero;
            animator.SetFloat("Movement", 0.0f);
        };
        attackAction.performed += context => //attackAction�� ���� ���� �� ������ ���� �Լ��� �߰��Ѵ�.
        {
            Debug.Log("Attack by C# event");
            animator.SetTrigger("Attack");
        };
        #endregion
    }
    private void Update()
    {
        if(moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDir); //�Էµ� ���� �������� ȸ��
            transform.Translate(Vector3.forward * Time.deltaTime * 4.0f); //�ٶ� �����ִ� �������� ����
        }
    }

    #region Sendmessage(behevior)���
    void OnMove(InputValue value) //private public�̰� ����� ����.
    {
        //Debug.Log($"Move = ({dir.x},{dir.y})"); // �Է� Ȯ�ο�
        Vector2 dir = value.Get<Vector2>(); //Player Input������Ʈ���� ���� ������ �����Ѵ�.
        //Debug.Log($"Move = ({dir.x},{dir.y})");
        moveDir = new Vector3(dir.x, 0f, dir.y); //Input System���� �Էµ� ���� ���� �����Ѵ�.
        animator.SetFloat("Movement", dir.magnitude);
    }
    void OnAttack()
    {
        //Debug.Log("Attack"); //�Է� Ȯ�ο�
        animator.SetTrigger("Attack");
    }
    #endregion
    #region Unity_Events ���
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>(); //Player Input������Ʈ���� ���� ������ �����Ѵ�.
        moveDir = new Vector3(dir.x, 0f, dir.y);
        animator.SetFloat("Movement",dir.magnitude);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        //InputActionPhase.Started; //�Էµ� �� �ִ� context�� ����
        //InputActionPhase.Performed;
        //InputActionPhase.Canceled;
        //InputActionPhase.Waiting;

        Debug.Log($"context.phase = {context.phase}"); //�Էµ� context�� ������ ��Ÿ��
        if(context.performed) //context�� �Է��� �Ͼ���� true�� ��ȯ (context�� performed�� �Է� �Ǿ�����)
        {
            Debug.Log("Attack"); //�Է� Ȯ�ο�
            animator.SetTrigger("Attack");
        }
    }
    #endregion
    #region Invoke Csharp Events ��� (Player Input���۳�Ʈ�� MainAction�� Ȱ���� �̺�Ʈ�� �����Ͽ� ���)

    #endregion
}
