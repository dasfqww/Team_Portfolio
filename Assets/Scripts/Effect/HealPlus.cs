using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealPlus : MonoBehaviour
{
    float timer = 0;
    float waiting = 2.0f;
    float alpha_minus = 0;

    // Start is called before the first frame update
    void Start()
    {
        alpha_minus = this.GetComponent<Image>().color.a;
        waiting = Random.Range(1.5f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        moveUp();
        DisAppear();
    }

    void moveUp()
    {
        this.transform.Translate(Vector2.up * 20.0f * Time.deltaTime);
    }

    void DisAppear()
    {
        alpha_minus = alpha_minus + 0.5f;
        this.GetComponent<Image>().color = new Vector4(this.GetComponent<Image>().color.r,
                                            this.GetComponent<Image>().color.g,
                                            this.GetComponent<Image>().color.b,
                                            this.GetComponent<Image>().color.a * Mathf.Pow(0.9999f, alpha_minus));
        /*spriteRenderer.color = new Vector4(spriteRenderer.color.r, 
                                            spriteRenderer.color.g, 
                                            spriteRenderer.color.b, 
                                            spriteRenderer.color.a - alpha_minus);*/

        timer += Time.deltaTime;
        if(waiting < timer)
        {
            Destroy(this.gameObject);
        }
    }
}
