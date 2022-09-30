using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayfabFriendController : MonoBehaviour
{
    private int sended = 0;
    public static Action<string> OnRequestFriend = delegate { };
    private void update()
    {
        if (sended == 0)
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecieved, OnError);
        }

    }





    private void OnError(PlayFabError obj)
    {
        
    }

    private void OnDataRecieved(GetUserDataResult result)
    {
        string requestperson;
        if(result.Data != null && result.Data.ContainsKey("001"))
        {
            requestperson = result.Data["001"].Value;
            sended = 0;
            OnRequestFriend?.Invoke(requestperson);
        }
    }
}
