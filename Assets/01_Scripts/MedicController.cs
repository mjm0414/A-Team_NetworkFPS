using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;

public class MedicController : MonoBehaviour
{
    // �޵����� �޷��ִ� ĳ���� ��Ʈ�ѷ�
    CharacterController medicCc;
    // �޵����� �޷��ִ� �ִϸ�����
    Animator medicAnimator;

    [SerializeField]
    // �̵��ӵ�
    float moveSpeed;
    // ���콺 ȸ���ӵ�
    float rotateSpeed;                                                                                                
    // ���콺�� Y�� ���� ���� ����
    float mouseYClamp;                                                                                              

    // �÷��̾��� Y���� ���� ����
    float yVelocity = 0;
    // �÷��̾��� ���� �߷��� �Ҵ��� ����
    float gravity;
    // �÷��̾��� ������
    float jumpPower;

   


    [SerializeField]
    // ���� õõ�� �ȴ� ������ �Ǵ��� ����
    bool slowMove;
    // ���� ���������� �Ǵ��� ����
    bool isJumping;
    void Start()
    {
        // ĳ���� ��Ʈ�ѷ� �Ҵ�
        medicCc = GetComponent<CharacterController>();
        // �ִϸ����� �Ҵ�
        medicAnimator = GetComponent<Animator>();
        // �̵��ӵ��� 5
        moveSpeed = 5.0f;
        // ���콺 ȸ���ӵ��� 200
        rotateSpeed = 200.0f;                                                                                   
        // ���콺�� Y�� ���� ������ 0
        mouseYClamp = 0;
        // ���� �߷��� -5
        gravity = -5.0f;
        // �������� 2
        jumpPower = 2.0f;
        // ���� õõ�� �ȴ� ���� �ƴ�
        slowMove = false;
        // ���� �������� �ƴ�
        isJumping = false;

    }


    void Update()
    {
        // �÷��̾��� ���콺 ȸ�� ��Ʈ��
        PlayerMouseRot();
        // �÷��̾��� �⺻ �̵� ����
        PlayerMove();
  

    }   // Update End



