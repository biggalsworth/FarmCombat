using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    public static EnemySpawning instance;

    public bool allowSpawn = true;

    public int enemyLimit;
    public int enemyCount;
    private GameObject[] enemies;

    public int spawnDistance;
    private Vector3 spawnPos;
    public int noSpawn;

    private GameObject player;


    [Space]
    [Header ("Enemies")]
    public GameObject Enemy1;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        instance = this.GetComponent<EnemySpawning>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gameObject.name);
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCount = enemies.Length;
        if(enemies.Length < enemyLimit && allowSpawn == true)
        {
            float xRand = Random.Range(player.transform.position.x - spawnDistance, player.transform.position.x + spawnDistance);
            float zRand = Random.Range(player.transform.position.z - spawnDistance, player.transform.position.z + spawnDistance);


            //to make sure the enemy doesn't spawn within the no spawn radius
            spawnPos = new Vector3(xRand, player.transform.position.y, zRand); //gets the potential spawn location
            float distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, spawnPos); //checks the distance between spawn location and player location
            if (distance > noSpawn)
            {
                spawnPos = new Vector3(xRand, 100, zRand);
                RaycastHit hit;
                // note that the ray starts at 100 units
                Ray ray = new Ray(spawnPos + Vector3.up * 100, Vector3.down);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.collider.name.Contains("Terrain"))
                    {
                        // this is where the gameobject is actually put on the ground
                        spawnPos.Set(spawnPos.x, hit.point.y + 3, spawnPos.z);
                    }
                }
                if (enemyCount < enemyLimit)
                {
                    Instantiate(Enemy1, spawnPos, Quaternion.identity);
                }
            }
        }
        else
        {
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(GameObject.Find("Player").transform.position, enemy.transform.position);
                if (distance > spawnDistance)
                {
                    Destroy(enemy);
                }
            }
        }
    }

    public void DeleteEnemies()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}
