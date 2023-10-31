using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovment : MonoBehaviour
{

    public Animator anim;
    [Space]

    private float distance;

    public Rigidbody rb;
    public int damping = 10;
    public GameObject head;
    public GameObject PlayersHead;
    public int maxColliders = 100;

    public bool attacking;


    [Header("Capabilities")]
    public float detectRange = 60;
    public float advanceRange = 40;
    public float attackRange = 10;
    public float attackSpeedMult = 4;

    public int force = 100;
    //public Transform centre;
    public int speed = 10;

    public Vector3 forward;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        PlayersHead = GameObject.Find("LookAt");
        anim.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        if(PauseScreen.paused != true)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            distance = Vector3.Distance(player.transform.position, head.transform.position);

            this.GetComponent<CharacterController>().SimpleMove(Vector3.down);

            if ((distance <= detectRange && distance > attackRange) && attacking == false)
            {
                Vector3 lookPos = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
                if (head != null)
                {
                    head.transform.LookAt(PlayersHead.transform.position);
                }
                if (distance <= advanceRange)
                {
                    forward = transform.TransformDirection(Vector3.forward);
                    this.GetComponent<CharacterController>().SimpleMove(forward * speed);
                    anim.Play("walking");
                }
                else
                {
                    this.GetComponent<CharacterController>().SimpleMove(Vector3.zero); //Otherwise cow will be stuck in air if it isnt touching ground
                }
            }
            else if ((distance <= attackRange) || attacking == true)
            {
                StartCoroutine(Delay());

                forward = transform.TransformDirection(Vector3.forward);

                this.GetComponent<CharacterController>().SimpleMove(forward * (speed * attackSpeedMult));
                anim.Play("attack");

            }
            else
            {
                anim.Play("Idle");
            }
        }
    }

    IEnumerator Delay()
    {
        attacking = true;
        yield return new WaitForSeconds(0.75f);
        attacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            ImpactReceiver script = other.transform.GetComponent<ImpactReceiver>();
            if (script)
            {
                //Vector3 dir = other.transform.position - gameObject.transform.position;
                Vector3 dir = gameObject.transform.rotation * Vector3.forward;
                float impact = force;//Mathf.Clamp(force / 3, 0, 201);
                script.AddImpact(dir, impact);
            }

            EnemyStats obj = gameObject.GetComponent<EnemyStats>();
            PlayerStats.instance.health -= obj.damage;
        }
    }
}
