using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject PlayerUI;
    public GameObject mapUI;

    public bool mapOpen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            mapOpen = true;
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            mapOpen = false;
        }

        if (mapOpen)
        {
            mapUI.SetActive(true);
            PlayerUI.SetActive(false);
        }
        else
        {
            mapUI.SetActive(false);
            PlayerUI.SetActive(true);
        }
    }
}
