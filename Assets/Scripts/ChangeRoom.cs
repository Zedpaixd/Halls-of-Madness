using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoom : MonoBehaviour
{
    public GameObject original;
    public GameObject other;
    private CharacterController cc;
    public bool onOriginal = true;
    public bool activated = false;
    private float heightDiff;
    public Fade fade;

    // <summary> 
    // Alternates the camera between 2 different predefined spots that ideally should be positioned in different spots but
    // similar looking spots such that the illusion will be created. The second spot (other) will be given the movement script
    // so it moves together with the original player object
    // </summary>


    private void Start()
    {
        if (original && other)
        {
            heightDiff = Mathf.Abs(other.transform.position.y - original.transform.position.y);
        }
        cc = GetComponent<CharacterController>();
    }

    void applyFilters(bool enabled)
    {
        fade.StartFadeIn();
        fade.StartFadeOut();
        if (enabled)
        {
            //Debug.Log("Apply filters");
        }
        else
        {
            //Debug.Log("Remove filters");
        }
    }

    private void Update()
    {

        original.transform.position = !onOriginal ? this.transform.position - new Vector3(0, heightDiff, 0) : this.transform.position;
        other.transform.position = !onOriginal ? this.transform.position : this.transform.position + new Vector3(0, heightDiff, 0);


        if (activated && Input.GetKeyDown(KeyCode.F))
        {
            this.transform.position = onOriginal ? other.transform.position : original.transform.position;
            cc.enabled = false;

            cc.enabled = true;
            applyFilters(onOriginal);
            onOriginal = !onOriginal;
            //Debug.Log(heightDiff);
        }
    }

    public void activate()
    {
        activated = true;
        // turn on popup notification
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag == "AlternateRealityTrigger")
        {
            activate();
            Destroy(other.gameObject);
        }

    }
}
