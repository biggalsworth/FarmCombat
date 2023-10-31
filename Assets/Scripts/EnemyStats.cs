using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float health = 10;
    public int damage = 5;
    public int maxFood = 2;
    public int maxSouls = 1;
    public int soulChance = 1;

    public GameObject skin;
    public Material normalSkin;
    public Material hitEffect;
    public GameObject dieEffect;

    public bool change = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Instantiate(dieEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Drop();
        }
    }


    void Drop()
    {
        int food = Random.Range(0, maxFood+1);
        int getSoulChance = Random.Range(1, 21);
        int souls;
        if (getSoulChance <= soulChance)
        {
            souls = Random.Range(1, maxSouls+1);
        }
        else
        {
            souls = 0;
        }

        int healChance = Random.Range(1, 6);
        if(healChance == 4)
        {
            PlayerStats.instance.health = 100;
        }

        PlayerStats.instance.soulAmount += souls;
        PlayerStats.instance.foodAmount += food;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "bullet")
        {
            health -= other.GetComponent<bullet>().damage;
            StartCoroutine(hit());
        }
    }

    IEnumerator hit()
    {
        if (change)
        {
            skin.GetComponent<Renderer>().material = hitEffect;
            yield return new WaitForSeconds(0.1f);
            skin.GetComponent<Renderer>().material = normalSkin;
        }
    }
}
