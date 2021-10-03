using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] hazardObjects;
    [SerializeField] private float spawnInterval;
    private Coroutine spawnHazardsCoroutine;
    private Vector3 spawnPosition;
    private float timer;

    void Start()
    {
        spawnHazardsCoroutine = StartCoroutine(SpawnHazardsCoroutine());
    }

    IEnumerator SpawnHazardsCoroutine()
    {
        float spawnCooldown = Mathf.Max(spawnInterval - (GameManager.instance.GetLevelCount() * 0.5f), 0.1f);
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(spawnCooldown);
            timer = 0;
        }
    }

    private void Spawn()
    {
        float x = 0, y = 0, targetX = 0, targetY = 0;

        switch (Random.Range(0, 4)) //Selecting one of the 4 edges
        {
            case 0:
                //leftEdge selected
                x = CameraBounds.instance.leftEdge;
                y = Random.Range(CameraBounds.instance.bottomEdge, CameraBounds.instance.topEdge);
                targetX = -x;
                targetY = Random.Range(CameraBounds.instance.bottomEdge, CameraBounds.instance.topEdge);
                break;
            case 1:
                //rightEdge selected
                x = CameraBounds.instance.rightEdge;
                y = Random.Range(CameraBounds.instance.bottomEdge, CameraBounds.instance.topEdge);
                targetX = -x;
                targetY = Random.Range(CameraBounds.instance.bottomEdge, CameraBounds.instance.topEdge);
                break;
            case 2:
                //topEdge selected
                x = Random.Range(CameraBounds.instance.leftEdge, CameraBounds.instance.rightEdge);
                y = CameraBounds.instance.topEdge;
                targetX = Random.Range(CameraBounds.instance.bottomEdge, CameraBounds.instance.topEdge);
                targetY = -y;
                break;
            case 3:
                //bottomEdge selected
                x = Random.Range(CameraBounds.instance.leftEdge, CameraBounds.instance.rightEdge);
                y = CameraBounds.instance.bottomEdge;
                targetX = Random.Range(CameraBounds.instance.bottomEdge, CameraBounds.instance.topEdge);
                targetY = -y;
                break;
            default:
                break;
        }

        Vector3 spawnPos = new Vector3(x, y, 0);
        Vector3 targetPosition = new Vector3(targetX, targetY, 0);

        GameObject obj = Instantiate(hazardObjects[Random.Range(0, hazardObjects.Length)], spawnPos, Quaternion.identity);
        obj.GetComponent<DebrisBehaviour>().SetTargetPos(targetPosition);
    }
}