using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour
{
    private GameObject player;

    #region Vision
    [Header("Vision")]
    public float viewRadius;
    public float viewAngle = 90f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    #endregion

    #region Hearing
    [Header("Hearing")]
    public float hearRadius = 40f;
    #endregion


    #region Light vision
    [Header("Light vision")]
    public float lightIntensity;
    #endregion

    [Header("TEMPORARY INPUT LISTENER")]
    public float playerSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        string[] playerMaskNames = { "Player" };
        playerMask = LayerMask.GetMask(playerMaskNames);
        string[] obstacleMaskNames = { "Default", "Pickupable" };
        obstacleMask = LayerMask.GetMask(obstacleMaskNames);

        lightIntensity = this.GetComponent<Light>().intensity;
        this.GetComponent<Light>().intensity = 0;
    }

    public bool View()
    {
        Vector3 playerTarget = (player.transform.position - transform.position).normalized;

        if (Vector3.Angle(transform.forward, playerTarget) < viewAngle / 2)
        {
            float distanceToTarget = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToTarget <= viewRadius)
            {
                Debug.DrawRay(transform.position, playerTarget);
                if (Physics.Raycast(transform.position, playerTarget, distanceToTarget, obstacleMask) == false)
                {
                    //Debug.Log(distanceToTarget);

                    return true;
                }
            }
        }
        return false;
    }

    public bool Hear()
    {
        float hearDistance = Vector3.Distance(transform.position, player.transform.position);
        
     /**/   checkWalk();

     /**/  if (hearDistance <= hearRadius && playerSpeed > 0.5)
        {
           // Debug.Log(hearDistance);
            return true;
        }
        return false;
    }
    public void SwitchLightIntensity(bool turnOn)
    {
        if (turnOn)
        {
            this.GetComponent<Light>().intensity = lightIntensity;
        }
        else
        {
            this.GetComponent<Light>().intensity = 0;
        }
    }

    /*TEMPORARY*/

    private void checkWalk()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (playerSpeed >= 0.5)
                playerSpeed = 0.1f;
            else
                playerSpeed = 1;
        }
    }

}
