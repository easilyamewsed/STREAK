using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Node : MonoBehaviour
{
    private FloatySpawning floatySpawning;

    [SerializeField] public List<Node> neighbourNodes;
    [SerializeField] bool spawnDumbEnemy;
    [SerializeField] bool spawnSmartEnemy; 
    [SerializeField] public bool playerSpawn;

    public SpriteRenderer spriteRenderer;

    public GameObject dumbEnemyPrefab;
    public GameObject smartEnemyPrefab;
    private GameObject spawnedEnemy;
    public GameObject nodeToRemove; 

    public Color newColor;
    public Color originalColor; 
    public Color pathColour; 

    public Node parentNode; 

    [SerializeField] bool drawNeighbours;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();  
        originalColor = spriteRenderer.color;             
        floatySpawning = GameObject.FindGameObjectWithTag("Spawner").GetComponent<FloatySpawning>();

        if (spawnDumbEnemy)
        {
            spawnedEnemy = Instantiate(dumbEnemyPrefab, transform.position, Quaternion.identity);
            DumbEnemy spawnedEnemyScript = spawnedEnemy.GetComponent<DumbEnemy>();
            spawnedEnemyScript.node = this;
        }
        if (spawnSmartEnemy) 
        {
            spawnedEnemy = Instantiate(smartEnemyPrefab, transform.position, Quaternion.identity);
            SmartEnemy spawnedEnemyScript = spawnedEnemy.GetComponent<SmartEnemy>();
            spawnedEnemyScript.currentNode = this;
        }
    }

    void Update()
    {
        if (drawNeighbours)
        {
            foreach (Node node in neighbourNodes)
            {
                Debug.DrawRay(transform.position, node.transform.position - transform.position, Color.green, 0.3f);
            }
        }
    }

    private void OnValidate()
    {
        if(spriteRenderer != null)
        {
            
            

            if (spawnDumbEnemy)
            {
                spriteRenderer.color = newColor;
            }
            else
            {
                spriteRenderer.color = originalColor;
            }
        }
        foreach (Node node in neighbourNodes)           
        {
            if (!node.neighbourNodes.Contains(this))
            {
                node.neighbourNodes.Add(this);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Player")) //added here
        {
            nodeToRemove = this.gameObject;
            floatySpawning.allNodes.Remove(nodeToRemove);
            //Debug.Log("enemy enter");
            
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Player")) //added here
        {
            nodeToRemove = this.gameObject;
            floatySpawning.allNodes.Add(nodeToRemove);
            //Debug.Log("enemy exit");
            
        }
    }
}