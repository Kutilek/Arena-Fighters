using System.Collections;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    public GameObject spawnObject;
    [SerializeField] private float spawnPeriod;
    [SerializeField] private int numberOfSpawns;
    [SerializeField] private float startSpawnTime;
    private Animator animator;
    private Transform spawnPoint;
    
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spawnPoint = transform.Find("Spawn_Point").transform;
        if (spawnPeriod == 0f)
            spawnPeriod = 10f;
        if (numberOfSpawns == 0)
            numberOfSpawns = 1;
        if (startSpawnTime == 0f)
            startSpawnTime = 20f;
    }

    private void Update()
    {
        if (startSpawnTime + spawnPeriod <= Time.time)
        {
            animator.SetTrigger("open");
            startSpawnTime = Time.time;
        }
    }

    private void StartSpawning()
    {
        for (int i = 0; i < numberOfSpawns; i++)
        {
            float rand = Random.Range(-0.1f, 0.1f);
            Instantiate(spawnObject, spawnPoint.position + new Vector3(rand, 0f, 0f), Quaternion.identity);
        }
            
        StartCoroutine(CloseEntrance());   
    }

    private IEnumerator CloseEntrance()
    {
        yield return new WaitForSeconds(1f);

        animator.SetTrigger("close");
    }
}
