using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAI : MonoBehaviour
{

    public float[] followRangesMax = {10f, 15f, 20f, 25f, 30f, 35f};
    public float[] chaseSpeeds = {5f, 5.5f, 6f, 7f, 8f, 9f};

    public int currentState = 0;

    #region States
    [Header("States")]
    private readonly int state0 = 0;
    private readonly int state1 = 1;
    private readonly int state2 = 2;
    private readonly int state3 = 3;
    private readonly int state4 = 4;
    #endregion

    #region Max sanity value
    [Header("Max sanity value")]
    public int state0Max = 20;
    public int state1Max = 40;
    public int state2Max = 60;
    public int state3Max = 80;
    #endregion

    #region Attack Rate
    [Header("Attack Rate")]
    public float attackRate = 2;
    private float attackCount;
    #endregion

    //temporary
    [Header("TEMPORARY")]
    public float playerSanity = 0;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SanityCheck();

        if (attackCount > 0)
        {
            attackCount -= Time.deltaTime * attackRate;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (attackCount <= 0)
        {
            playerSanity += 10;
            Debug.Log("Ouch!");

            attackCount = 1;
        }
    }

    void SanityCheck()
    {
        if (playerSanity <= state0Max)
        {
            if (currentState != state0)
            {
                ChangeState(state0);
            }  
        }
        else if (playerSanity > state0Max && playerSanity <= state1Max)
        {
            if (currentState != state1)
            {
                ChangeState(state1);
            }
        }
        else if (playerSanity > state1Max && playerSanity <= state2Max)
        {
            if (currentState != state2)
            {
                ChangeState(state2);
            }
        }
        else if (playerSanity > state2Max && playerSanity <= state3Max)
        {
            if (currentState != state3)
            {
                ChangeState(state3);
            }
        }
        else
        {
            if (currentState != state4)
            {
                ChangeState(state4);
            }
        }
    }

    void ChangeState(int newState)
    {
        currentState = newState;
        this.GetComponent<FollowPlayer>().followRangeMax = followRangesMax[newState];
        this.GetComponent<FollowPlayer>().chaseSpeed = chaseSpeeds[newState];
    }
}
