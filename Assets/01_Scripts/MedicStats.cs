using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicStats : MonoBehaviour
{
    // �޵����� �޷��ִ� �ִϸ�����
    Animator medicAnimator;

    [SerializeField]
    // ���� ���ݿ� Į
    GameObject knife;

    [SerializeField]
    // �޵�� ��
    GameObject medicGun;

    // ���� ���ݿ� ��ž��ġ
    float knifeCurrentTime;
    // ���� ���� ������ �ð�
    float knifeAttackTime;
    // ���� ���� ������ �Ǵ��� ���� 
    bool knifeAttack;

    float shotCurrentTime;
    float shotCoolTime;
    bool shot;
    void Start()
    {
        // �ִϸ����� �Ҵ�
        medicAnimator = GetComponent<Animator>();

        // ó���� ���� ���� ���� �ƴϴ� Į�� ���ش�
        knife.SetActive(false);

        // ���� ���� ��ž��ġ �ʱⰪ 0��
        knifeCurrentTime = 0.0f;
        // ���� ���� ������ �ð��� 1��
        knifeAttackTime = 1.0f;
        // ó���� ���� ���� ���� �ƴ�
        knifeAttack = false;

        shotCurrentTime = 0.0f;
        shotCoolTime = 1.0f;
        shot = false;
    }


    void Update()
    {
        // ���� ���� �޼���
        MedicKnifeAttack();
        // �޵� ���� / �� �޼���
        MedicGunShot();

    }   // Update End

    void MedicKnifeAttack() // �޵��� ���� ���� ���� �޼���
    {
        // ���� ���� ���� ���̶��
        if (knifeAttack)   
        {
            // ��ž��ġ�� �����Ѵ�
            knifeCurrentTime += Time.deltaTime;
        }

        // ���� V�� ������ ���� �������� �ƴ϶��
        if (Input.GetKeyDown(KeyCode.V) && !knifeAttack)
        {
            // ���� ������
            knifeAttack = true;

            // �޵�� ���� ���ְ�
            medicGun.SetActive(false);
            // ���� ���ݿ� Į�� ���ְ�
            knife.SetActive(true);
            // ���� ���� �ִϸ��̼��� ���Ѵ�
            medicAnimator.SetTrigger("KnifeAttack");
        }
        // ���� ���� ������ �ϰ� 1�ʰ� �����ٸ�
        if (knifeCurrentTime > knifeAttackTime)
        {
            // Į�� ���ְ�
            knife.SetActive(false);
            // �ٽ� ���� ���ְ�
            medicGun.SetActive(true);
            // ���� �������� �ƴ�
            knifeAttack = false;
            // ���� ���� �ð� �ʱ�ȭ 
            knifeCurrentTime = 0.0f;
        }
    }

    void MedicGunShot() // �޵��� �� �߻� ���� �޼���
    {
        // ���� ���콺 ���� ��ư�� ������
        if (Input.GetMouseButtonDown(0))
        {
            // �� �߻� �ִϸ��̼� ���� 
            medicAnimator.SetTrigger("Shot");
        }
    }

}   // Class End
