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

    float rotateSpeed;                                                                                                  // ���콺�� �����̴� �ӵ� 

    float mouseYClamp;                                                                                                 // ���콺�� y �� ������ ���ѽ�ų ���� 


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

        rotateSpeed = 200.0f;                                                                                          // �ʱⰪ�� 10

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
            }   // ������Ʈ
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                medicAnimator.SetBool("IsSprint", false);
            }
        }   // �⺻ �̵�



        if (Input.GetKey(KeyCode.LeftControl))    
        {
            slowMove = true;

            medicAnimator.SetFloat("xMove", xMove * 0.3f);
            medicAnimator.SetFloat("zMove", zMove * 0.3f);
       
            medicCc.Move(moveDir * moveSpeed * 0.5f * Time.deltaTime);
        }   // õõ�� �ȱ�
        else if (Input.GetKeyUp(KeyCode.LeftControl))   
        {
            slowMove = false;
        }// �⺻�̵����� ���ư���

        if (medicCc.isGrounded)
        {
            yVelocity = 0;
            isJumping = false;
        }   // ���� �ִ��� üũ

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
            medicAnimator.SetTrigger("Jump");
            medicAnimator.SetFloat("yVelocity", yVelocity * 0.5f);
        }   // ����


    }

    void PlayerMouseRot()
    {
        if (Time.time < 1.0f) return;
        // ���콺�� x �������� �޾Ƽ� Player�� y ���� ȸ�� ��Ų��                                                                                                                                
        float mouseX = Input.GetAxis("Mouse X");                                                    //  �������� ���ο������� �޾Ƽ� �� ������ mouseX�� �Ҵ��Ѵ�
        transform.eulerAngles += Vector3.up * mouseX * rotateSpeed * Time.deltaTime;    // �Է� ���� ���콺�� x�� ���� ���������� �÷��̾��� ���Ϸ� �ޱ� ���� �����ش� 

        // ���콺�� y �������� �޾Ƽ� ������ �޷��ִ� ����ī�޶��� x���� ȸ����Ų�� 
        float mouseY = Input.GetAxis("Mouse Y");                                                                          // ���콺�� y�� �������� �޾Ƽ� �� ������ moseY�� �Ҵ��Ѵ�
        mouseYClamp += mouseY * rotateSpeed * Time.deltaTime;                                              // ���������� ���콺 ������ ���� ��ø ��Ų�� 
        mouseYClamp = Mathf.Clamp(mouseYClamp, -80.0f, 80.0f);                                        // ��ø��Ų ���������� �������� �����Ѵ� 

        medicAnimator.SetFloat("mouseY", mouseYClamp);

        // ī�޶��� x ���� ȸ������ ��� ��ø���Ѽ� camRotate�� ������Ʈ �ϰ� 
        // y���  z���� ���� ī�޶� ������ �ִ� ���� �״�� �־��ش� 
        Vector3 camRotate = new Vector3(-mouseYClamp,
                                                                         Camera.main.transform.eulerAngles.y,
                                                                         Camera.main.transform.eulerAngles.z);

        Camera.main.transform.eulerAngles = camRotate;                                                        // ��ø��Ų ī�޶��� x ���� ������ Vector3���� ī�޶��� ���Ϸ� �ޱ۰��� �� ������ ���� ��Ų�� 
    }



}   // Class End
