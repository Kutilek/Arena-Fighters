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
    private Enemy_Counter enemyCounter;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyCounter = GameObject.FindGameObjectWithTag("EnemyCounter").GetComponent<Enemy_Counter>();
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
        if (enemyCounter.enemies.Length <= 40)
        {
            for (int i = 0; i < numberOfSpawns; i++)
            {
                float rand = Random.Range(-1f, 1f);
                Instantiate(spawnObject, spawnPoint.position + new Vector3(rand, 0f, 0f), Quaternion.identity);
            }
        }  
        StartCoroutine(CloseEntrance());   
    }

    private IEnumerator CloseEntrance()
    {
        yield return new WaitForSeconds(1f);

        animator.SetTrigger("close");
    }
}
