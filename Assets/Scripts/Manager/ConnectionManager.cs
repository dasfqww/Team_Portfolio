using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.IsConnected==false)
        {
            //NameServer 접속(AppId, GameVersion, region)
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnConnected()
    {
        base.OnConnected();
        print("OnConnected");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("OnConnectedToMaster");

        //닉네임 설정
        PhotonNetwork.NickName = "CSM"+Random.Range(1,10000);
        //default 로비 진입
        PhotonNetwork.JoinLobby();
        //특정 로비 진입
        //PhotonNetwork.JoinLobby(new TypedLobby("CSMLobby", LobbyType.Default));
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("OnJoinedLobby");
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
