using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    [SerializeField]
    float rotSpeed = 150.0f;

    float mouseX;
    float mouseY;

    float rotDelta;

    private void Awake()
    {
        rotDelta = rotSpeed * Time.deltaTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        mouseX = transform.eulerAngles.y;
        mouseY = -transform.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        mouseX += h*rotDelta;
        mouseY += v*rotDelta;

        mouseY = Mathf.Clamp(mouseY, -90, 90);

        transform.eulerAngles = new Vector3(-mouseY, mouseX, 0);
    }
}
