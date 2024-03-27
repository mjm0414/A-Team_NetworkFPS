using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;

public class MedicController : MonoBehaviour
{
    // 메딕한테 달려있는 캐릭터 컨트롤러
    CharacterController medicCc;
    // 메딕한테 달려있는 애니메이터
    Animator medicAnimator;

    [SerializeField]
    // 이동속도
    float moveSpeed;
    // 마우스 회전속도
    float rotateSpeed;                                                                                                
    // 마우스의 Y축 범위 제한 변수
    float mouseYClamp;                                                                                              

    // 플레이어의 Y값을 담을 변수
    float yVelocity = 0;
    // 플레이어의 가상 중력을 할당할 변수
    float gravity;
    // 플레이어의 점프력
    float jumpPower;

   


    [SerializeField]
    // 현재 천천히 걷는 중인지 판단할 변수
    bool slowMove;
    // 현재 점프중인지 판단할 변수
    bool isJumping;
    void Start()
    {
        // 캐릭터 컨트롤러 할당
        medicCc = GetComponent<CharacterController>();
        // 애니메이터 할당
        medicAnimator = GetComponent<Animator>();
        // 이동속도는 5
        moveSpeed = 5.0f;
        // 마우스 회전속도는 200
        rotateSpeed = 200.0f;                                                                                   
        // 마우스의 Y축 범위 제한은 0
        mouseYClamp = 0;
        // 가상 중력은 -5
        gravity = -5.0f;
        // 점프력은 2
        jumpPower = 2.0f;
        // 현재 천천히 걷는 중이 아님
        slowMove = false;
        // 현재 점프중이 아님
        isJumping = false;

    }


    void Update()
    {
        // 플레이어의 마우스 회전 컨트롤
        PlayerMouseRot();
        // 플레이어의 기본 이동 조작
        PlayerMove();
  

    }   // Update End



    void PlayerMove()   // 플레이어의 기본 이동 조작 구현 메서드
    {
        // 인풋 매니저의 Horizontal 값을 xMove 변수에 할당 (좌우 이동)
        float xMove = Input.GetAxis("Horizontal");
        // 인풋 매니저의 Vertical 값을 zMove 변수에 할당 (앞뒤 이동)
        float zMove = Input.GetAxis("Vertical");

        // 플레이어의 Y값에 가상 중력값 할당 시작
        yVelocity += gravity * Time.deltaTime;
        // Vec3 moveDir 값에 앞 뒤 좌 우 , 가상 중력 값을 매 프레임 받아 적용 시켜준다
        Vector3 moveDir = new Vector3(xMove, yVelocity, zMove);
        // 받은 값을 현재 플레이어의 방향으로 설정한다 
        moveDir = transform.TransformDirection(moveDir);

        // 기본 이동 구현
        if (!slowMove)
        {
            // 캐릭터 컨트롤러의 Move를 이용해 기본 조작구현
            medicCc.Move(moveDir * moveSpeed * Time.deltaTime);

            // 기본 조작에 맞춰 애니메이션 재생
            medicAnimator.SetFloat("xMove", xMove);
            medicAnimator.SetFloat("zMove", zMove);

            // 만약 왼쪽 쉬프트를 누르는 중이라면 (뛰기 구현)
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // 플레이어의 이동속도를 올려주고
                medicCc.Move(moveDir * moveSpeed * 1.1f * Time.deltaTime);
                // 뛰는 애니메이션 재생
                medicAnimator.SetBool("IsSprint", true);
            }   
            // 만약 왼쪽 쉬프트에서 손을 떼었다면
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                // 뛰는 애니메이션 재생 중지
                medicAnimator.SetBool("IsSprint", false);
            }
        }   // 기본 이동

        // 만약 왼쪽 컨트롤 버튼을 누르는 중이라면 (천천히 걷기)
        if (Input.GetKey(KeyCode.LeftControl))    
        {
            // 천천히 걷는 중
            slowMove = true;
            // 천천히 걷는 애니메이션과 속도를 맞추어준다
            medicAnimator.SetFloat("xMove", xMove * 0.3f);
            medicAnimator.SetFloat("zMove", zMove * 0.3f);
       
            // 플레이어의 이동속도 감소
            medicCc.Move(moveDir * moveSpeed * 0.5f * Time.deltaTime);
        }   
        // 만약 왼쪽 컨트롤에서 손을 떼었다면
        else if (Input.GetKeyUp(KeyCode.LeftControl))   
        {
            // 천천히 걷는 중이 아님
            slowMove = false;
        }

        // 현재 캐릭터 컨트롤러가 땅을 밟고 있다면
        if (medicCc.isGrounded)
        {
            // 플레이어의 Y값은 0
            yVelocity = 0;
            // 점프 중이 아님
            isJumping = false;
        }   // 땅에 있는지 체크

        // 만약 스페이스바를 누르고 점프중이 아니라면
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            // 플레이어의 Y값을 점프력으로 바꾸고
            yVelocity = jumpPower;
            // 현재 점프중
            isJumping = true;
            // 점프 애니메이션 재생
            medicAnimator.SetTrigger("Jump");
        }   // 점프


    }

    void PlayerMouseRot()   // 플레이어의 마우스 회전 컨트롤 구현 메서드 
    {
        // 시간이 멈췄다면 회전 중지
        if (Time.time < 1.0f) return;

        // 마우스의 x 움직임을 받아서 Player의 y 축을 회전 시킨다
        // 마우스의 가로움직임을 받아서 매 프레임 mouseX에 할당한다
        float mouseX = Input.GetAxis("Mouse X");

        // 입력 받은 마우스의 x축 값을 지속적으로 플레이어의 오일러 앵글 값에 더해준다 
        transform.eulerAngles += Vector3.up * mouseX * rotateSpeed * Time.deltaTime;

        // 마우스의 y 움직임을 받아서 나에게 달려있는 메인카메라의 x축을 회전시킨다 
        // 마우스의 y축 움직임을 받아서 매 프레임 moseY에 할당한다
        float mouseY = Input.GetAxis("Mouse Y");

        // 전역변수에 마우스 움직임 값을 중첩 시킨다 
        mouseYClamp += mouseY * rotateSpeed * Time.deltaTime;
        // 중첩시킨 전역변수를 범위내로 제한한다 
        mouseYClamp = Mathf.Clamp(mouseYClamp, -80.0f, 80.0f);                                  
        // 마우스의 회전 값에 따라 위 / 아래를 쳐다보는 애니메이션 재생
        medicAnimator.SetFloat("mouseY", mouseYClamp);

        // 카메라의 x 축의 회전값만 계속 중첩시켜서 camRotate에 업데이트 하고 
        // y축과  z축은 원래 카메라가 가지고 있는 값을 그대로 넣어준다 
        Vector3 camRotate = new Vector3(-mouseYClamp,
                                                                         Camera.main.transform.eulerAngles.y,
                                                                         Camera.main.transform.eulerAngles.z);

        // 중첩시킨 카메라의 x 축을 적용한 Vector3값을 카메라의 오일러 앵글값에 매 프레임 적용 시킨다 
        Camera.main.transform.eulerAngles = camRotate;                                                  
    }



}   // Class End
