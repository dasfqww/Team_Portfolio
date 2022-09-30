using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ultimate : MonoBehaviour
{
    [SerializeField]
    private int interval = 20; // �ñر� �� ������ ĭ ���� by ����

    [SerializeField]
    private float tick = 0.05f;

    public Image circle = null;
    public Image ultimate_back_img = null;
    public Image ultimate_icon_img = null;

    public Text cur_gauge_percent_txt = null;

    float cur_gauge_percent;

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
            print("timer");
            if(circle.fillAmount < 1)
            {
                circle.fillAmount += tick;
                cur_gauge_percent += (tick * 100.0f);
                cur_gauge_percent_txt.text = cur_gauge_percent.ToString() + "%";
            }

            timer = 0;
        }    

        if (Input.GetMouseButtonDown(1)) // �׽�Ʈ�� ���� ��Ŭ�� ���ݽ� �Ϲݰ��ݼ������� �����ϰ� 4ƽ������ ��½��׽��ϴ�. by ����
        {
            if (circle.fillAmount < 1)
            {
                circle.fillAmount += tick * 4.0f;
                cur_gauge_percent += (tick * 4.0f * 100.0f);
                cur_gauge_percent_txt.text = cur_gauge_percent.ToString() + "%";
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
