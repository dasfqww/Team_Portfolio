using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Skill_Cooltime : MonoBehaviour
{
    public List<Image> skillCoolImg;
    public List<Image> iconImg;

    public Image left_img = null;
    public Image mid_img = null;
    public Image right_img = null;

    public Text left_txt = null;
    public Text mid_txt = null;
    public Text right_txt = null;

    float timer = 0;
    float waiting = 1.0f;
    float timer_click = 0;
    float waiting_click = 1.0f;
    float curTime = 5.0f;
    float curTime_click = 5.0f;
    bool charge = false;

    [SerializeField]
    private float cooltime1 = 5.0f;


    private void Awake()
    {
        left_img.fillAmount = 0;
        mid_img.fillAmount = 0;
        right_img.fillAmount = 0;

        for(int i = 0; i < iconImg.Count; i++)
        {
            Color color = iconImg[i].color;
            color.a = 0.3f;
            iconImg[i].color = color;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fillCoolTime();
        readySkill();
        testUseSkill();
    }

    void fillCoolTime()
    {
        timer += Time.deltaTime;
        if(timer > waiting)
        {
            //left_img.fillAmount += ((float)left_img.rectTransform.rect.height / cooltime1) / (float)left_img.rectTransform.rect.height;
            mid_img.fillAmount += ((float)mid_img.rectTransform.rect.height / cooltime1) / (float)left_img.rectTransform.rect.height;
            right_img.fillAmount += ((float)right_img.rectTransform.rect.height / cooltime1) / (float)left_img.rectTransform.rect.height;

            //left_txt.text = curTime.ToString();
            mid_txt.text = curTime.ToString();
            right_txt.text = curTime.ToString();

            if (curTime >= 0)
                curTime--;
            else
            {
                mid_txt.text = cooltime1.ToString();
                right_txt.text = cooltime1.ToString();
            }

            timer = 0;
        }

        //Charge형은 우클릭하고 쿨타임 시작
        if(Input.GetMouseButtonDown(1))
        {
            charge = true;
        }
        if(charge)
        {
            timer_click += Time.deltaTime;
            if (timer_click > waiting_click)
            {
                left_img.fillAmount += ((float)left_img.rectTransform.rect.height / cooltime1) / (float)left_img.rectTransform.rect.height;
                left_txt.text = curTime_click.ToString();

                if (curTime_click >= 0)
                    curTime_click--;

                timer_click = 0;
            }
        }
    }

    //쿨타임 모두 차면 이미지 변경
    void readySkill()
    {
        if(curTime == -1)
        {
            //아이콘 투명도 진하게 255
            for (int i = 0; i < iconImg.Count; i++)
            {
                Color color = iconImg[i].color;
                color.a = 1;
                iconImg[i].color = color;
                //iconImg[i].color = new Vector4(iconImg[i].color.r, iconImg[i].color.g, iconImg[i].color.b, 255);
            }

            for (int i = 1; i < skillCoolImg.Count; i++)
            {
                skillCoolImg[i].gameObject.SetActive(true);
            }

            mid_img.gameObject.SetActive(false);
            right_img.gameObject.SetActive(false);

            mid_txt.gameObject.SetActive(false);
            right_txt.gameObject.SetActive(false);
        }

        if (curTime_click == -1)
        {
            //아이콘 투명도 진하게 255
            Color color = iconImg[0].color;
            color.a = 1;
            iconImg[0].color = color;
            //iconImg[0].color = new Vector4(iconImg[0].color.r, iconImg[0].color.g, iconImg[0].color.b, 255);

            skillCoolImg[0].gameObject.SetActive(true);

            left_img.gameObject.SetActive(false);
            left_txt.gameObject.SetActive(false);
        }
    }

    void testUseSkill()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(curTime == -1)
            {
                //아이콘 투명도 연하게 100
                for (int i = 0; i < iconImg.Count; i++)
                {
                    iconImg[i].color = new Vector4(iconImg[i].color.r, iconImg[i].color.g, iconImg[i].color.b, 0.3f);
                }

                curTime = cooltime1 - 1;

                for (int i = 1; i < skillCoolImg.Count; i++)
                {
                    skillCoolImg[i].gameObject.SetActive(false);
                }

                //left_img.gameObject.SetActive(true);
                mid_img.gameObject.SetActive(true);
                right_img.gameObject.SetActive(true);

                //left_img.fillAmount = 0;
                mid_img.fillAmount = 0;
                right_img.fillAmount = 0;

                //left_txt.gameObject.SetActive(true);
                mid_txt.gameObject.SetActive(true);
                right_txt.gameObject.SetActive(true);
            }
        }
    }
}
