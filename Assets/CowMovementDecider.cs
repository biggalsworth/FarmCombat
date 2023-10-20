using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowMovementDecider : MonoBehaviour
{
    public Animator anim;
    private PlayerAbilities abil;
    public static CowMovementDecider instance;

    public bool paused;

    // Start is called before the first frame update
    void Start()
    {
        instance = this.GetComponent<CowMovementDecider>();    
        abil = PlayerAbilities.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (abil.standing)
            {
                if (ThirdPersonMovement.instance.moving == true)
                {
                    anim.Play("StandWalking");
                }
                else
                {
                    anim.Play("StandingIdle");
                }
            }
            else
            {
                if (ThirdPersonMovement.instance.moving == true)
                {
                    anim.Play("walking");
                }
                else
                {
                    anim.Play("idle");
                }
            }
        }
    }

    public void Stand()
    {
        
        anim.Play("stand");
        StartCoroutine(Delay(1, true));
        //abil.standing = true;
    }


    public void UnStand()
    {

        anim.Play("bend");
        StartCoroutine(Delay(1, false));
        //abil.standing = false;

    }


    IEnumerator Delay(float time, bool isStanding)
    {
        paused = true;
        yield return new WaitForSeconds(time);
        abil.standing = isStanding;
        paused = false;
    }
}
