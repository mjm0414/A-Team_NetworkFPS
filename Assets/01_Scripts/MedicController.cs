using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicController : MonoBehaviour
{

    CharacterController medicCc;

    Animator medicAnimator;

    float moveSpeed;

    [SerializeField]
    bool slowMove;
    void Start()
    {
        medicCc = GetComponent<CharacterController>();
        medicAnimator = GetComponent<Animator>();
        moveSpeed = 5.0f;
        slowMove = false;

    }


    void Update()
    {
        Move();
        SlowMove();

        if (Input.GetKeyDown(KeyCode.K))
        {
            medicAnimator.SetTrigger("Prone");
        }

    }   // Update End
    void Move()
    {
        if (!slowMove)
        {
            float xMove = Input.GetAxis("Horizontal");
            float zMove = Input.GetAxis("Vertical");

            Vector3 moveDir = new Vector3(xMove, 0.0f, zMove);

            moveDir = transform.TransformDirection(moveDir);                                                  // ���� ������ ������ �� (Plyaer)�� �ٲ۴�

            medicCc.Move(moveDir * moveSpeed * Time.deltaTime);

            medicAnimator.SetFloat("xMove", xMove);
            medicAnimator.SetFloat("zMove", zMove);
        }
    }
    void SlowMove()
    {
        if (Input.GetKey(KeyCode.LeftShift))    // õõ�� �ȱ�
        {
            slowMove = true;

            float xMove = Input.GetAxis("Horizontal");
            float zMove = Input.GetAxis("Vertical");

            Vector3 moveDir = new Vector3(xMove, 0.0f, zMove);

            moveDir = transform.TransformDirection(moveDir);                                                  // ���� ������ ������ �� (Plyaer)�� �ٲ۴�

            medicCc.Move(moveDir * moveSpeed * 0.4f * Time.deltaTime);

            medicAnimator.SetFloat("xMove", xMove * 0.3f);
            medicAnimator.SetFloat("zMove", zMove * 0.3f);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))   // �⺻�̵�
        {
            slowMove = false;
        }
    }

}   // Class End
