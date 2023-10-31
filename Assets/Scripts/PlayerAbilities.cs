using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    private bool paused;
    public class Skill
    {
        public string name;
        public bool active;
        public GameObject weapon;

        public Skill(string title, bool enabled, GameObject piece)
        {
            name = title;
            active = enabled;
            weapon = piece;
        }
    }


    public static PlayerAbilities instance;

    public bool standing;

    [Header("UI Prompts")]
    public GameObject UI;
    public GameObject StoreUI;
    public GameObject SkillStoreUI;
    public GameObject prompt;

    [Space]

    [Header("Effects")]
    public GameObject foodEffect;
    private GameObject obj;
    public GameObject DashEffect;

    [Space]

    public GameObject blank;

    public GameObject milkBarrel;
    public GameObject pistolSkin;

    [Space]

    [Header("Items")]
    public Skill sprint;// = new Skill("sprint", true, blank);
    public Skill stand;// = new Skill("stand", true, blank);
    public Skill moo;// = new Skill("moo", true, blank);
    public Skill milk;// = new Skill("milk", true, GameObject.Find("milkBarrel"));
    public Skill pistol;// = new Skill("pistol", true, pistolObj);

    //soul skills
    public Skill dash;// = new Skill("dash", true, blank);
    public bool dashing;
    public bool dashDelay;
    public float dashCooldown;
    public Skill hjump;// = new Skill("hjump", true, blank);
    public Skill slow;// = new Skill("slow", true, blank);
    public bool slowReady;
    public float slowCooldown;
    public float slowDuration;

    public List<Skill> possibleSkills = new List<Skill>();

    // Start is called before the first frame update
    void Start()
    {
        dashing = false;
        instance = this.GetComponent<PlayerAbilities>();
        standing = false;


        sprint = new Skill("sprint", false, blank);
        stand = new Skill("stand", false, blank);
        moo = new Skill("moo", false, blank);
        milk = new Skill("milk", false, milkBarrel);
        pistol = new Skill("pistol", false, pistolSkin);


        dash = new Skill("dash", true, blank);
        hjump = new Skill("hjump", true, blank);
        slow = new Skill("slow", true, blank);

        //adds all the skills underneath the "items" header so it can be checked later in playerStats
        possibleSkills.Add(sprint);
        possibleSkills.Add(stand);
        possibleSkills.Add(moo);
        possibleSkills.Add(milk);
        possibleSkills.Add(pistol);

        //soul skills
        possibleSkills.Add(dash);
        possibleSkills.Add(hjump);
        possibleSkills.Add(slow);
    }

    // Update is called once per frame
    void Update()
    {

        //Checking skills

        for (int i = 0; i < PlayerStats.instance.inv.Count; i++)
        {
            for (int ii = 0; ii < possibleSkills.Count; ii++)
            {
                //Debug.Log(possibleSkills[ii].name);
                if (PlayerStats.instance.inv[i] == possibleSkills[ii].name)
                {
                    possibleSkills[ii].active = true;
                }
            }
        }

        for (int i = 0; i < possibleSkills.Count; i++)
        {
            if (possibleSkills[i].weapon.name != "blank")// || PlayerStats.instance.activeAbil != "")
            {
                //GameObject weapon = possibleSkills[i].weapon;
                GameObject weapon = GameObject.Find(possibleSkills[i].weapon.name);

                if (PlayerStats.instance.activeAbil == possibleSkills[i].name && dashing == false)
                {
                    //weapon.SetActive(true);
                    possibleSkills[i].weapon.SetActive(true);
                }
                else
                {
                    //weapon.SetActive(false);
                    possibleSkills[i].weapon.SetActive(false);

                }
                /*
                if (PlayerStats.instance.activeAbil == "pistol" && possibleSkills[i].name == "M1911" && dashing == false) //this special decision is because the pistol gameobject is called an m1911, changing this would mean changing animations and names for the weapon
                {
                    weapon.SetActive(true);
                }
                */
            }
        }
        
        //checks if player is using abilities
        CheckInput();
        




        //interacting
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (prompt.activeSelf)
            {
                if (obj.gameObject.name.Contains("hay"))
                {
                    Instantiate(foodEffect, obj.transform.position, Quaternion.identity);
                    GameObject.Destroy(obj);
                    prompt.SetActive(false);
                    PlayerStats.instance.foodAmount += 1;
                    if (QuestHolder.instance.quest.title == "Time To Work")
                    {
                        QuestHolder.instance.newQuest("Time To Grow", "Keep getting food until you can buy something at the store");
                        Debug.Log("NewMisson");
                        GameManager.instance.SaveData();
                    }
                }

                else if (obj.gameObject.name == "Store")
                {
                    UI.SetActive(false);
                    StoreUI.SetActive(true);
                    GameManager.instance.cursorLock = false;
                    EnemySpawning.instance.DeleteEnemies();
                    EnemySpawning.instance.allowSpawn = false;
                    PlayerStats.instance.CheckPoint = gameObject.transform.position;

                    gameObject.GetComponent<MapController>().enabled = false;
                }
                else if (obj.gameObject.name == "SkillStore")
                {
                    UI.SetActive(false);
                    SkillStoreUI.SetActive(true);
                    GameManager.instance.cursorLock = false;
                    EnemySpawning.instance.DeleteEnemies();
                    EnemySpawning.instance.allowSpawn = false;
                    PlayerStats.instance.CheckPoint = gameObject.transform.position;

                    gameObject.GetComponent<MapController>().enabled = false;
                }

                else if(obj.gameObject.name == "BossSpawnPoint")
                {
                    PlayerStats.instance.CheckPoint = transform.position;
                    SaveSystem.SaveProgress(PlayerStats.instance, Rendering.instance, QuestHolder.instance);
                    obj.GetComponent<BossSpawner>().SpawnBoss();
                }

            }
        }


        //checking if player is finished dashing and should show body
        if(!dashing)
        {
            SkinnedMeshRenderer bod = GameObject.Find("CowSkin").GetComponent<SkinnedMeshRenderer>();
            if(bod.enabled == false)
            {
                bod.enabled = true;
            }
        }
        else if(!paused && dashing)
        {
            dashing = false;
            Instantiate(DashEffect, gameObject.transform);
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Interactable" || other.gameObject.tag.Contains("item"))
        {
            prompt.SetActive(true);
            obj = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        prompt.SetActive(false);
    }



    public void CheckInput()
    {
        //Standing
        if (Input.GetKeyDown(KeyCode.X) && CowMovementDecider.instance.paused == false)
        {
            if (stand.active == true)
            {
                if (!standing)
                {
                    if(PlayerStats.instance.activeAbil != "pistol")
                    {
                        CowMovementDecider.instance.Stand();
                    }
                    //else if (PlayerStats.instance.activeAbil == "pistol")
                    //{
                    //    CowMovementDecider.instance.UnStand();
                    //    Notify.instance.Message("Can't stand with the current weapon", 3);
                    //}
                }
                else
                {
                    CowMovementDecider.instance.UnStand();
                }
            }
            else
            {
                Notify.instance.Message("Learn to stand from barn", 5);
            }

        }
        //dashing
        if (Input.GetMouseButtonDown(2) && !dashing && dashDelay == false)
        {
            if (dash.active == true)
            {
                if(dashDelay != true)
                {
                    ThirdPersonMovement tm = ThirdPersonMovement.instance;
                    if (ThirdPersonMovement.instance.moving)
                    {
                        Instantiate(DashEffect, gameObject.transform.position, Quaternion.identity);
                        StartCoroutine(Delay(0.25f));
                        tm.Dash();
                    }
                }
                else
                {
                    Notify.instance.Message("Dash is on cooldown", 3);
                }
                
            }
        }

        //slowtime
        if (Input.GetKeyDown(KeyCode.Q) && slow.active == true)
        {
            if(slowReady == true)
            {
                Time.timeScale = 0.5f;
                StartCoroutine(SlowDelay());
            }
        }
    }


    IEnumerator Delay(float time)
    {
        paused = true;
        yield return new WaitForSeconds(time);
        paused = false;

        if (dashing == true)
        {
            dashDelay = true;
            yield return new WaitForSeconds(dashCooldown);
            dashDelay = false;
        }

    }

    IEnumerator SlowDelay()
    {   
        slowReady = false;
        yield return new WaitForSeconds(slowDuration);
        Time.timeScale = 1f;
        yield return new WaitForSeconds(slowCooldown);
        slowReady = true;
        
    }
}
