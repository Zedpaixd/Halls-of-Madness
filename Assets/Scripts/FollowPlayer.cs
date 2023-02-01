using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [Tooltip("If null, the target will be the player.")]
    public Transform target;
    public float followRangeMin = 1f;
    public float followRangeMax = 10f;
    public float chaseSpeed = 3f;
    public bool mustFollow = true;

    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (mustFollow)
        {
            Follow();
        }
    }

    public void  Follow()
    {
        float x = Mathf.Abs(target.position.x - this.transform.position.x);
        float z = Mathf.Abs(target.position.z - this.transform.position.z);
        if ((x < followRangeMax && z < followRangeMax)
            && (x > followRangeMin || z > followRangeMin)) 
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);
        }
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
        mustFollow = true;
    }

    public void ChangeMustFollow()
    {
        mustFollow = !mustFollow;
    }

}
