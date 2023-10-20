using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    private PlayerAbilities abils;
    private PlayerStats stats;

    public Animator anim;
    

    // Start is called before the first frame update
    void Start()
    {
        stats = PlayerStats.instance;
        abils = PlayerAbilities.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) && abils.pistol.active == true)// && abils.standing == false)
        {
            if (stats.activeAbil != "pistol")
            {
                //anim.Play("UnequipPistol");
                StartCoroutine(waitForBend());

            }
            else if(stats.activeAbil == "pistol")
            {

                //StartCoroutine(waitForBend());
                anim.Play("UnequipPistol");
                stats.activeAbil = "";

            }
        }
        else if (abils.standing == true && stats.activeAbil == "pistol")
        {
            GameObject.Find("M1911").SetActive(false);
        }
    }



    IEnumerator waitForBend()
    {
        //Debug.Log("Waiting for princess to be rescued...");
        yield return new WaitUntil(() => (abils.standing == false));
        //Debug.Log("Princess was rescued!");
        anim.Play("EquipPistol");
        stats.activeAbil = "pistol";
    }
}
