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

    [SerializeField] float occupyingGageRed;// ������ ���ɰ�����
    [SerializeField] float occupyingGageBlue;// ����� ���ɰ�����

    [SerializeField] int occupyingRatioRed;//������ ���� ���� %
    [SerializeField] int occupyingRatioBlue;//����� ���� ���� %

    [SerializeField] float fillSpeed = 1.0f;//���� ������ ������ �ӵ�
    [SerializeField] float fillAmount = 4.0f;//�����ؾ��ϴ� ���� �ð�

    [SerializeField] int RedTeamWins = 0;//������ �¸� Ƚ��
    [SerializeField] int BlueTeamWins = 0;//����� �¸� Ƚ��

    int maxWins = 2;//�¸� ���� - ��� ���̵� 2�� �����ϸ� ���� �¸�

    [SerializeField] bool isRedGageUp = false;//������ ���� �� ����//TODO �����Ǹ� ���������
    [SerializeField] bool isBlueGageUp = false;//����� ���� �� ����//TODO �����Ǹ� ���������

    [SerializeField] bool isOccupiedRed;//�������� ���� ��Ż��
    [SerializeField] bool isOccupiedBlue;//������� ���� ��Ż��

    //�������� �����ϴ� ������ �÷��̾� ��
    [SerializeField] public int playerRedCount { get; set; } = 0;
    //�������� �����ϴ� ����� �÷��̾� ��
    [SerializeField] public int playerBlueCount { get; set; } = 0;

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
        
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
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
