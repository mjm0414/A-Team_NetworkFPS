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

            moveDir = transform.TransformDirection(moveDir);                                                  // 방향 벡터의 기준을 나 (Plyaer)로 바꾼다

            medicCc.Move(moveDir * moveSpeed * Time.deltaTime);

            medicAnimator.SetFloat("xMove", xMove);
            medicAnimator.SetFloat("zMove", zMove);
        }
    }
    void SlowMove()
    {
        if (Input.GetKey(KeyCode.LeftShift))    // 천천히 걷기
        {
            slowMove = true;

            float xMove = Input.GetAxis("Horizontal");
            float zMove = Input.GetAxis("Vertical");

            Vector3 moveDir = new Vector3(xMove, 0.0f, zMove);

            moveDir = transform.TransformDirection(moveDir);                                                  // 방향 벡터의 기준을 나 (Plyaer)로 바꾼다

            medicCc.Move(moveDir * moveSpeed * 0.4f * Time.deltaTime);

            medicAnimator.SetFloat("xMove", xMove * 0.3f);
            medicAnimator.SetFloat("zMove", zMove * 0.3f);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))   // 기본이동
        {
            slowMove = false;
        }
    }

}   // Class End
