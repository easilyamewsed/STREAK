using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : MonoBehaviour
{
    public GameObject iceSpawnNode;
    private IceSpawning iceSpawner;
    private PlayerMovement player;
    private BoxCollider2D col;

    [SerializeField] int minLifeTime;
    [SerializeField] int maxLifeTime;

    Animator animator;
    AudioSource audioSource;

    [SerializeField] AudioClip formSound;
    [SerializeField] AudioClip smashSound;

    IEnumerator Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(formSound);
        iceSpawner = GameObject.FindGameObjectWithTag("IceyWeather").GetComponent<IceSpawning>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();

        yield return new WaitForSeconds(Random.Range(minLifeTime, maxLifeTime));
        animator.SetTrigger("destroy");
        col.enabled = false;

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("JumpMarker"))
        {
            player.jumpEnabled = false;
            player.jumpMarkerSpriteRenderer.color = Color.red;
        }
        if (col.gameObject.CompareTag("Bucky"))
        {
            Debug.Log("bucky walked into ice");
            audioSource.PlayOneShot(smashSound);
            Smash();
        }

    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("JumpMarker"))
        {
            player.jumpEnabled = true;
            player.jumpMarkerSpriteRenderer.color = Color.green;
        }
    }

    public void Destroy()
    {
        iceSpawner.spawnedIceNodes.Remove(iceSpawnNode);
        Destroy(gameObject);
    }

    public void Smash()
    {
        animator.SetTrigger("smash");
    }
    
 
}