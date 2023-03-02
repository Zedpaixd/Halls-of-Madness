using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouDied : MonoBehaviour
{
    public Fade fade;
    public CanvasGroup CG;

    private void Start()
    {
        CG = fade.GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (CG.alpha >= 0.99f)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            fade.StartFadeIn();
        }
    }
}
