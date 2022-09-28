using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(CharacterController))]
public  class PlayerBase : MonoBehaviour
{
    private CharacterController characterController;

    [SerializeField] float attackAmount;
    [SerializeField] float healAmount;
    [SerializeField] float maxHp;
    float curHp;
    [SerializeField] float maxDurability;
    float curDurability;
    [SerializeField] float shield;

    [SerializeField] float rotSpeed = 100.0f;
    float rotDelta;

    [SerializeField] float moveSpeed = 5.0f;

    [SerializeField]
    enum PlayerType
    {
        Attack,
        Tank,
        Support
    }

    [SerializeField] protected float attackRate;

    // Start is called before the first frame update
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        rotDelta = rotSpeed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float ReturnMaxHp()
    {

        return 1.0f;
    }
}
