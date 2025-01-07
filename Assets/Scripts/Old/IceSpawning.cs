using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpawning : MonoBehaviour
{
    FloatySpawning floatySpawning;
    
    public List<GameObject> allNodes;
    public List<GameObject> spawnedIceNodes;

    [SerializeField] int iceCap;
    private GameObject iceSpawnNode;

    [SerializeField] GameObject ice;

    [SerializeField] float minTimeBetweenSpawns;
    [SerializeField] float maxTimeBetweenSpawns;
    [SerializeField] float startWaitTime;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(startWaitTime);

        floatySpawning = GameObject.FindGameObjectWithTag("Spawner").GetComponent<FloatySpawning>();
        allNodes = floatySpawning.allNodes;
        spawnedIceNodes = floatySpawning.spawnedFloatyNodes;

        do
        {
            yield return StartCoroutine(SpawnIce());

        }
        while (allNodes.Count >= 0);
    }

    IEnumerator SpawnIce()
    {
        if (spawnedIceNodes.Count < iceCap + floatySpawning.floatyCap)
        {
            do
            {
                if (allNodes.Count >= 0)
                {
                    //Debug.Log(allNodes.Count);
                    iceSpawnNode = allNodes[Random.Range(0, allNodes.Count)];
                }
                
                

            } while (spawnedIceNodes.Contains(iceSpawnNode));



            Transform _spawnPosition = iceSpawnNode.transform;
            GameObject _spawnedIce = Instantiate(ice, _spawnPosition.position, Quaternion.identity);
            spawnedIceNodes.Add(iceSpawnNode);
            IceBlock iceBlockScript = _spawnedIce.GetComponent<IceBlock>();
            iceBlockScript.iceSpawnNode = iceSpawnNode;




            yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));
        }
    }


}
