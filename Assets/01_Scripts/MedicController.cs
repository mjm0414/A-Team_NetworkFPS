using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;

public class MedicController : MonoBehaviour
{

    CharacterController medicCc;

    Animator medicAnimator;

    [SerializeField]
    float moveSpeed;

    float rotateSpeed;                                                                                                  // 마우스를 움직이는 속도 

    float mouseYClamp;                                                                                                 // 마우스의 y 축 범위를 제한시킬 변수 


    float yVelocity = 0;
    float gravity;
    float jumpPower;

   


    [SerializeField]
    bool slowMove;
    bool isJumping;
    void Start()
    {
        medicCc = GetComponent<CharacterController>();

        medicAnimator = GetComponent<Animator>();

        moveSpeed = 5.0f;

        rotateSpeed = 200.0f;                                                                                          // 초기값은 10

        mouseYClamp = 0;

        gravity = -5.0f;

        jumpPower = 2.0f;

       

        slowMove = false;
        isJumping = false;

    }


    void Update()
    {

        PlayerMouseRot();
        PlayerMove();
       

        if (Input.GetKeyDown(KeyCode.K))
        {
            medicAnimator.SetTrigger("Prone");
        }

    }   // Update End



    void PlayerMove()
    {
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        yVelocity += gravity * Time.deltaTime;
        Vector3 moveDir = new Vector3(xMove, yVelocity, zMove);
        moveDir = transform.TransformDirection(moveDir);


        if (!slowMove)
        {
            medicCc.Move(moveDir * moveSpeed * Time.deltaTime);

            medicAnimator.SetFloat("xMove", xMove);
            medicAnimator.SetFloat("zMove", zMove);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                medicCc.Move(moveDir * moveSpeed * 1.1f * Time.deltaTime);
                medicAnimator.SetBool("IsSprint", true);
            }   // 스프린트
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                medicAnimator.SetBool("IsSprint", false);
            }
        }   // 기본 이동



        if (Input.GetKey(KeyCode.LeftControl))    
        {
            slowMove = true;

            medicAnimator.SetFloat("xMove", xMove * 0.3f);
            medicAnimator.SetFloat("zMove", zMove * 0.3f);
       
            medicCc.Move(moveDir * moveSpeed * 0.5f * Time.deltaTime);
        }   // 천천히 걷기
        else if (Input.GetKeyUp(KeyCode.LeftControl))   
        {
            slowMove = false;
        }// 기본이동으로 돌아가기

        if (medicCc.isGrounded)
        {
            yVelocity = 0;
            isJumping = false;
        }   // 땅에 있는지 체크

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
            medicAnimator.SetTrigger("Jump");
            medicAnimator.SetFloat("yVelocity", yVelocity * 0.5f);
        }   // 점프


    }

    void PlayerMouseRot()
    {
        if (Time.time < 1.0f) return;
        // 마우스의 x 움직임을 받아서 Player의 y 축을 회전 시킨다                                                                                                                                
        float mouseX = Input.GetAxis("Mouse X");                                                    //  마무스의 가로움직임을 받아서 매 프레임 mouseX에 할당한다
        transform.eulerAngles += Vector3.up * mouseX * rotateSpeed * Time.deltaTime;    // 입력 받은 마우스의 x축 값을 지속적으로 플레이어의 오일러 앵글 값에 더해준다 

        // 마우스의 y 움직임을 받아서 나에게 달려있는 메인카메라의 x축을 회전시킨다 
        float mouseY = Input.GetAxis("Mouse Y");                                                                          // 마우스의 y축 움직임을 받아서 매 프레임 moseY에 할당한다
        mouseYClamp += mouseY * rotateSpeed * Time.deltaTime;                                              // 전역변수에 마우스 움직임 값을 중첩 시킨다 
        mouseYClamp = Mathf.Clamp(mouseYClamp, -80.0f, 80.0f);                                        // 중첩시킨 전역변수를 범위내로 제한한다 

        medicAnimator.SetFloat("mouseY", mouseYClamp);

        // 카메라의 x 축의 회전값만 계속 중첩시켜서 camRotate에 업데이트 하고 
        // y축과  z축은 원래 카메라가 가지고 있는 값을 그대로 넣어준다 
        Vector3 camRotate = new Vector3(-mouseYClamp,
                                                                         Camera.main.transform.eulerAngles.y,
                                                                         Camera.main.transform.eulerAngles.z);

        Camera.main.transform.eulerAngles = camRotate;                                                        // 중첩시킨 카메라의 x 축을 적용한 Vector3값을 카메라의 오일러 앵글값에 매 프레임 적용 시킨다 
    }



}   // Class End
