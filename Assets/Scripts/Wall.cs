using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private PlayerMovement player;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("JumpMarker"))
        {
            player.jumpEnabled = false;
            player.jumpMarkerSpriteRenderer.color = Color.red;
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
}