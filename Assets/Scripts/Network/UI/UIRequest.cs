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

        //���������()
        //�����鿡 text����//�Ŀ� ��ư Ŭ���� ���� ���̰�       
        // RequestText.text = "DO YOU WANT TO ACCEPT " + name + "'S FRIEND REQUEST?";

    }
}
