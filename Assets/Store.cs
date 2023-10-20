using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{

    public new string name;
    public int cost;

    private Button obj;

    private List<string> inv;

    public bool soul = false;

    [Header("Only include if its the exit button")]
    public GameObject playerUI;
    // Start is called before the first frame update
    void Start()
    {
        obj = gameObject.GetComponent<Button>();
        inv = PlayerStats.instance.inv;
    }

    // Update is called once per frame
    void Update()
    {
        bool found = false;
        for (int i = 0; i < inv.Count; i++)
        {
            if (inv[i] == name)
            {
                found = true;
                break; //don't need to check the remaining ones now that we found one
            }
        }

        if (!soul)
        {
            if (PlayerStats.instance.foodAmount < cost || found == true)
            {
                obj.interactable = false;
            }
        }
        else if (soul)
        {
            if (PlayerStats.instance.soulAmount < cost || found == true)
            {
                obj.interactable = false;
            }
        }
        else
        {
            obj.interactable = true;
        }

        for (int i = 0; i < (PlayerStats.instance.inv).Count; i++)
        {
            if (PlayerStats.instance.inv[i] == name)
            {
                gameObject.GetComponent<Button>().enabled = false;
            }
        }


    }

    public void Buy()
    {
        
        if(QuestHolder.instance.quest.title == "Time To Grow")
        {
            QuestHolder.instance.newQuest("The Chicken Lord", "Find and defeat the chicken lord when you are ready.");
            GameManager.instance.SaveData();
        }
        if (!soul)
        {
            if (PlayerStats.instance.foodAmount >= cost)
            {
                PlayerStats.instance.addItem(name);
                PlayerStats.instance.foodAmount -= cost;
            }
        }
        else
        {
            if (PlayerStats.instance.soulAmount >= cost)
            {
                PlayerStats.instance.addItem(name);
                PlayerStats.instance.soulAmount -= cost;
            }
        }
        
    }

    public void CloseMenu()
    {
        if(GameObject.Find("StoreUI") == null)
        {

            GameObject.Find("SkillStoreUI").SetActive(false);
        }
        else
        {
            GameObject.Find("StoreUI").SetActive(false);
        }
        playerUI.SetActive(true);
        CowMovementDecider.instance.anim.Play("idle");
        PlayerAbilities.instance.standing = false;
        GameManager.instance.cursorLock = true;
        GameManager.instance.SaveData();

        GameObject.Find("Player").GetComponent<MapController>().enabled = true;
        EnemySpawning.instance.allowSpawn = true;


    }
}
