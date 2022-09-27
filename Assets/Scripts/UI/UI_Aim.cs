using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Aim : MonoBehaviour
{
    public Image up_aim = null;
    public Image down_aim = null;
    public Image left_aim = null;
    public Image right_aim = null;

    Vector2 up_init_position;
    Vector2 down_init_position;
    Vector2 left_init_position;
    Vector2 right_init_position;

    [SerializeField]
    [Range(0, 30)]
    public float aim_interval = 15.0f;

    private void Awake()
    {
        up_init_position = up_aim.rectTransform.position;
        down_init_position = down_aim.rectTransform.position;
        left_init_position = left_aim.rectTransform.position;
        right_init_position = right_aim.rectTransform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        aim_success();
    }

    void aim_success()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            up_aim.rectTransform.position = new Vector2(up_aim.rectTransform.position.x,
                                                        up_init_position.y + aim_interval);
            down_aim.rectTransform.position = new Vector2(up_aim.rectTransform.position.x,
                                                        down_init_position.y - aim_interval);
            left_aim.rectTransform.position = new Vector2(left_init_position.x - aim_interval,
                                                        left_aim.rectTransform.position.y);
            right_aim.rectTransform.position = new Vector2(right_init_position.x + aim_interval,
                                                        right_aim.rectTransform.position.y);
        }
        else
        {
            up_aim.rectTransform.position = up_init_position;
            down_aim.rectTransform.position = down_init_position;
            left_aim.rectTransform.position = left_init_position;
            right_aim.rectTransform.position = right_init_position;
        }
    }
}
