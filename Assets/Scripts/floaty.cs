using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floaty : MonoBehaviour
{
    public ProgressBar progressBar;

    private PlayerMovement player;
    [SerializeField] public Transform sealJumpPoint;
    
    public GameObject floatySpawnNode;
    private FloatySpawning floatySpawning;

    void Start()
    {
        progressBar = FindObjectOfType<ProgressBar>();
        floatySpawning = GameObject.FindGameObjectWithTag("Spawner").GetComponent<FloatySpawning>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("JumpMarker") && !player.isJumping)
        {
            player.targetFloaty = this;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("JumpMarker") && !player.isJumping)
        {
            player.targetFloaty = null;
        }
    }

    public void Destroy()
    {
        //alex, the code to affect the scoring system goes here here
        //because the score is added right before the floaty is destroyed. 
        
        floatySpawning.spawnedFloatyNodes.Remove(floatySpawnNode);
        progressBar.CollectibleGrabbed();
        Destroy(gameObject);
    }
}