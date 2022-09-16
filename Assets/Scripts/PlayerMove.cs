using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;

    [SerializeField]
    float gravity = -10.0f;
    
    float yVelocity = 0;

    [SerializeField]
    float jumpPower = 5.0f;

    bool isJumping = false;

    CharacterController characterController;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);

        dir = Camera.main.transform.TransformDirection(dir);

        yVelocity += gravity * Time.deltaTime;

        if (characterController.collisionFlags==CollisionFlags.Below)
        {
            yVelocity = 0;
            isJumping = false;           
        }

        if (isJumping==false&&Input.GetButtonDown("Jump"))
        {
            yVelocity = jumpPower;
            isJumping = true;
        }

        dir.y = yVelocity;

        characterController.Move(dir * speed * Time.deltaTime);
    }
}
