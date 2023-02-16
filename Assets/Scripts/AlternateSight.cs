using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AlternateSight : MonoBehaviour
{
    public GameObject original;
    public GameObject other;
    private CharacterController cc;
    public bool abilityGotten, sightActive;
    private float heightDiff;
    public Fade fade;
    GameObject eye, eyeClosed, eyeOpen, eyeGlow;

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
        else
        {
            Debug.LogError("Original object or other object does not exist");
        }
        cc = GetComponent<CharacterController>();
        eye = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        var eyeTransform = eye.transform;
        eyeClosed = eyeTransform.GetChild(0).gameObject;
        eyeOpen = eyeTransform.GetChild(1).gameObject;
        eyeGlow = eyeTransform.GetChild(2).gameObject;
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
        original.transform.position = sightActive ? transform.position - new Vector3(0, heightDiff, 0) : transform.position;
        other.transform.position = sightActive ? transform.position : transform.position + new Vector3(0, heightDiff, 0);
    }

    public void OnAlternateSightPressed(InputAction.CallbackContext ctx)
    {
        if (!(abilityGotten && ctx.performed) || PauseGame.paused) { return; }
        transform.position = sightActive ? original.transform.position : other.transform.position;
        cc.enabled = false;
        cc.enabled = true;
        sightActive = !sightActive;
        applyFilters(sightActive);
        eyeClosed.SetActive(!sightActive);
        eyeOpen.SetActive(sightActive);
        eyeGlow.SetActive(sightActive);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "AlternateSightObject")
        {
            abilityGotten = true;
            eye.SetActive(true);
            Destroy(other.gameObject);
        }
    }
}
