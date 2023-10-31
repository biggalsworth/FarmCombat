using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    private bool bossFound = false;

    PlayerStats stats;

    public string bossName;

    public GameObject Boss;
    public GameObject SpawnedBoss; //This will be used to keep track of the instantiated boss
    public Transform BossSpawn;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnBoss()
    {
        stats = PlayerStats.instance;
        gameObject.GetComponent<Collider>().enabled = false;

        if (stats.bosses.Count != 0)
        {
            for (int i = 0; i < stats.bosses.Count; i++)
            {
                if (stats.bosses[i] == bossName)
                {
                    bossFound = true;
                }
            }
        }
        
        //only if the boss has not been found on a list of completed bosses - then spawn boss
        if(bossFound != true)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            EnemySpawning.instance.DeleteEnemies();
            EnemySpawning.instance.allowSpawn = false;
            SpawnedBoss = Instantiate(Boss, BossSpawn.position, Quaternion.identity);
        }
        else
        {
            Notify.instance.Message("You had defeated this boss", 3f);
        }
    }
}
