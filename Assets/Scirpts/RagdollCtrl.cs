using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class RagdollCtrl : MonoBehaviour
{
    [SerializeField]private Rigidbody[] rbodys;
    private Animator animator;

    private Vector3 moveDir = Vector3.zero;

    private string playerTag = "Player";
    void Start()
    {
        rbodys = GetComponentsInChildren<Rigidbody>(); //리깃바디가 자식에도 있으므로 모두 저장.
        animator = GetComponent<Animator>();
        SetRagDoll(true); //false를 넣으면 리깃바디 비활성화, true를 넣으면 리깃바디 활성화
        //StartCoroutine(WakeUpRagDoll());
    }

    private void Update()
    {
        if(moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDir);
            transform.Translate(Vector3.forward * Time.deltaTime * 4.0f);
        }
    }

    void SetRagDoll(bool isEnable)
    {
        foreach (Rigidbody rbody in rbodys)
        {
            rbody.isKinematic = !isEnable;
        }
    }
    IEnumerator WakeUpRagDoll() //일정 시간 후에 레그돌이 작동하도록 한다.
    {
        yield return new WaitForSeconds(5.0f);
        GetComponent<Animator>().enabled = false; //애니메이터가 없으면 레그돌이 작동한다.
        SetRagDoll(true); //레그돌이 작동할 때는 isKinematic은 활성화 되어야 한다.
    }
    IEnumerator HitCarRagDoll()
    {
        GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(5.0f);
        this.gameObject.SetActive(false);
    }

    private void DieRagDoll()
    {
        GetComponent<Animator>().enabled = false;
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag(playerTag))
        {
            StartCoroutine(HitCarRagDoll());
            //DieRagDoll();
        }
            
    }

    private void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
        moveDir = new Vector3(dir.x, 0f, dir.y);
        animator.SetFloat("Movement", moveDir.magnitude);
    }
    private void OnAttack()
    {
        animator.SetTrigger("Attack");
    }
}
