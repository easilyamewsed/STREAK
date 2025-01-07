using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatySpawning : MonoBehaviour
{
    [SerializeField] GameObject floaty; 
    [SerializeField] float minTimeBetweenSpawns;  
    [SerializeField] float maxTimeBetweenSpawns;

    public List<GameObject> spawnedFloatyNodes;
    public List<GameObject> allNodes;
    public int floatyCap;
    public float startWaitTime;

    private GameObject _floatySpawnNode;


    IEnumerator Start()
    {
        yield return new WaitForSeconds(startWaitTime);
        
        allNodes = new List<GameObject>(GameObject.FindGameObjectsWithTag("Node"));
        

        do
        {
              
            yield return StartCoroutine(SpawnFloaties());
                
        }
        while (allNodes.Count >= 0); //changed this
    }

    IEnumerator SpawnFloaties()
    {
        if (spawnedFloatyNodes.Count < floatyCap)
        {
            do
            {
                _floatySpawnNode = allNodes[Random.Range(0, allNodes.Count)];

            } while (spawnedFloatyNodes.Contains(_floatySpawnNode));

            
            
                Transform _spawnPosition = _floatySpawnNode.transform;
                GameObject _spawnedFloaty = Instantiate(floaty, _spawnPosition.position, Quaternion.identity);
                spawnedFloatyNodes.Add(_floatySpawnNode);
                Floaty _spawnedFloatyScript = _spawnedFloaty.GetComponent<Floaty>();
                _spawnedFloatyScript.floatySpawnNode = _floatySpawnNode;
            
            
         

            yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));
        }
    }


}