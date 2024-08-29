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
        rbodys = GetComponentsInChildren<Rigidbody>(); //����ٵ� �ڽĿ��� �����Ƿ� ��� ����.
        animator = GetComponent<Animator>();
        SetRagDoll(true); //false�� ������ ����ٵ� ��Ȱ��ȭ, true�� ������ ����ٵ� Ȱ��ȭ
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
    IEnumerator WakeUpRagDoll() //���� �ð� �Ŀ� ���׵��� �۵��ϵ��� �Ѵ�.
    {
        yield return new WaitForSeconds(5.0f);
        GetComponent<Animator>().enabled = false; //�ִϸ����Ͱ� ������ ���׵��� �۵��Ѵ�.
        SetRagDoll(true); //���׵��� �۵��� ���� isKinematic�� Ȱ��ȭ �Ǿ�� �Ѵ�.
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
