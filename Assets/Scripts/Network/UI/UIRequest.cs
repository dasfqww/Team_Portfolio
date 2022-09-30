using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIRequest : MonoBehaviour
{
    [SerializeField] private TMP_Text RequestText;
    [SerializeField] private GameObject InvitePannel;
    [SerializeField] private GameObject RequestPannel;
    [SerializeField] private GameObject newImg;
    [SerializeField] private GameObject newImg2;



    private void Awake()
    {
        newImg.SetActive(false);
        newImg2.SetActive(false);
        PlayfabFriendController.OnRequestFriend += HandleRequestFriend;
    }
    private void OnDestroy()
    {
        PlayfabFriendController.OnRequestFriend -= HandleRequestFriend;
    }


    private void HandleRequestFriend(string name)
    {
        newImg.SetActive(true);
        newImg2.SetActive(true);

        //프리펩생성()
        //프리펩에 text연결//후에 버튼 클릭시 문장 보이게       
        // RequestText.text = "DO YOU WANT TO ACCEPT " + name + "'S FRIEND REQUEST?";

    }
}
