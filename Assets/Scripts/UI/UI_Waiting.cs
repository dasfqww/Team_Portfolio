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

    //거점 위 플레이어 팀별 수
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
        curScramble_txt.text = "공격을 준비하십시오.  " + waiting.ToString();
        scramble_ring.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        waiting_mode();
        Occupy_Gauge();
    }

    //공격준비 10초, 거점활성화 30초 대기
    void waiting_mode()
    {
        timer += Time.deltaTime;
        if(timer > interval)
        {
            if(!attackReady) // 10초전
            {
                curScramble_txt.text = "공격을 준비하십시오.  " + (waiting - interval).ToString();
            }
            else // 10초 후 거점 활성화 30초 카운트
            {
                curScramble_txt.text = "거점 활성화까지 남은시간.  " + (waiting - interval).ToString();
            }
            interval++;
        }

        if(!attackReady && timer > waiting)// 10초 후 공격시작되면 거점활성화 30초 카운트시작
        {
            waiting = 30.0f;
            timer = 0;
            interval = 1;
            scrambling.SetActive(true);
            curScramble_txt.text = "거점 활성화까지 남은시간.  " + (waiting).ToString();
            
            attackReady = true;
        }

        if(attackReady && (timer > waiting)) // 30초 후 거점 활성화 되면
        {
            scrambling.SetActive(false);
            waiting_canvas.SetActive(false);
            unLock_Img.SetActive(true);
            aim.SetActive(true);
        }
    }

    //거점 점령
    void Occupy_Gauge()
    {
        // : 점령유지% 텍스트 출력 by 혜원
        red = GameManager.instance.occupyingRatioRed;
        blue = GameManager.instance.occupyingRatioRed;
        RedTeam_txt.text = red.ToString() + "%";
        BlueTeam_txt.text = blue.ToString() + "%";
        // : end

        if(GameManager.instance.playerRedCount != 0 && GameManager.instance.playerBlueCount != 0) // 점령존에 양팀플레이어 모두 존재하면 격돌중 표시 by 혜원
        {
            fight_txt.SetActive(true);
            fight_txt.GetComponent<Text>().text = "격돌 중!";
        }
        else // 둘중에 한팀만 점령존에 플레이어 있을때 해당팀 점령게이지에 비례하여 점령링이미지 채우기 by 혜원
        {
            fight_txt.SetActive(false);
            if (GameManager.instance.playerRedCount == 0 && GameManager.instance.playerBlueCount > 0)
                scramble_ring.fillAmount = GameManager.instance.occupyingGageBlue;
            else if (GameManager.instance.playerBlueCount == 0 && GameManager.instance.playerRedCount > 0)
                scramble_ring.fillAmount = GameManager.instance.occupyingGageRed;
        }

        // : 점령존에 있는 팀별 플레이어 수 출력 by 혜원
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
