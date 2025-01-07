using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerup : MonoBehaviour
{
    PlayerMovement player;
    public GameObject spawnNode;
    PowerupSpawning powerUpSpawner;
    //AudioSource audioSource;

    //[SerializeField] AudioClip collectedPU;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        powerUpSpawner = GameObject.FindGameObjectWithTag("PowerupSpawner").GetComponent<PowerupSpawning>();
        //audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            powerUpSpawner.spawnedPowerupNodes.Remove(spawnNode);
            powerUpSpawner.numberOfSpawnedPUs--;

            if (!player.isSpeedy)
            {
                player.powerUpCoroutineActice = true;
            }
            
            //audioSource.PlayOneShot(collectedPU);   
            Destroy(gameObject);
        }
    }


}