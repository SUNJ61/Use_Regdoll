#pragma warning disable IDE0051
//전처리기(메크로) 설정 : 경고 문구를 비활성화 한다.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerCtrl : MonoBehaviour
{
    private Animator animator;
    private new Transform transform; //부모 매서드 숨기기, 이미 정의 된 transform 메소드를 숨겨준다.
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

        # region Invoke Csharp Events 방식 (Player Input컴퍼넌트와 MainAction을 활용해 이벤트를 연결하여 사용)
        playerInput = GetComponent<PlayerInput>(); 
        mainActionMap = playerInput.actions.FindActionMap("PlayerActions"); //Player Input에서 ActionMap 추출
        moveAction = mainActionMap.FindAction("Move"); //ActionMap에서 Action추출
        attackAction = mainActionMap.FindAction("Attack"); //ActionMap에서 Action추출

        moveAction.performed += context => //moveAction이 실행 중일 때 다음과 같은 함수를 추가한다.
        {
            Vector2 dir = context.ReadValue<Vector2>();
            moveDir = new Vector3(dir.x, 0f, dir.y);
            animator.SetFloat("Movement", dir.magnitude);
        };
        moveAction.canceled += context => //moveAction이 종료할 때 다음과 같은 함수를 추가한다.
        {
            moveDir = Vector3.zero;
            animator.SetFloat("Movement", 0.0f);
        };
        attackAction.performed += context => //attackAction이 실행 중일 때 다음과 같은 함수를 추가한다.
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
            transform.rotation = Quaternion.LookRotation(moveDir); //입력된 진행 방향으로 회전
            transform.Translate(Vector3.forward * Time.deltaTime * 4.0f); //바라 보고있는 방향으로 전진
        }
    }

    #region Sendmessage(behevior)방식
    void OnMove(InputValue value) //private public이건 상관이 없음.
    {
        //Debug.Log($"Move = ({dir.x},{dir.y})"); // 입력 확인용
        Vector2 dir = value.Get<Vector2>(); //Player Input컴포넌트에서 값을 가져와 저장한다.
        //Debug.Log($"Move = ({dir.x},{dir.y})");
        moveDir = new Vector3(dir.x, 0f, dir.y); //Input System에서 입력된 백터 값을 저장한다.
        animator.SetFloat("Movement", dir.magnitude);
    }
    void OnAttack()
    {
        //Debug.Log("Attack"); //입력 확인용
        animator.SetTrigger("Attack");
    }
    #endregion
    #region Unity_Events 방식
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>(); //Player Input컴포넌트에서 값을 가져와 저장한다.
        moveDir = new Vector3(dir.x, 0f, dir.y);
        animator.SetFloat("Movement",dir.magnitude);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        //InputActionPhase.Started; //입력될 수 있는 context의 종류
        //InputActionPhase.Performed;
        //InputActionPhase.Canceled;
        //InputActionPhase.Waiting;

        Debug.Log($"context.phase = {context.phase}"); //입력된 context의 종류를 나타냄
        if(context.performed) //context에 입력이 일어났으면 true를 반환 (context에 performed가 입력 되었으면)
        {
            Debug.Log("Attack"); //입력 확인용
            animator.SetTrigger("Attack");
        }
    }
    #endregion
    #region Invoke Csharp Events 방식 (Player Input컴퍼넌트와 MainAction을 활용해 이벤트를 연결하여 사용)

    #endregion
}
