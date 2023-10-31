using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public GameObject questUI;
    public GameObject playerUI;

    public Text QuestTitle;
    public Text QuestDesc;

    public static QuestHolder quests;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        quests = QuestHolder.instance;
        QuestTitle.text = quests.quest.title;
        QuestDesc.text = quests.quest.descr;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);

            Time.timeScale = 1;

            gameObject.SetActive(false);
            playerUI.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Back();
        }
    }

    public void Back()
    {
        Time.timeScale = 1;
        GameManager.instance.cursorLock = true;
        gameObject.SetActive(false);
        playerUI.SetActive(true);
    }
}
