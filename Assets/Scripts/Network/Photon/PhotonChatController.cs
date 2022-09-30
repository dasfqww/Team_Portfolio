using UnityEngine;
using System;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using PlayFab.ClientModels;
using PlayFab;

public class PhotonChatController : MonoBehaviour, IChatClientListener
{
    [SerializeField] private string nickName;
    private ChatClient chatClient;
    // Start is called before the first frame update

    private void Awake()
    {
        nickName = PlayerPrefs.GetString("USERNAME");

    }
    void Start()
    {
        chatClient = new ChatClient(this);
        ConnectoToPhotonChat();
    }

    // Update is called once per frame
    void Update()
    {
        chatClient.Service();
    }


    private void ConnectoToPhotonChat()
    {
        Debug.Log("Connecting to Photon Chat");
        chatClient.AuthValues = new AuthenticationValues(nickName);
        ChatAppSettings chatSettings = new ChatAppSettings
        {
            Server = PhotonNetwork.PhotonServerSettings.AppSettings.Server,
            Protocol = PhotonNetwork.PhotonServerSettings.AppSettings.Protocol,
            AppIdChat = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
            AppVersion = PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion,
            FixedRegion = PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion,
            NetworkLogging = PhotonNetwork.PhotonServerSettings.AppSettings.NetworkLogging,
        };
        chatClient.ConnectUsingSettings(chatSettings);
        
    }

    #region Photon Chat Callbacks

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log($"Photon Chat DebugReturn: {message}");
    }

    public void OnDisconnected()
    {
        Debug.Log("You have disconnected from the Photon Chat");
        chatClient.SetOnlineStatus(ChatUserStatus.Offline);
    }

    public void OnConnected()
    {
        Debug.Log("You have connected to the Photon Chat");
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log($"Photon Chat OnChatStateChange: {state.ToString()}");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        Debug.Log($"Photon Chat OnGetMessages {channelName}");
        for (int i = 0; i < senders.Length; i++)
        {
            Debug.Log($"{senders[i]} messaged: {messages[i]}");
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        if (!string.IsNullOrEmpty(message.ToString()))
        {
            // Channel Name format [Sender : Recipient]
            string[] splitNames = channelName.Split(new char[] { ':' });
            string senderName = splitNames[0];

            string[] splitMessage = message.ToString().Split(new char[] { ':' });

            
            if(splitMessage[0] =="001")
            {
                var request = new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string>
                    {
                        {"001","splitMessage[1]" }
                    }
                };
                PlayFabClientAPI.UpdateUserData(request, Ondatasend, OnError);
            }
            else if (!sender.Equals(senderName, StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log($"{sender}: {message}");
            }
        }
    }

    private void OnError(PlayFabError obj)
    { 
    }

    private void Ondatasend(UpdateUserDataResult obj)
    {
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log($"Photon Chat OnSubscribed");
        for (int i = 0; i < channels.Length; i++)
        {
            Debug.Log($"{channels[i]}");
        }
    }

    public void OnUnsubscribed(string[] channels)
    {
        Debug.Log($"Photon Chat OnUnsubscribed");
        for (int i = 0; i < channels.Length; i++)
        {
            Debug.Log($"{channels[i]}");
        }
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log($"Photon Chat OnStatusUpdate: {user} changed to {status}: {message}");
        PhotonStatus newStatus = new PhotonStatus(user, status, (string)message);
        Debug.Log($"Status Update for {user} and its now {status}.");

    }

    public void OnUserSubscribed(string channel, string user)
    {
        Debug.Log($"Photon Chat OnUserSubscribed: {channel} {user}");
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        Debug.Log($"Photon Chat OnUserUnsubscribed: {channel} {user}");
    }
    

    #endregion


}

