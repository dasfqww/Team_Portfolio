using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    //[SerializeField] GameObject occupationZone;

    //[SerializeField] Transform RedRespawnPos[];
    //[SerializeField] Transform BlueRespawnPos[];

    enum GameState
    {
        Waiting,
        Ready,
        Proceeding,
        RoundFinish,
        GameOver
    }

    [SerializeField] public float occupyingGageRed { get; set; }// 레드팀 점령게이지
    [SerializeField] public float occupyingGageBlue { get; set; }// 블루팀 점령게이지

    [SerializeField] public int occupyingRatioRed { get; set; }//레드팀 점령 유지 %
    [SerializeField] public int occupyingRatioBlue { get; set; }//블루팀 점령 유지 %

    //UI연결 무관
    [SerializeField] float fillSpeed = 1.0f;//점령 게이지 오르는 속도(multiplyer구현 예정)
    [SerializeField] float fillAmount = 4.0f;//유지해야하는 점령 시간

    [SerializeField] int RedTeamWins = 0;//레드팀 승리 횟수
    [SerializeField] int BlueTeamWins = 0;//블루팀 승리 횟수

    int maxWins = 2;//승리 조건 - 어느 팀이든 2승 먼저하면 그팀 승리

    [SerializeField] bool isRedGageUp = false;//레드팀 점령 중 여부//TODO 구현되면 사라질예정
    [SerializeField] bool isBlueGageUp = false;//블루팀 점령 중 여부//TODO 구현되면 사라질예정

    [SerializeField] bool isOccupiedRed;//레드팀이 거점 쟁탈함
    [SerializeField] bool isOccupiedBlue;//블루팀이 거점 쟁탈함

    //점령존에 존재하는 레드팀 플레이어 수
    public int playerRedCount { get; set; } = 0;
    //점령존에 존재하는 블루팀 플레이어 수
    public int playerBlueCount { get; set; } = 0;

    private PhotonView photonView;

    WaitForSeconds ratioCountDelay;

    private void Awake()
    {
        ratioCountDelay = new WaitForSeconds(fillSpeed);
        //photonView.GetComponent<PhotonView>();
        if (instance ==null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else if (instance!=this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        PhotonNetwork.Instantiate("Player", Vector3.up, Quaternion.identity);
        //OccupationZone.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if ((playerRedCount == 0 && playerBlueCount == 0)
            ||(playerRedCount>0&&playerBlueCount>0))
        {
            isRedGageUp = false;
            isBlueGageUp = false;
            return;
        }
        OccupyingRed();
        OccupyingBlue();

        if (isOccupiedRed==true||isOccupiedBlue==true)
        {

        }

        if (RedTeamWins== maxWins || BlueTeamWins== maxWins)
        {
            GameOver();
        }
    }

    void GameStart()
    {
        //Invoke("ActivateOccupatinoZone",10.0f);
    }

    /*void ActivateOccupatinoZone()
    {
        OccupationZone.SetActive(true);
    }*/

    void OccupyingRed()
    {
        if (playerRedCount > 0 && playerBlueCount == 0)
        {
            isRedGageUp = true;
            isBlueGageUp = false;
            //TODO:레드팀 점령 게이지 상승 및 UI출력

        }

        else
        {
            return;
        }
    }

    void OccupyingBlue()
    {
        if (playerRedCount == 0 && playerBlueCount > 0)
        {
            isRedGageUp = false;
            isBlueGageUp = true;
            //TODO:블루팀 점령 게이지 상승 및 UI출력

        }

        else
        {
            return;
        }
    }

    void GameOver()
    {
        if (RedTeamWins==maxWins)
        {
            RedTeamWin();
        }

        else if(BlueTeamWins==maxWins)
        {
            BlueTeamWin();
        }
    }

    void RedTeamWin()
    {
        //TODO:Implement RedTeamWins

    }

    void BlueTeamWin()
    {
        //TODO:Implement BlueTeamWins

    }
}
