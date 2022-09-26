using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealSpawner : MonoBehaviour
{
    [SerializeField]
    [Range(0, 200)]
    private float healEffectSPos = 0;

    List<Vector3> posList0 = new List<Vector3>();
    List<Vector3> posList = new List<Vector3>();
    List<Vector3> posList2 = new List<Vector3>();
    List<Vector3> spawnerPosList = new List<Vector3>();

    //public Image plusImage0 = null;
    public Image plusImage = null;
    //public Image plusImage2 = null;
    Image newPlusImage;

    float timer0 = 0;
    float waiting0 = 1.0f;

    float timer = 0;
    float waiting = 1.0f;

    float timer2 = 0;
    float waiting2 = 1.0f;

    int half_width;// = Screen.width / 2;
    int half_height;// = Screen.height / 2;

    // Start is called before the first frame update
    void Start()
    {
        half_width = Screen.width / 2;
        half_height = Screen.height / 2;
        /*spawnerPosList.Add(new Vector3(half_width * 2 - (plusImage.sprite.rect.width / 2.0f), half_height * 2 - (plusImage.sprite.rect.height / 2.0f), 0));
        spawnerPosList.Add(new Vector3(70, (plusImage.sprite.rect.height / 2.0f), 0));
        spawnerPosList.Add(new Vector3(70, half_height * 2 - (plusImage.sprite.rect.height / 2.0f), 0));
        spawnerPosList.Add(new Vector3(half_width * 2, (plusImage.sprite.rect.height / 2.0f), 0));
        spawnerPosList.Add(new Vector3(half_width * 2 - (plusImage.sprite.rect.width / 2.0f), half_height, 0));
        spawnerPosList.Add(new Vector3((plusImage.sprite.rect.width / 2.0f), half_height, 0));
        spawnerPosList.Add(new Vector3(half_width, half_height * 2 - (plusImage.sprite.rect.height / 2.0f), 0));
        spawnerPosList.Add(new Vector3(half_width, (plusImage.sprite.rect.height / 2.0f), 0));*/
    }

    // Update is called once per frame
    void Update()
    {
        makeHealPlusIcon0();
        makeHealPlusIcon();
        makeHealPlusIcon2();
    }

    void makeRandomPosList0()
    {
        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            Vector3 pos = new Vector3(half_width * 2 - 70 * i - healEffectSPos, half_height * 2 - (plusImage.sprite.rect.width / 2.0f) - 10 * i- healEffectSPos, 0);
            posList0.Add(pos);
        }

        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            Vector3 pos = new Vector3(70 + 70 * i + healEffectSPos, half_height * 2 - (plusImage.sprite.rect.width / 2.0f) - 10 * i - healEffectSPos, 0);
            posList0.Add(pos);
        }

        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            Vector3 pos = new Vector3((plusImage.sprite.rect.width / 2.0f) + 70 * i + healEffectSPos, half_height + 10 * i + healEffectSPos, 0);
            posList0.Add(pos);
        }
    }

    void makeRandomPosList()
    {
        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            Vector3 pos = new Vector3(half_width * 2 - 70 * i - healEffectSPos, 10 * i + healEffectSPos, 0);
            posList.Add(pos);
        }

        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            Vector3 pos = new Vector3(half_width + 70*i + healEffectSPos, half_height * 2 - 10*i - healEffectSPos, 0);
            posList.Add(pos);
        }

        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            Vector3 pos = new Vector3(half_width + 70 * i, 10 * i + healEffectSPos, 0);
            posList.Add(pos);
        }
    }

    void makeRandomPosList2()
    {
        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            Vector3 pos = new Vector3(half_width * 2 - (plusImage.sprite.rect.width / 2.0f) * i - healEffectSPos, half_height - 10 * i, 0);
            posList2.Add(pos);
        }

        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            Vector3 pos = new Vector3(70 + 70 * i + healEffectSPos, 10 * i + healEffectSPos, 0);
            posList2.Add(pos);
        }
    }
    void makeHealPlusIcon0()
    {
        timer0 += Time.deltaTime;

        if (timer0 > waiting0)
        {
            makeRandomPosList0();
            for (int i = 0; i < posList0.Count; i++)
            {
                newPlusImage = Instantiate(plusImage, this.transform);
                newPlusImage.rectTransform.position = posList0[i];
            }
            timer0 = 0;
            waiting0 = Random.Range(0.5f, 1.5f);
            posList0.Clear();
        }
    }

    void makeHealPlusIcon()
    {
        timer += Time.deltaTime;
        
        if (timer > waiting)
        {
            makeRandomPosList();
            for (int i = 0; i < posList.Count; i++)
            {
                newPlusImage = Instantiate(plusImage, this.transform);
                newPlusImage.rectTransform.position = posList[i];
            }
            timer = 0;
            waiting = Random.Range(0.5f, 1.5f);
            posList.Clear();
        }
    }

    void makeHealPlusIcon2()
    {
        timer2 += Time.deltaTime;

        if (timer2 > waiting2)
        {
            makeRandomPosList2();
            for (int i = 0; i < posList2.Count; i++)
            {
                newPlusImage = Instantiate(plusImage, this.transform);
                newPlusImage.rectTransform.position = posList2[i];
            }
            timer2 = 0;
            waiting2 = Random.Range(0.5f, 1.5f);
            posList2.Clear();
        }
    }
}
