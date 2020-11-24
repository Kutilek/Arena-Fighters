using System.Collections;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    public bool enemzSpawned;
    public GameObject prefab;
    public int numberOfEnemies;

    void Update()
    { 
        if (!enemzSpawned)
        {
            for (int i = 0; i < numberOfEnemies; i++)
                Instantiate(prefab, Vector3.zero, Quaternion.identity);

            enemzSpawned = true;
            StartCoroutine(SpawnEnemy());
        }
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(2f);

        enemzSpawned = false;
    }
}
