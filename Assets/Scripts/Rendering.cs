using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rendering : MonoBehaviour
{
    public static Rendering instance;
    public List<string> activatedAreas;
    // Start is called before the first frame update
    void Start()
    {
        instance = this.GetComponent<Rendering>();
        //LoadSceneData();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = GameObject.Find("Player").transform.position;
        for(int i=0; i<activatedAreas.Count; i++)
        {
            //GameObject.Find(activatedAreas[i]).SetActive(true);
            Debug.Log("cool");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            activatedAreas.Add(other.gameObject.name);
        }
        //other.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
            EnemySpawning.instance.enemyCount -= 1;
        }
        else
        {
            activatedAreas.Remove(other.gameObject.name);
            other.gameObject.SetActive(false);
        }

    }



    public void LoadSceneData(SaveData data)
    {
        activatedAreas.Capacity = data.activeAreas.Length;

        for(int i = 0; i<data.activeAreas.Length; i++)
        {
            activatedAreas[i] = data.activeAreas[i];
        }
    }
    
}
