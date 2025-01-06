using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawning : MonoBehaviour
{
    FloatySpawning floatySpawning;
    public List<GameObject> allNodes;
    public List<GameObject> spawnedPowerupNodes;

    [SerializeField] int cap;
    public int numberOfSpawnedPUs = 0;
    private GameObject pUSpawnNode;

    [SerializeField] List<GameObject> powerUps;

    [SerializeField] float minTimeBetweenSpawns;
    [SerializeField] float maxTimeBetweenSpawns;
    [SerializeField] float startWaitTime;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(startWaitTime);

        floatySpawning = GameObject.FindGameObjectWithTag("Spawner").GetComponent<FloatySpawning>();
        allNodes = floatySpawning.allNodes;
        spawnedPowerupNodes = floatySpawning.spawnedFloatyNodes;

        do
        {
            yield return StartCoroutine(SpawnPUs());

        }
        while (allNodes.Count >= 0);
    }

    IEnumerator SpawnPUs()
    {
        if (numberOfSpawnedPUs < cap)
        {
            do
            {
                if (allNodes.Count >= 0)
                {
                    pUSpawnNode = allNodes[Random.Range(0, allNodes.Count)];
                }



            } while (spawnedPowerupNodes.Contains(pUSpawnNode));



            Transform _spawnPosition = pUSpawnNode.transform;
            numberOfSpawnedPUs++;
            GameObject _spawnedPU = Instantiate(powerUps[Random.Range(0, powerUps.Count)], _spawnPosition.position, Quaternion.identity);
            spawnedPowerupNodes.Add(pUSpawnNode);
            //IceBlock iceBlockScript = _spawnedPU.GetComponent<IceBlock>();
            //iceBlockScript.iceSpawnNode = iceSpawnNode;
            if (_spawnedPU.CompareTag("HealthUP"))
            {
                HealthPowerUp script = _spawnedPU.GetComponent<HealthPowerUp>();
                script.spawnNode = pUSpawnNode;
            }
            else if (_spawnedPU.CompareTag("SpeedUP"))
            {
                SpeedPowerup script = _spawnedPU.GetComponent<SpeedPowerup>();
                script.spawnNode = pUSpawnNode;
            }
            


            yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));
        }
    }
}