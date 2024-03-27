using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class ConnManager : MonoBehaviourPunCallbacks
{

    void Start()
    {
        PhotonNetwork.GameVersion = "1.0";      // 일단 게임 버젼은 1.0으로 한다 

        int num = Random.Range(0, 1000);    // 일단 이름은 랜덤하게 뽑는다 

        // 원래 이 단계에서 ID와 Password를 입력하는 단계
        PhotonNetwork.NickName = num.ToString();    // 랜덤하게 뽑은 숫자를 닉네임으로 한다 

        PhotonNetwork.AutomaticallySyncScene = true;    // 씬을 자동으로 동기화 시킨다 

        PhotonNetwork.ConnectUsingSettings();   // 마스터 서버에 접속한다
    }


    void Update()
    {
        
    }
}
