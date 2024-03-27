using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicStats : MonoBehaviour
{
    // 메딕한테 달려있는 애니메이터
    Animator medicAnimator;

    [SerializeField]
    // 근접 공격용 칼
    GameObject knife;

    [SerializeField]
    // 메딕용 총
    GameObject medicGun;

    // 근접 공격용 스탑워치
    float knifeCurrentTime;
    // 근접 공격 딜레이 시간
    float knifeAttackTime;
    // 근접 공격 중인지 판단할 변수 
    bool knifeAttack;

    float shotCurrentTime;
    float shotCoolTime;
    bool shot;
    void Start()
    {
        // 애니메이터 할당
        medicAnimator = GetComponent<Animator>();

        // 처음엔 근접 공격 중이 아니니 칼을 꺼준다
        knife.SetActive(false);

        // 근접 공격 스탑워치 초기값 0초
        knifeCurrentTime = 0.0f;
        // 근접 공격 딜레이 시간은 1초
        knifeAttackTime = 1.0f;
        // 처음엔 근접 공격 중이 아님
        knifeAttack = false;

        shotCurrentTime = 0.0f;
        shotCoolTime = 1.0f;
        shot = false;
    }


    void Update()
    {
        // 근접 공격 메서드
        MedicKnifeAttack();
        // 메딕 공격 / 힐 메서드
        MedicGunShot();

    }   // Update End

    void MedicKnifeAttack() // 메딕의 근접 공격 구현 메서드
    {
        // 만약 근접 공격 중이라면
        if (knifeAttack)   
        {
            // 스탑워치를 가동한다
            knifeCurrentTime += Time.deltaTime;
        }

        // 만약 V를 누르고 근접 공격중이 아니라면
        if (Input.GetKeyDown(KeyCode.V) && !knifeAttack)
        {
            // 근접 공격중
            knifeAttack = true;

            // 메딕용 총은 꺼주고
            medicGun.SetActive(false);
            // 근접 공격용 칼을 켜주고
            knife.SetActive(true);
            // 근접 공격 애니메이션을 취한다
            medicAnimator.SetTrigger("KnifeAttack");
        }
        // 만약 근접 공격을 하고 1초가 지났다면
        if (knifeCurrentTime > knifeAttackTime)
        {
            // 칼을 꺼주고
            knife.SetActive(false);
            // 다시 총을 켜주고
            medicGun.SetActive(true);
            // 근접 공격중이 아님
            knifeAttack = false;
            // 근접 공격 시간 초기화 
            knifeCurrentTime = 0.0f;
        }
    }

    void MedicGunShot() // 메딕의 총 발사 구현 메서드
    {
        // 만약 마우스 왼쪽 버튼을 누르면
        if (Input.GetMouseButtonDown(0))
        {
            // 총 발사 애니메이션 실행 
            medicAnimator.SetTrigger("Shot");
        }
    }

}   // Class End
