using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;


    public int foodAmount;
    public int soulAmount;
    public int health;
    public int regenTime = 20;
    private bool healing;
    public Vector3 CheckPoint;

    [Header("UI")]
    public TextMeshProUGUI foodCounter;
    public TextMeshProUGUI healthTracker;
    public TextMeshProUGUI soulCounter;



    [Header("Inventory")]
    public List<string> inv;

    public List<string> bosses;


    public string activeAbil;

    private PlayerAbilities abil;
    private Notify notice;

    //To check if the player has played before.
    public bool PlayedBefore;

    // Start is called before the first frame update
    void Start()
    {
        //CheckPoint = GameObject.Find("StartLocation").transform.position;
        instance = this.GetComponent<PlayerStats>();
        abil = PlayerAbilities.instance;
        notice = Notify.instance;
        health = 100;
        activeAbil = "blank";

        CowMovementDecider.instance.anim.Play("idle");

    }

    // Update is called once per frame
    void Update()
    {
        if(MenuManager.Load == false)
        {
            PlayedBefore = false;
        }
        if(health < 1)
        {
            health = 0;
            PlayerDeath.instance.Die();
        }

        if(health > 100)
        {
            health = 100;
        }

        foodCounter.text = foodAmount.ToString();
        healthTracker.text = health.ToString();
        soulCounter.text = soulAmount.ToString();



        //Checking Weapon Equip
        WeaponEquip();




        //healing
        if (health < 100 && healing == false)
        {
            StartCoroutine(heal());
        }
    }



    public void LoadPlayer(SaveData data)
    {

        health = data.playerHealth;
        foodAmount = data.foodAmount;
        soulAmount = data.soulAmount;



        CheckPoint = new Vector3 (data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);
        gameObject.GetComponent<ThirdPersonMovement>().enabled = false;
        transform.position = CheckPoint;
        gameObject.GetComponent<ThirdPersonMovement>().enabled = true;


        //inv.Capacity = data.inv.Length;
        for(int i = 0; i<data.inv.Length; i++)
        {
            //inv[i] = data.inv[i];
            inv.Add(data.inv[i]);
            Debug.Log(data.inv[i]);
        }

        for (int i = 0; i < data.bossList.Length; i++)
        {
            bosses.Add(data.bossList[i]);
        }

        abil.standing = false;
        PlayedBefore = data.Played;

        QuestHolder.instance.quest.title = data.Title;
        QuestHolder.instance.quest.descr = data.Descr;
    }








    public void addItem(string items)
    {
        inv.Add(items);
        /*
        for (int i = 0; i < inv.Count; i++)
        {
            for (int ii = 0; ii < PlayerAbilities.instance.possibleSkills.Count; ii++)
            {
                Debug.Log(PlayerAbilities.instance.possibleSkills[ii].name);
                if (inv[i] == PlayerAbilities.instance.possibleSkills[ii].name)
                {
                    PlayerAbilities.instance.possibleSkills[ii].active = true;
                }
            }
        }
        */
    }


    public void Hit()
    {
        if (abil.dashing != true)
        {
            //normal hit marking
        }
    }

    private IEnumerator heal()
    {
        healing = true;
        yield return new WaitForSeconds(regenTime);
        health++;
        healing = false;
    }




    public void WeaponEquip()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && abil.milk.active == true)
        {
            for(int i = 0; i< abil.possibleSkills.Count; i++)
            {
                if(activeAbil == abil.possibleSkills[i].name)
                {
                    abil.possibleSkills[i].weapon.SetActive(false);
                }
            }
            if (activeAbil != "milk")
            {
                activeAbil = "milk";
            }
            else
            {
                activeAbil = "";
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && abil.milk.active == false)
        {
            notice.Message("Buy this weapon from barn first", 5);
        }



        if (Input.GetKeyDown(KeyCode.Alpha2) && abil.pistol.active == true)
        {
            if (activeAbil != "pistol")
            {
                for (int i = 0; i < abil.possibleSkills.Count; i++)
                {
                    if (activeAbil == abil.possibleSkills[i].name)
                    {
                        abil.possibleSkills[i].weapon.SetActive(false);
                    }
                }
                if (abil.standing == true)
                {
                    CowMovementDecider.instance.UnStand();
                }
                //All the defining for the active abil to be made pistol is done in animations.cs
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && abil.pistol.active == false)
        {
            notice.Message("Buy this weapon from barn first", 5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && activeAbil != "pistol" && abil.standing == true && abil.pistol.active == true)
        {
            activeAbil = "";
            notice.Message("Stop Standing to use this weapon", 2);
        }










    }
}
