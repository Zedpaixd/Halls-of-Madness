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
    public float rotationSpeed = 0.75f;
    public bool mustFollow = true;

    private Quaternion _lookRotation;
    private Vector3 _direction;

    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    public void Move()
    {  
        if (mustFollow)
        {
            // Target is in the range

            float x = Mathf.Abs(target.position.x - this.transform.position.x);
            float z = Mathf.Abs(target.position.z - this.transform.position.z);
            if ((x < followRangeMax && z < followRangeMax)
                && (x > followRangeMin || z > followRangeMin))
            {
                Follow();
                Rotation();
            }
        }   
    }

    public bool CanMove()
    {
        float x = Mathf.Abs(target.position.x - this.transform.position.x);
        float z = Mathf.Abs(target.position.z - this.transform.position.z);
        if (x > followRangeMin || z > followRangeMin)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Follow()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z), chaseSpeed * Time.deltaTime);
    }

    public void Rotation()
    {
        _direction = (target.position - transform.position).normalized;
        _direction.y = 0;

        //create the rotation we need to be in to look at the target
        _lookRotation = Quaternion.LookRotation(_direction);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
    }

    public void Rotation(float slowRotationSpeed)
    {
        _direction = (target.position - transform.position).normalized;
        _direction.y = 0;

        //create the rotation we need to be in to look at the target
        _lookRotation = Quaternion.LookRotation(_direction);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * slowRotationSpeed);
    }

}
