using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedBehaviour : MonoBehaviour
{
    public float damage, knockbackSpeed, immuneTime;
    [SerializeField] private HealthController healthController;
    private bool immune = false;
    private Movement movement;

    private void Start()
    {
        movement = this.GetComponent<Movement>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.transform.tag.Equals("Attack") || immune) { return; }
        StartCoroutine(Immunity());
        healthController.linearlyChangeHealth(-damage, 0.4f);
        movement.Attacked(other.transform.position, knockbackSpeed);
    }
    IEnumerator Immunity()
    {
        immune = true;
        yield return new WaitForSeconds(immuneTime);
        immune = false;
    }
}
