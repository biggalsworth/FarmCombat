using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHolder : MonoBehaviour
{
    public static QuestHolder instance;

    public QuestClass quest;


    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        instance = this.GetComponent<QuestHolder>();
    }

    public void Awake()
    {
        newQuest("Time To Work", "Get some food from around the area.");
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStats.instance.PlayedBefore == false)
        {
            PlayerStats.instance.PlayedBefore = true;
        }
        //Debug.Log(quest.title);
    }

    public void newQuest(string title, string descr)
    {
        quest = new QuestClass(title, descr);
        anim.Play("QuestNotification");
        Debug.Log("New      Misson");
        //return quest;
    }
}
