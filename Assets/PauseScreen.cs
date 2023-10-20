using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseScreen;
    public GameObject mainPauseScreen;
    public GameObject playerUI;
    public GameObject store1;
    public GameObject store2;
    public GameObject helpScreen;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && store1.activeSelf == false && store2.activeSelf == false)
        {
            if(paused == false)
            {
                GameManager.instance.cursorLock = false;
                Time.timeScale = 0f;
                pauseScreen.SetActive(true);
                playerUI.SetActive(false);
                paused = true;
            }
            else if(helpScreen.activeSelf == true)
            {
                helpScreen.SetActive(false);
                mainPauseScreen.SetActive(true);
            }
            else
            {
                GameManager.instance.cursorLock = true;
                Time.timeScale = 1f;
                pauseScreen.SetActive(false);
                playerUI.SetActive(true);
                paused = false;

            }

        }
    }

    public void Resume()
    {
        GameManager.instance.cursorLock = true;
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
        playerUI.SetActive(true);
        paused = false;
    }

    public void Help()
    {
        mainPauseScreen.SetActive(false);
        helpScreen.SetActive(true);
    }

    public void Quit()
    {
        GameManager.instance.SaveData();
        Application.Quit();
    }

    public void Back()
    {
        helpScreen.SetActive(false);
        mainPauseScreen.SetActive(true);
    }
}
