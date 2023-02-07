using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncLoading : MonoBehaviour
{

    [SerializeField] public int startLoad = 0;
    [SerializeField] public bool teleport = false;
    [SerializeField] private Fade fade;
    //[SerializeField] private GameObject player;
    //[SerializeField] private PlayerCoords playerCoords;


    private void Start()
    {

        //if (!playerCoords) playerCoords = GameObject.Find("PlayerInfo").GetComponent<PlayerCoords>();

        //fade.StartFadeOut();
        /*if (playerCoords.playerTransform != null)
        {
            
            player.transform.SetPositionAndRotation(playerCoords.playerTransform.position, playerCoords.playerTransform.rotation);
            player.transform.localScale = playerCoords.playerTransform.localScale;
        }*/
    }

    private void Awake()
    {
        fade.StartFadeOut();
    }

    private void Update()
    {

        //Debug.Log(playerCoords.playerTransform);

        if (startLoad == 1)
        {
            StartCoroutine(loadLevelAsync(SceneManager.GetActiveScene().buildIndex + 1));
            startLoad++;
        }
    }

    IEnumerator loadLevelAsync(int index)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(index);
        // Disable auto activation of new scene
        asyncOp.allowSceneActivation = false;

        //Debug.Log("here");

        // Check if done
        while (!asyncOp.isDone)
        {

            // Check progress
            if (asyncOp.progress >= 0.9f && teleport == true)
            {
                //playerCoords.playerTransform = player.transform;
                asyncOp.allowSceneActivation = true;
                //Debug.Log("attempted");
                // Avoid infinite loop
                yield return null;
            }

            // Avoid infinite loop
            yield return null;
        }
    }

}
