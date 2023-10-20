using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static bool Load;
    float loadProgress;
    AsyncOperation nextSceneLoad;



    public GameObject MenuUI;

    public GameObject HelpUI;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (nextSceneLoad.isDone)
        {
            GameManager.instance.LoadData();
            Destroy(gameObject);
        }
    }



    public void NewGame()
    {
        nextSceneLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        loadProgress = nextSceneLoad.progress;

        Load = false;
    }

    public void LoadGame()
    {
        nextSceneLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        loadProgress = nextSceneLoad.progress;
        Load = true;

    }


    public void Quit()
    {
        Application.Quit();
    }


    public void controls()
    {
        MenuUI.SetActive(false);
        HelpUI.SetActive(true);
    }

    public void backFromControls()
    {
        MenuUI.SetActive(true);
        HelpUI.SetActive(false);
    }
}
