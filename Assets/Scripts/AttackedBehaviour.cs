using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedBehaviour : MonoBehaviour
{
    [SerializeField] private HealthController healthController;
    private bool immune = false;
    private Movement movement;

    private void Start()
    {
        movement = this.GetComponent<Movement>();    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("AttackLayer") && !immune)
        {
            immune = true;
            healthController.linearlyChangeHealth(-0.15f, 1);
            StartCoroutine(immuneTime());
            movement.Attacked();
        }
    }

    private IEnumerator immuneTime()
    {
        yield return new WaitForSeconds(1);
        immune = false;
    }
}
