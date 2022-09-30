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

    [SerializeField] public float occupyingGageRed { get; set; }// ������ ���ɰ�����
    [SerializeField] public float occupyingGageBlue { get; set; }// ����� ���ɰ�����

    [SerializeField] public int occupyingRatioRed { get; set; }//������ ���� ���� %
    [SerializeField] public int occupyingRatioBlue { get; set; }//����� ���� ���� %

    //UI���� ����
    [SerializeField] float fillSpeed = 1.0f;//���� ������ ������ �ӵ�(multiplyer���� ����)
    [SerializeField] float fillAmount = 4.0f;//�����ؾ��ϴ� ���� �ð�

    [SerializeField] int RedTeamWins = 0;//������ �¸� Ƚ��
    [SerializeField] int BlueTeamWins = 0;//����� �¸� Ƚ��

    int maxWins = 2;//�¸� ���� - ��� ���̵� 2�� �����ϸ� ���� �¸�

    [SerializeField] bool isRedGageUp = false;//������ ���� �� ����//TODO �����Ǹ� ���������
    [SerializeField] bool isBlueGageUp = false;//����� ���� �� ����//TODO �����Ǹ� ���������

    [SerializeField] bool isOccupiedRed;//�������� ���� ��Ż��
    [SerializeField] bool isOccupiedBlue;//������� ���� ��Ż��

    //�������� �����ϴ� ������ �÷��̾� ��
    public int playerRedCount { get; set; } = 0;
    //�������� �����ϴ� ����� �÷��̾� ��
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
            //TODO:������ ���� ������ ��� �� UI���

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
            //TODO:����� ���� ������ ��� �� UI���

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
