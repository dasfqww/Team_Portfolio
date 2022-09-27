using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpGauge : PlayerMove
{
    PlayerMove playerMove = new PlayerMove();

    // ����ü�� / �ƽ�ü�� text ǥ��
    public Text curHealth = null;
    public Text maxHealth = null;

    //ü�� ĭ ä������ �ӵ�
    [SerializeField]
    [Range(0, 1)]
    private float fillSpeed = 0.3f;

    //ü�� ĭ ����Ʈ (ĳ���͸��� ĭ�� �ٸ�)
    public List<Image> imageList;
    int last_fill_idx = 0;
    float fill = 0;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        curHealth.text = playerMove.curhp.ToString();
        maxHealth.text = playerMove.maxhp.ToString();

        for (int i = 0; i < imageList.Count; i++)
        {
            imageList[i].fillAmount = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //HealthDown();

        curHealth.text = playerMove.curhp.ToString();
        maxHealth.text = playerMove.maxhp.ToString();

        fillHealthBox();
    }

    void fillHealthBox()
    {
        int fill_num = (int)playerMove.curhp / 25;
        int rest_gauge = (int)playerMove.maxhp % 25;
        fill = 0;
        //float filledAmount = 25.0f * last_fill_idx;

        for (int i = 0; i < imageList.Count; i++)
        {
            fill += imageList[i].fillAmount;
        }

        fill *= 25.0f;

        if (playerMove.curhp > fill) // Hp������ų�� by ����
        {
            if (fill_num == 0)
            {
                if (imageList[fill_num].fillAmount < rest_gauge / 25.0f)
                    imageList[fill_num].fillAmount += fillSpeed * Time.deltaTime;
            }
            for (int i = 0; i < fill_num; i++)
            {
                if (imageList[i].fillAmount < 1)
                {
                    imageList[i].fillAmount += fillSpeed * Time.deltaTime;
                    last_fill_idx = i;
                    break;
                }
            }

            //������ ĭ ó�� by ����
            if (last_fill_idx == fill_num - 1)
            {
                if (imageList[last_fill_idx].fillAmount >= 1)
                {
                    if (imageList[last_fill_idx + 1].fillAmount < (float)rest_gauge / 25.0f)
                        imageList[last_fill_idx + 1].fillAmount += fillSpeed * Time.deltaTime;
                }
            }
        }
        else if (playerMove.curhp < fill) // HP ���ҽ�ų�� by ����
        {
            if (fill_num >= imageList.Count - 1)
            {
                if (imageList[fill_num].fillAmount > rest_gauge / 25.0f)
                    imageList[fill_num].fillAmount -= fillSpeed * Time.deltaTime;
            }

            for (int i = imageList.Count - 1; i > fill_num; i--)
            {
                if (imageList[i].fillAmount > 0)
                {
                    imageList[i].fillAmount -= fillSpeed * Time.deltaTime;
                    last_fill_idx = i;
                    break;
                }
            }

            //������ ĭ ó�� by ����
            if (last_fill_idx == fill_num + 1)
            {
                if (imageList[last_fill_idx].fillAmount <= 0)
                {
                    if (imageList[last_fill_idx - 1].fillAmount > (float)rest_gauge / 25.0f)
                        imageList[last_fill_idx - 1].fillAmount -= fillSpeed * Time.deltaTime;
                }
            }
        }
    }

    void HealthDown()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerMove.curhp += 8;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerMove.curhp -= 8;
        }
    }
}
