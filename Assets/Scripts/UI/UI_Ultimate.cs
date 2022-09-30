using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ultimate : MonoBehaviour
{
    /*[SerializeField]
    private int interval = 20; // �ñر� �� ������ ĭ ���� by ����*/

    private float tick = 0.01f;

    public Image circle = null;
    public Image ultimate_back_img = null;
    public Image ultimate_icon_img = null;

    public Text cur_gauge_percent_txt = null;

    float cur_gauge_percent;
    float fill_gauge;
    float fill_attack_gauge = 0;
    bool attack_success = false;

    [SerializeField]
    private float fill_velocity = 4.0f;


    float timer = 0;
    float waiting = 1.0f;

    private void Awake()
    {
        circle.fillAmount = 0;
        cur_gauge_percent = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ultimate_Gauge();
    }

    void ultimate_Gauge()
    {
        timer += Time.deltaTime;
        if(waiting < timer)
        {
            fill_gauge += tick;
            print("timer");

            if(attack_success)
            {
                cur_gauge_percent += (tick * 4.0f * 100.0f);
                if (cur_gauge_percent > 100)
                    cur_gauge_percent = 100;
            }
            else
            {
                cur_gauge_percent += (tick * 100.0f);
                if (cur_gauge_percent > 100)
                    cur_gauge_percent = 100;
            }

            cur_gauge_percent_txt.text = cur_gauge_percent.ToString() + "%";

            timer = 0;
        }

        if (circle.fillAmount < fill_gauge)
        {
            if (fill_attack_gauge <= 0)
            {
                attack_success = false;
                fill_attack_gauge = 0;
            }
               
            if (attack_success)
            {
                fill_attack_gauge -= tick * fill_velocity * Time.deltaTime;
                circle.fillAmount += tick * fill_velocity * Time.deltaTime;
            }
            else
                circle.fillAmount += (tick * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(1)) // �׽�Ʈ�� ���� ��Ŭ�� ���ݽ� �Ϲݰ��ݼ������� �����ϰ� 4ƽ������ ��½��׽��ϴ�. by ����
        {
            if (circle.fillAmount < 1)
            {
                attack_success = true;
                fill_gauge += tick * 4.0f;
                fill_attack_gauge = tick * 4.0f;
                //circle.fillAmount += tick * 4.0f * Time.deltaTime;
                //cur_gauge_percent += (tick * 4.0f * 100.0f);
                //cur_gauge_percent_txt.text = cur_gauge_percent.ToString() + "%";
            }
            //circle.fillAmount += tick * 4.0f;
        }

        if(circle.fillAmount == 1)
        {
            //�ñر� �� á����
            cur_gauge_percent = 0; // �ñر� ������ �ۼ�Ʈ �ʱ�ȭ 0
            if (cur_gauge_percent_txt.gameObject.active == true) // ������ �ۼ�Ʈ ����
            {
                cur_gauge_percent_txt.gameObject.SetActive(false);
            }
            if(ultimate_back_img.gameObject.active == false) // ������ ���
            {
                ultimate_back_img.gameObject.SetActive(true);
                ultimate_icon_img.gameObject.SetActive(true);
            }

            //�ñر� ���������
            if(Input.GetKeyDown(KeyCode.Q)) // �ñر� ���� �ٽ� �̹��� ���� by ����
            {
                if (cur_gauge_percent_txt.gameObject.active == false) // ������ �ۼ�Ʈ ����
                {
                    cur_gauge_percent_txt.gameObject.SetActive(true);
                }

                ultimate_back_img.gameObject.SetActive(false);
                ultimate_icon_img.gameObject.SetActive(false);
                circle.fillAmount = 0;
            }
        }
    }
}
