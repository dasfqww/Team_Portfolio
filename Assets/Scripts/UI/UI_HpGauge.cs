using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpGauge : MonoBehaviour
{
    PlayerMove playerMove = new PlayerMove();

    // 현재체력 / 맥스체력 text 표시
    public Text curHealth = null;
    public Text maxHealth = null;

    //체력 칸 채워지는 속도
    [SerializeField]
    [Range(0, 2)]
    private float fillSpeed = 1.5f;

    //체력 칸 리스트 (캐릭터마다 칸수 다름)
    public GameObject emptyBar_img = null;
    public GameObject Bar_img = null;
    GameObject newEmptyBar_img = null;
    GameObject newBar_img = null;
    public List<Image> imageList;
    int last_fill_idx = 0;
    float fill = 0;
    Vector2 createPoint;
    public GameObject canvas = null;

    float test = 100;

    private void Awake()
    {
        makeBar();
        createPoint = emptyBar_img.GetComponent<Image>().rectTransform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        curHealth.text = playerMove.curhp.ToString();
        maxHealth.text = playerMove.maxhp.ToString();

        /*for (int i = 0; i < imageList.Count; i++)
        {
            imageList[i].fillAmount = 0;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        HealthDown();

        curHealth.text = playerMove.curhp.ToString();
        maxHealth.text = playerMove.maxhp.ToString();

        fillHealthBox();
    }

    void makeBar()
    {
        int Bar_num = (int)playerMove.maxhp / 20; // 체력바 칸수(현재 캐릭터 맥스체력 / 20)
        for(int i = 0; i < Bar_num; i++)
        {
            newEmptyBar_img = Instantiate(emptyBar_img, createPoint, Quaternion.identity, canvas.transform);
            newBar_img = Instantiate(Bar_img, createPoint, Quaternion.identity, canvas.transform);

            Vector3 plusXPos = new Vector3(emptyBar_img.GetComponent<Image>().rectTransform.rect.width * i + Screen.width / 2.0f,
                                            Screen.height / 2.0f,
                                            0);
            newEmptyBar_img.GetComponent<Image>().rectTransform.position = emptyBar_img.GetComponent<Image>().rectTransform.position + plusXPos;
            newBar_img.GetComponent<Image>().rectTransform.position = Bar_img.GetComponent<Image>().rectTransform.position + plusXPos;
            imageList.Add(newBar_img.GetComponent<Image>());
        }
    }

    void fillHealthBox()
    {
        int fill_num = (int)playerMove.curhp / 20;
        int rest_gauge = (int)playerMove.curhp % 20;
        fill = 0;
        //print("fillnum : " + fill_num);
        //float filledAmount = 25.0f * last_fill_idx;

        for (int i = 0; i < imageList.Count; i++)
        {
            fill += imageList[i].fillAmount;
        }

        fill *= 20.0f;

        if (playerMove.curhp > fill) // Hp증가시킬때 by 혜원
        {
            if (fill_num == 0)
            {
                if (imageList[fill_num].fillAmount < rest_gauge / 20.0f)
                {
                    imageList[fill_num].fillAmount += fillSpeed * Time.deltaTime;
                    if (imageList[fill_num].fillAmount > 1)
                        imageList[fill_num].fillAmount = 1;
                }
            }
            for (int i = 0; i < fill_num; i++)
            {
                print("fill_num : " + fill_num);
                print("i : " + i);
                if (imageList[i].fillAmount < 1)
                {
                    imageList[i].fillAmount += fillSpeed * Time.deltaTime;
                   /* if (imageList[i].fillAmount > 1)
                        imageList[i].fillAmount = 1;*/
                    last_fill_idx = i;
                    break;
                }
            }

            //마지막 칸 처리 by 혜원
            if (last_fill_idx == fill_num - 1)
            {
                if (imageList[last_fill_idx].fillAmount >= 1)
                {
                    if (imageList[last_fill_idx + 1].fillAmount < (float)rest_gauge / 20.0f)
                    {
                        imageList[last_fill_idx + 1].fillAmount += fillSpeed * Time.deltaTime;
                        if (imageList[last_fill_idx + 1].fillAmount > (float)rest_gauge / 20.0f)
                            imageList[last_fill_idx + 1].fillAmount = (float)rest_gauge / 20.0f;
                    }
                        
                }
            }
        }
        else if (playerMove.curhp < fill) // HP 감소시킬때 by 혜원
        {
            if (fill_num >= imageList.Count - 1)//if (fill_num <= imageList.Count - 1 && fill_num >= 0) //if (fill_num >= imageList.Count - 1)
            {
                if (imageList[fill_num].fillAmount > (float)rest_gauge / 20.0f)
                {
                    imageList[fill_num].fillAmount -= fillSpeed * Time.deltaTime;
                    if (imageList[fill_num].fillAmount < 0)
                        imageList[fill_num].fillAmount = 0;
                }
            }

            for (int i = imageList.Count - 1; i > fill_num; i--)
            {
                if (imageList[i].fillAmount > 0)
                {
                    imageList[i].fillAmount -= fillSpeed * Time.deltaTime;
                    /*if (imageList[i].fillAmount < 0)
                        imageList[i].fillAmount = 0;*/
                    last_fill_idx = i;
                    break;
                }
            }

            //마지막 칸 처리 by 혜원
            if (last_fill_idx == fill_num + 1)
            {
                if (imageList[last_fill_idx].fillAmount <= 0)
                {
                    if (imageList[last_fill_idx - 1].fillAmount > (float)rest_gauge / 20.0f)
                    {
                        imageList[last_fill_idx - 1].fillAmount -= fillSpeed * Time.deltaTime;
                        if (imageList[last_fill_idx - 1].fillAmount < (float)rest_gauge / 20.0f)
                            imageList[last_fill_idx - 1].fillAmount = (float)rest_gauge / 20.0f;
                    }
                }
            }
        }
    }

    void HealthDown() // 단순 테스트를 위한 함수입니다. by 혜원
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerMove.curhp = 50.0f;
            //print("+");
            //playerMove.curhp = 8;
            //print(playerMove.curhp);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //print("-");
            playerMove.curhp = 70;
            //print(playerMove.curhp);
        }
    }
}
