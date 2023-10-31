using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool cursorLock = true;

    public GameObject player;
    public GameObject CamController;

    public List<string> currentAreas;

    public static bool found;



    // Start is called before the first frame update
    void Start()
    {

        instance = this.GetComponent<GameManager>();
        player = GameObject.Find("Player");
        found = false;

        if(GameObject.Find("Manager") != null)
        {
            if(MenuManager.Load == true)
            {
                LoadData();
            }
            else
            {
                SaveSystem.ClearData();

                PlayerStats.instance.CheckPoint = GameObject.Find("StartLocation").transform.position;
            }
            Destroy(GameObject.Find("Manager"));
        }

        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        if (cursorLock)
        {
            Cursor.visible = false;
            //Screen.lockCursor = true; 
            Cursor.lockState = CursorLockMode.Locked;
            //player.SetActive(true);
            player.GetComponent<Collider>().enabled = true;
            CamController.SetActive(true);
        }
        else
        {
            Cursor.visible = true;
            //Screen.lockCursor = true; 
            Cursor.lockState = CursorLockMode.None;
            //player.SetActive(false);
            player.GetComponent<Collider>().enabled = false;
            CamController.SetActive(false);

        }
    }
    
    public void AddArea(string area)
    {
        currentAreas.Add(area);
    }

    public void SearchAreas(string area)
    {
        foreach(string location in currentAreas)
        {
            if(area == location)
            {
                found = true;
            }
            else
            {
                found = false;
            }
        }
    }





    public void SaveData()
    {
        SaveSystem.SaveProgress(PlayerStats.instance, Rendering.instance, QuestHolder.instance);
    }

    public void LoadData()
    {
        SaveData data = SaveSystem.LoadData();
        PlayerStats.instance.LoadPlayer(data);
        Rendering.instance.LoadSceneData(data);
    }


}
