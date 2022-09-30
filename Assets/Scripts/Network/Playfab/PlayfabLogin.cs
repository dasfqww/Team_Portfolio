using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class PlayfabLogin : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_Text ErrorText;

    public TMP_InputField signupUsername;
    public TMP_InputField signupPassword;
    public TMP_InputField email;

    public GameObject LoginPannel;
    public GameObject SignUpPannel;

    #region Unity Methods
    void Start()
    {
        LoginPannel.SetActive(true);
        SignUpPannel.SetActive(false);

        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "5FAC2";
        }
    }
    #endregion
    #region Private Methods
    private bool IsValidUsername()
    {
        bool isValid = false;
        if (username.text.Length > 3 && username.text.Length <= 24)
            isValid = true;
        return isValid;
    }
    private void LoginWithCustomId()
    {
        Debug.Log($"Login to Playfab as {username}");
        var request = new LoginWithCustomIDRequest { CustomId = username.text, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }
    private void UpdateDisplayName(string displayname)
    {
        Debug.Log($"Updating Playfab account's DisPlay name to: {displayname}");
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = displayname };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameSuccess, OnLoginFailure);
    }
    
   
    #endregion
    #region Public Methods
    public void SetUsername(string name)
    {
        username.text = name;
        //PlayerPrefs.SetString("USERNAME", username.text);
    }
    public void Login()
    {
        var request = new LoginWithPlayFabRequest { Username = username.text, Password = password.text };
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
        UpdateDisplayName(username.text);
    }

    public void CreateAccount()
    {
        LoginPannel.SetActive(false);
        SignUpPannel.SetActive(true);


    }

    public void Register()
    {
        var request = new RegisterPlayFabUserRequest { Username = signupUsername.text, Password = signupPassword.text, Email = email.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, RegisterSuccess, RegisterFailure);
    }

    public void onClickBack()
    {
        LoginPannel.SetActive(true);
        SignUpPannel.SetActive(false);

    }

    #endregion
    #region Playfab Callbacks

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log($"You have logged into Playfab using custom id {username}");
        UpdateDisplayName(username.text);
    }
    private void OnDisplayNameSuccess(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log($"You have Updated the displayname of the Playfab account!");
        //SceneController.LoadScene("LobbyScene");
        PhotonNetwork.NickName = username.text;
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.Log($"There was an issue with your request:{error.GenerateErrorReport()}");
        ErrorText.text = error.GenerateErrorReport();
    }

    private void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("가입 성공");
        LoginPannel.SetActive(true);
        SignUpPannel.SetActive(false);
    }

    private void RegisterFailure(PlayFabError error)
    {
        Debug.LogWarning("가입 실패");
        Debug.LogWarning(error.GenerateErrorReport());
        ErrorText.text = error.GenerateErrorReport();
    }

    


    #endregion

}
