using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryScript : MonoBehaviour
{
    public string territoryName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameManager.instance.AddArea(territoryName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.currentAreas.Remove(territoryName);
        }
    }
}
