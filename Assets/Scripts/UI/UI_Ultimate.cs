using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ultimate : MonoBehaviour
{
    [SerializeField]
    private int interval = 20; // 궁극기 원 나눠진 칸 개수 by 혜원

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

        if (Input.GetMouseButtonDown(1)) // 테스트를 위해 우클릭 공격시 일반공격성공으로 가정하고 4틱게이지 상승시켰습니다. by 혜원
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
            //궁극기 다 찼을때
            cur_gauge_percent = 0; // 궁극기 게이지 퍼센트 초기화 0
            if (cur_gauge_percent_txt.gameObject.active == true) // 게이지 퍼센트 삭제
            {
                cur_gauge_percent_txt.gameObject.SetActive(false);
            }
            if(ultimate_back_img.gameObject.active == false) // 아이콘 출력
            {
                ultimate_back_img.gameObject.SetActive(true);
                ultimate_icon_img.gameObject.SetActive(true);
            }

            //궁극기 사용했을때
            if(Input.GetKeyDown(KeyCode.Q)) // 궁극기 사용시 다시 이미지 변경 by 혜원
            {
                if (cur_gauge_percent_txt.gameObject.active == false) // 게이지 퍼센트 삭제
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
