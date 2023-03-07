using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class FollowPlayer : MonoBehaviour
{

    [Tooltip("If null, the target will be the player.")]

    private Transform startPosition;
    private Transform playerPosition;
    public Transform target;
    public float followRangeMin = 1f;
    public float followRangeMax = 10f;
    public float chaseSpeed = 5f;
    public float rotationSpeed = 0.75f;
    public bool mustFollow = true;

    private NavMeshAgent navigation;

    private Quaternion _lookRotation;
    private Vector3 _direction;

    void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        if (target == null)
        {
            target = playerPosition;
        }
        navigation = GetComponent<NavMeshAgent>();
        startPosition = this.transform;
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
        navigation.SetDestination(target.position);
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

    public void TargetPlayer()
    {
        target = playerPosition;
    }

    public void BackToStart()
    {
        target = startPosition;
    }
}
