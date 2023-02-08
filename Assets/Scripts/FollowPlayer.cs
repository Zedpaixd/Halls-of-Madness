using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FollowPlayer : MonoBehaviour
{

    [Tooltip("If null, the target will be the player.")]
    public Transform target;
    public float followRangeMin = 1f;
    public float followRangeMax = 10f;
    public float chaseSpeed = 5f;
    public float rotationSpeed = 5f;
    public bool mustFollow = true;

    public GameObject lightVision;
    public float lightIntensity;

    private Quaternion _lookRotation;
    private Vector3 _direction;

    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        lightIntensity = lightVision.GetComponent<Light>().intensity;
        lightVision.GetComponent<Light>().intensity = 0;
    }

    void Update()
    {
        if (mustFollow)
        {
            // Target is in the range

            float x = Mathf.Abs(target.position.x - this.transform.position.x);
            float z = Mathf.Abs(target.position.z - this.transform.position.z);
            if ((x < followRangeMax && z < followRangeMax)
                && (x > followRangeMin || z > followRangeMin))
            {

                lightVision.GetComponent<Light>().intensity = lightIntensity;
                Follow();
                Rotation();
            }
            else if (x > followRangeMax || z > followRangeMax)
            {
                lightVision.GetComponent<Light>().intensity = 0;
            }
        }
    }

    public void Follow()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z), chaseSpeed * Time.deltaTime);
    }

    private void Rotation()
    {
        _direction = (target.position - transform.position).normalized;
        _direction.y = 0;

        //create the rotation we need to be in to look at the target
        _lookRotation = Quaternion.LookRotation(_direction);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void MustFollow()
    {
        mustFollow = true;
    }

    public void MustNotFollow()
    {
        mustFollow = false;
    }

    public void ChangeMustFollow()
    {
        mustFollow = !mustFollow;
    }

}
