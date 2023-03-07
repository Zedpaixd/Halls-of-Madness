using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour
{
    private GameObject player;
    private Movement playerMovement;

    #region Vision
    [Header("Vision")]
    public float viewRadius;
    public float viewAngle = 60f;
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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<Movement>();
        string[] playerMaskNames = { "Player" };
        playerMask = LayerMask.GetMask(playerMaskNames);

        lightIntensity = this.GetComponent<Light>().intensity;
        this.GetComponent<Light>().intensity = 0;
    }

    public bool View()
    {
        Vector3 playerTarget = (player.transform.position - transform.position).normalized;

        if (Vector3.Angle(transform.forward, playerTarget) < viewAngle)
        {
            float distanceToTarget = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToTarget <= viewRadius)
            {
                if (Physics.Raycast(transform.position, playerTarget, distanceToTarget, obstacleMask) == false)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool Hear()
    {
        float hearDistance = Vector3.Distance(transform.position, player.transform.position);

        if (hearDistance <= hearRadius && playerMovement.moving && playerMovement.sprinting)
        {
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

}
