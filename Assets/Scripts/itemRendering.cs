using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemRendering : MonoBehaviour
{
    public GameObject item1;
    public string item1Tag;
    public string item1SpawnArea = "first";
    public int item1Limit;
    private GameObject[] item1List;
    private int item1Count;

    [Space]
    public int spawnDistance = 800;
    public int noSpawn = 200;

    private GameObject player;
    private Vector3 spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        item1List = GameObject.FindGameObjectsWithTag(item1Tag);
        item1Count = item1List.Length;
        if (item1Count < item1Limit)
        {
            float xRand = Random.Range(player.transform.position.x - spawnDistance, player.transform.position.x + spawnDistance);
            float zRand = Random.Range(player.transform.position.z - spawnDistance, player.transform.position.z + spawnDistance);


            //to make sure the enemy doesn't spawn within the no spawn radius
            spawnPos = new Vector3(xRand, player.transform.position.y, zRand);
            float distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, spawnPos);
            if (distance>noSpawn)
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
                        spawnPos.Set(spawnPos.x, hit.point.y + 5, spawnPos.z);
                    }
                }
                //checking for item1
                if (item1Count < item1Limit)
                {
                    //checks if player is in area for enemy1
                    GameManager.instance.SearchAreas(item1SpawnArea);
                    if (GameManager.found == true)
                    {
                        Instantiate(item1, spawnPos, Quaternion.identity, GameObject.Find("Food").transform);
                    }
                }
            }
        }
        else
        {
            foreach(GameObject item in item1List)
            {
                float distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, item.transform.position);
                if (distance > spawnDistance)
                {
                    Destroy(item);
                }
            }
        }


    }
}
