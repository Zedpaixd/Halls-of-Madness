using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteAppear : MonoBehaviour
{
    [SerializeField]
    private GameObject noteImage;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")){
            noteImage.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")){
            noteImage.gameObject.SetActive(false);
        }
    }
}
