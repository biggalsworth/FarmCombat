using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUIShow : MonoBehaviour
{
    public GameObject questUI;
    public GameObject playerUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && PauseScreen.paused == false)
        {
            Time.timeScale = 0;
            questUI.SetActive(true);
            playerUI.SetActive(false);
            GameManager.instance.cursorLock = false;
        }
    }
}
