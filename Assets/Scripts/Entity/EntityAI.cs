using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class EntityAI : MonoBehaviour
{

    public float[] viewRanges = {10f, 15f, 20f, 25f, 30f};
    public float[] chaseSpeeds = {3f, 5f, 5.5f, 6f, 7f};
    public float[] sanityStateMax = {20f, 40f, 60f, 80f};

    #region States
    public int currentState = 0;

    private readonly int state0 = 0;
    private readonly int state1 = 1;
    private readonly int state2 = 2;
    private readonly int state3 = 3;
    private readonly int state4 = 4;

    public bool isAlerted = false;
    #endregion

    private GameObject player;
    [SerializeField]
    private SensorController sensorController;
    [SerializeField]
    private FollowPlayer followPlayer;
    [SerializeField]
    private float rotationSpeed = 2f;

    public EntitySoundController soundController;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sensorController.viewRadius = viewRanges[currentState];
        followPlayer = this.GetComponent<FollowPlayer>();
        soundController = this.GetComponent<EntitySoundController>();
    }

    // Update is called once per frame
    void Update()
    {
     //   SanityCheck();

        if (sensorController.View())
        {
            if (!isAlerted)
            {
                ActiveEntity();
            }

            if (followPlayer.CanMove())
            {
                followPlayer.Follow();
                followPlayer.Rotation();
            }
        }
        else if (sensorController.Hear())
        {
            if (!isAlerted)
            {
                ActiveEntity();
            }

            if (followPlayer.CanMove())
            {
                followPlayer.Follow();
                followPlayer.Rotation(rotationSpeed);
            }
        } 
        else
        {
            if (isAlerted)
            {
                DesactivateEntity();
            }
        }
    }

    private void ActiveEntity()
    {
        isAlerted = true;
        sensorController.SwitchLightIntensity(true); 
        followPlayer.TargetPlayer();

        soundController.Angry();
    }

    private void DesactivateEntity()
    {
        isAlerted = false;
        sensorController.SwitchLightIntensity(false);
        followPlayer.BackToStart();
        
        soundController.Passive();
    }

    /*  void SanityCheck()
      {
          if (playerSanity <= sanityStateMax[state0])
          {
              if (currentState != state0)
              {
                  ChangeState(state0);
              }  
          }
          else if (playerSanity > sanityStateMax[state0] && playerSanity <= sanityStateMax[state1])
          {
              if (currentState != state1)
              {
                  ChangeState(state1);
              }
          }
          else if (playerSanity > sanityStateMax[state1] && playerSanity <= sanityStateMax[state2])
          {
              if (currentState != state2)
              {
                  ChangeState(state2);
              }
          }
          else if (playerSanity > sanityStateMax[state2] && playerSanity <= sanityStateMax[state3])
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
  */
   /* void ChangeState(int newState)
    {
        currentState = newState;

        sensorController.viewRadius = viewRanges[currentState];
        followPlayer.chaseSpeed = chaseSpeeds[currentState];
    }
   */
}
