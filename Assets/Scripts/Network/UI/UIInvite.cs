using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIInvite : MonoBehaviour
{
    [SerializeField] public string _friendName;
    [SerializeField] private TMP_Text _friendNameText;
    [SerializeField] private GameObject newImg;
    [SerializeField] private GameObject newImg2;

    public static Action<UIInvite> OnInviteAccept = delegate { };
    public static Action<UIInvite> OnInviteDecline = delegate { };

    public void Initialize(string friendName)
    {
        _friendName = friendName;

        _friendNameText.SetText(_friendName);
    }


    public void AccecptInvite()
    {
        OnInviteAccept?.Invoke(this);
        newImg.SetActive(false);
        newImg2.SetActive(false);
    }


    public void DeclineInvite()
    {
        OnInviteDecline?.Invoke(this);
        newImg.SetActive(false);
        newImg2.SetActive(false);
    }








}
