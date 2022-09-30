using Photon.Pun;
using Photon.Realtime;
using PlayfabFriendInfo = PlayFab.ClientModels.FriendInfo;
using PhotonFriendInfo = Photon.Realtime.FriendInfo;
using System.Collections;
using System.Collections.Generic;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine;
using System;
using System.Linq;

public class PhotonFriendConroller
{

    private ChatClient chatClient;
    // Start is called before the first frame update
    private void Awake()
    {
        UIAddFriend.OnAddFriend += HandleFriendRequest;
    }

    
    // Update is called once per frame
    private void OnDestroy()
    {
        UIAddFriend.OnAddFriend -= HandleFriendRequest;
    }

    private void HandleFriendRequest(string Friendname)
    {
        string Message = "001:" + PlayerPrefs.GetString("USERNAME");
        if(chatClient.SendPrivateMessage(Friendname, Message))
        {
            Debug.Log($"{Friendname}님에게 친구 요청했습니다");
        }
        else
        {
            Debug.Log("친구요청실패");
        }

    }

    
}
