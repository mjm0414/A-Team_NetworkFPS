using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class ConnManager : MonoBehaviourPunCallbacks
{

    void Start()
    {
        PhotonNetwork.GameVersion = "1.0";      // �ϴ� ���� ������ 1.0���� �Ѵ� 

        int num = Random.Range(0, 1000);    // �ϴ� �̸��� �����ϰ� �̴´� 

        // ���� �� �ܰ迡�� ID�� Password�� �Է��ϴ� �ܰ�
        PhotonNetwork.NickName = num.ToString();    // �����ϰ� ���� ���ڸ� �г������� �Ѵ� 

        PhotonNetwork.AutomaticallySyncScene = true;    // ���� �ڵ����� ����ȭ ��Ų�� 

        PhotonNetwork.ConnectUsingSettings();   // ������ ������ �����Ѵ�
    }


    void Update()
    {
        
    }
}
