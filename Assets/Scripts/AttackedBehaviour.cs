using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedBehaviour : MonoBehaviour
{
    public float damage, knockbackSpeed, immuneTime, launchAngle;
    [SerializeField] private HealthController healthController;
    [SerializeField] private SanityController sanityController;
    private bool immune = false;
    private Movement movement;

    private void Start()
    {
        movement = this.GetComponent<Movement>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag.Equals("AttackSpot") && !immune)
        {
            StartCoroutine(Immunity());
            healthController.linearlyChangeHealth(-damage, 0.4f);
            movement.Attacked(other.transform.position, knockbackSpeed, launchAngle);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("SanityRecovery"))
        {
            sanityController.linearlyChangeSanity(0.33f, 3);
            Destroy(other.gameObject);
        }
    }
    IEnumerator Immunity()
    {
        immune = true;
        yield return new WaitForSeconds(immuneTime);
        immune = false;
    }
}