    void PlayerMove()   // �÷��̾��� �⺻ �̵� ���� ���� �޼���
    {
        // ��ǲ �Ŵ����� Horizontal ���� xMove ������ �Ҵ� (�¿� �̵�)
        float xMove = Input.GetAxis("Horizontal");
        // ��ǲ �Ŵ����� Vertical ���� zMove ������ �Ҵ� (�յ� �̵�)
        float zMove = Input.GetAxis("Vertical");

        // �÷��̾��� Y���� ���� �߷°� �Ҵ� ����
        yVelocity += gravity * Time.deltaTime;
        // Vec3 moveDir ���� �� �� �� �� , ���� �߷� ���� �� ������ �޾� ���� �����ش�
        Vector3 moveDir = new Vector3(xMove, yVelocity, zMove);
        // ���� ���� ���� �÷��̾��� �������� �����Ѵ� 
        moveDir = transform.TransformDirection(moveDir);

        // �⺻ �̵� ����
        if (!slowMove)
        {
            // ĳ���� ��Ʈ�ѷ��� Move�� �̿��� �⺻ ���۱���
            medicCc.Move(moveDir * moveSpeed * Time.deltaTime);

            // �⺻ ���ۿ� ���� �ִϸ��̼� ���
            medicAnimator.SetFloat("xMove", xMove);
            medicAnimator.SetFloat("zMove", zMove);

            // ���� ���� ����Ʈ�� ������ ���̶�� (�ٱ� ����)
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // �÷��̾��� �̵��ӵ��� �÷��ְ�
                medicCc.Move(moveDir * moveSpeed * 1.1f * Time.deltaTime);
                // �ٴ� �ִϸ��̼� ���
                medicAnimator.SetBool("IsSprint", true);
            }   
            // ���� ���� ����Ʈ���� ���� �����ٸ�
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                // �ٴ� �ִϸ��̼� ��� ����
                medicAnimator.SetBool("IsSprint", false);
            }
        }   // �⺻ �̵�

        // ���� ���� ��Ʈ�� ��ư�� ������ ���̶�� (õõ�� �ȱ�)
        if (Input.GetKey(KeyCode.LeftControl))    
        {
            // õõ�� �ȴ� ��
            slowMove = true;
            // õõ�� �ȴ� �ִϸ��̼ǰ� �ӵ��� ���߾��ش�
            medicAnimator.SetFloat("xMove", xMove * 0.3f);
            medicAnimator.SetFloat("zMove", zMove * 0.3f);
       
            // �÷��̾��� �̵��ӵ� ����
            medicCc.Move(moveDir * moveSpeed * 0.5f * Time.deltaTime);
        }   
        // ���� ���� ��Ʈ�ѿ��� ���� �����ٸ�
        else if (Input.GetKeyUp(KeyCode.LeftControl))   
        {
            // õõ�� �ȴ� ���� �ƴ�
            slowMove = false;
        }

        // ���� ĳ���� ��Ʈ�ѷ��� ���� ��� �ִٸ�
        if (medicCc.isGrounded)
        {
            // �÷��̾��� Y���� 0
            yVelocity = 0;
            // ���� ���� �ƴ�
            isJumping = false;
        }   // ���� �ִ��� üũ

        // ���� �����̽��ٸ� ������ �������� �ƴ϶��
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            // �÷��̾��� Y���� ���������� �ٲٰ�
            yVelocity = jumpPower;
            // ���� ������
            isJumping = true;
            // ���� �ִϸ��̼� ���
            medicAnimator.SetTrigger("Jump");
        }   // ����


    }

    void PlayerMouseRot()   // �÷��̾��� ���콺 ȸ�� ��Ʈ�� ���� �޼��� 
    {
        // �ð��� ����ٸ� ȸ�� ����
        if (Time.time < 1.0f) return;

        // ���콺�� x �������� �޾Ƽ� Player�� y ���� ȸ�� ��Ų��
        // ���콺�� ���ο������� �޾Ƽ� �� ������ mouseX�� �Ҵ��Ѵ�
        float mouseX = Input.GetAxis("Mouse X");

        // �Է� ���� ���콺�� x�� ���� ���������� �÷��̾��� ���Ϸ� �ޱ� ���� �����ش� 
        transform.eulerAngles += Vector3.up * mouseX * rotateSpeed * Time.deltaTime;

        // ���콺�� y �������� �޾Ƽ� ������ �޷��ִ� ����ī�޶��� x���� ȸ����Ų�� 
        // ���콺�� y�� �������� �޾Ƽ� �� ������ moseY�� �Ҵ��Ѵ�
        float mouseY = Input.GetAxis("Mouse Y");

        // ���������� ���콺 ������ ���� ��ø ��Ų�� 
        mouseYClamp += mouseY * rotateSpeed * Time.deltaTime;
        // ��ø��Ų ���������� �������� �����Ѵ� 
        mouseYClamp = Mathf.Clamp(mouseYClamp, -80.0f, 80.0f);                                  
        // ���콺�� ȸ�� ���� ���� �� / �Ʒ��� �Ĵٺ��� �ִϸ��̼� ���
        medicAnimator.SetFloat("mouseY", mouseYClamp);

        // ī�޶��� x ���� ȸ������ ��� ��ø���Ѽ� camRotate�� ������Ʈ �ϰ� 
        // y���  z���� ���� ī�޶� ������ �ִ� ���� �״�� �־��ش� 
        Vector3 camRotate = new Vector3(-mouseYClamp,
                                                                         Camera.main.transform.eulerAngles.y,
                                                                         Camera.main.transform.eulerAngles.z);

        // ��ø��Ų ī�޶��� x ���� ������ Vector3���� ī�޶��� ���Ϸ� �ޱ۰��� �� ������ ���� ��Ų�� 
        Camera.main.transform.eulerAngles = camRotate;                                                  
    }



}   // Class End
