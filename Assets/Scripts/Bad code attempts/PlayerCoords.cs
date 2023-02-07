using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoords : MonoBehaviour
{
    [HideInInspector] public Transform playerTransform;
    [SerializeField] private Vector3 coords;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        playerTransform.position = new Vector3(1, 1, 1);
    }
    private void Update()
    {
        if (playerTransform) coords = playerTransform.position;
    }
}
