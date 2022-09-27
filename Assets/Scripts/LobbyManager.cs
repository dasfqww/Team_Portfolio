using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        //CreateRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions=new RoomOptions();        

        roomOptions.MaxPlayers = 6;

        PhotonNetwork.CreateRoom("TestRoom", roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("OnCreatedRoom");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreatedRoomFailed, "+ returnCode + ", "+message);

        JoinRoom();
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("TestRoom");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");
        PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        print("OnJoinRoomFailed, " + returnCode + ", " + message);
    }
}
