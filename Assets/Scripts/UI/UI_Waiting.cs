using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Waiting : MonoBehaviour
{
    public Text curScramble_txt = null;
    public GameObject unLock_Img = null;
    public GameObject waiting_canvas = null;
    public GameObject scrambling = null;
    public GameObject aim = null;

    //���� �� �÷��̾� ���� ��
    public GameObject red_member_num = null;
    public GameObject blue_member_num = null;
    public GameObject fight_txt = null;
    //float red_mem_num = 0;
    //float blue_mem_num = 0;

    public Image scramble_ring = null;
    public Text BlueTeam_txt = null;
    public Text RedTeam_txt = null;

    float timer = 0;
    float interval = 1.0f;
    float waiting = 10.0f;

    float red = 0;
    float blue = 0;

    bool attackReady = false;

    // Start is called before the first frame update
    void Start()
    {
        curScramble_txt.text = "������ �غ��Ͻʽÿ�.  " + waiting.ToString();
        scramble_ring.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        waiting_mode();
        Occupy_Gauge();
    }

    //�����غ� 10��, ����Ȱ��ȭ 30�� ���
    void waiting_mode()
    {
        timer += Time.deltaTime;
        if(timer > interval)
        {
            if(!attackReady) // 10����
            {
                curScramble_txt.text = "������ �غ��Ͻʽÿ�.  " + (waiting - interval).ToString();
            }
            else // 10�� �� ���� Ȱ��ȭ 30�� ī��Ʈ
            {
                curScramble_txt.text = "���� Ȱ��ȭ���� �����ð�.  " + (waiting - interval).ToString();
            }
            interval++;
        }

        if(!attackReady && timer > waiting)// 10�� �� ���ݽ��۵Ǹ� ����Ȱ��ȭ 30�� ī��Ʈ����
        {
            waiting = 30.0f;
            timer = 0;
            interval = 1;
            scrambling.SetActive(true);
            curScramble_txt.text = "���� Ȱ��ȭ���� �����ð�.  " + (waiting).ToString();
            
            attackReady = true;
        }

        if(attackReady && (timer > waiting)) // 30�� �� ���� Ȱ��ȭ �Ǹ�
        {
            scrambling.SetActive(false);
            waiting_canvas.SetActive(false);
            unLock_Img.SetActive(true);
            aim.SetActive(true);
        }
    }

    //���� ����
    void Occupy_Gauge()
    {
        // : ��������% �ؽ�Ʈ ��� by ����
        red = GameManager.instance.occupyingRatioRed;
        blue = GameManager.instance.occupyingRatioRed;
        RedTeam_txt.text = red.ToString() + "%";
        BlueTeam_txt.text = blue.ToString() + "%";
        // : end

        if(GameManager.instance.playerRedCount != 0 && GameManager.instance.playerBlueCount != 0) // �������� �����÷��̾� ��� �����ϸ� �ݵ��� ǥ�� by ����
        {
            fight_txt.SetActive(true);
            fight_txt.GetComponent<Text>().text = "�ݵ� ��!";
        }
        else // ���߿� ������ �������� �÷��̾� ������ �ش��� ���ɰ������� ����Ͽ� ���ɸ��̹��� ä��� by ����
        {
            fight_txt.SetActive(false);
            if (GameManager.instance.playerRedCount == 0 && GameManager.instance.playerBlueCount > 0)
                scramble_ring.fillAmount = GameManager.instance.occupyingGageBlue;
            else if (GameManager.instance.playerBlueCount == 0 && GameManager.instance.playerRedCount > 0)
                scramble_ring.fillAmount = GameManager.instance.occupyingGageRed;
        }

        // : �������� �ִ� ���� �÷��̾� �� ��� by ����
        if (GameManager.instance.playerRedCount != 0)
        {
            print("red : " + GameManager.instance.playerRedCount);
            red_member_num.SetActive(true);
            red_member_num.GetComponentInChildren<Text>().text = GameManager.instance.playerRedCount.ToString();
        }
        else
        {
            red_member_num.SetActive(false);
        }

        if (GameManager.instance.playerBlueCount != 0)
        {
            blue_member_num.SetActive(true);
            blue_member_num.GetComponentInChildren<Text>().text = GameManager.instance.playerRedCount.ToString();
        }
        else
        {
            blue_member_num.SetActive(false);
        }
        // : end
    }
}
