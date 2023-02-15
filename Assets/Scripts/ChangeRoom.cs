using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoom : MonoBehaviour
{
    public GameObject original;
    public GameObject other;
    private CharacterController cc;
    public bool sightActive;
    public bool abilityGotten = false;
    private float heightDiff;
    public Fade fade;
    GameObject eyeClosed, eyeOpen, eyeGlow, eyeKey;

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
        Transform eye = GameObject.Find("Canvas").transform.GetChild(0);
        eyeClosed = eye.GetChild(0).gameObject;
        eyeOpen = eye.GetChild(1).gameObject;
        eyeGlow = eye.GetChild(2).gameObject;
        eyeKey = eye.GetChild(3).gameObject;
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

        original.transform.position = sightActive ? this.transform.position - new Vector3(0, heightDiff, 0) : this.transform.position;
        other.transform.position = sightActive ? this.transform.position : this.transform.position + new Vector3(0, heightDiff, 0);


        if (abilityGotten && Input.GetKeyDown(KeyCode.F))
        {
            this.transform.position = !sightActive ? other.transform.position : original.transform.position;
            cc.enabled = false;

            cc.enabled = true;
            applyFilters(!sightActive);
            sightActive = !sightActive;

            print(sightActive);
            eyeClosed.SetActive(!sightActive);
            eyeOpen.SetActive(sightActive);
            eyeGlow.SetActive(sightActive);
        }
    }

    public void activate()
    {
        abilityGotten = true;
        print("Alternate Sight Ability Gained");
        eyeClosed.SetActive(true);
        eyeKey.SetActive(true);
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
