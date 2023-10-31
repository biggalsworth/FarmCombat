using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chickenLord : MonoBehaviour
{
    public Animator anim;
    [Space]

    private float distance;

    //public Rigidbody rb;
    public int damping = 10;
    public GameObject head;
    public GameObject PlayersHead;
    public int maxColliders = 100;

    public bool attacking;
    public bool allowAttacking = true;
    public bool egging = false;

    float mass = 1.0f;

    [Space]


    [Header("Capabilities")]
    public float detectRange = 100;
    public float advanceRange = 60;
    public float attackRange = 40;
    public float attackSpeedMult = 4;

    public int force = 100;
    public int JumpForce = 20;

    //public Transform centre;
    public int speed = 10;

    public Vector3 forward;

    [Space]
    [Header("Attack Stats")]
    public float slamRange = 40f;
    public float slamForce = 100f;
    public int slamDamage = 15;
    public int rollDamage = 5;

    


    CharacterController character;
    [Space]

    [Header("Object grounded variable")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;



    [Space]
    [Header("Particals/objects")]
    public Transform EffectPos;
    public GameObject dust;
    public GameObject Egg;
    public Transform EggLayer;

    private bool rolling;

    // Start is called before the first frame update
    void Start()
    {
        PlayersHead = GameObject.Find("LookAt");
        anim.Play("Idle");
        character = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        if(gameObject.GetComponent<EnemyStats>().health <= 0)
        {
            PlayerStats.instance.bosses.Add("ChickenLord");
            QuestHolder.instance.newQuest("The Almighty Shepherd", "I have killed the chicken lord! " +
                "I learnt some valuable knowledge from our fight, might even be able to aquire new weapons." +
                "The next strongest in this land is the shepherd, let's get him next.");
            SaveSystem.SaveProgress(PlayerStats.instance, Rendering.instance, QuestHolder.instance);
        }
        if (allowAttacking || egging || rolling)
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
                    if (!egging || !rolling)
                    {
                        head.transform.LookAt(PlayersHead.transform.position);
                    }

                    //Vector3 LookPos = PlayersHead.transform.position;
                    //transform.rotation = Quaternion.EulerRotation(0f, LookPos.y, lookzPos.z);
                }
                if (distance <= advanceRange)
                {
                    if (!egging)
                    {
                        forward = transform.TransformDirection(Vector3.forward);
                    }
                    else
                    {
                        forward = transform.TransformDirection(Vector3.back);
                    }
                    this.GetComponent<CharacterController>().SimpleMove(forward * speed);
                    if (!rolling)
                    {
                        anim.Play("Walk");

                    }
                    else if (rolling)
                    {
                        anim.Play("Rolling");
                    }
                }
                else
                {
                    this.GetComponent<CharacterController>().SimpleMove(Vector3.zero); //Otherwise cow will be stuck in air if it isnt touching ground
                }
            }
            else if ((distance <= attackRange) || attacking == true)
            {
                if (allowAttacking)
                {
                    Attack();
                }
                else if (egging)
                {
                    forward = transform.TransformDirection(Vector3.forward);
                    this.GetComponent<CharacterController>().SimpleMove(forward * speed);
                }
                else if (rolling)
                {
                    anim.Play("Rolling");
                    forward = transform.TransformDirection(Vector3.forward);
                    this.GetComponent<CharacterController>().SimpleMove(forward * (speed + 6));
                }
                else
                {
                    anim.Play("Idle");
                }
            }
            else
            {

                anim.Play("Idle");
            }
        }
    }

    public void Attack()
    {
        //Debug.Log("Attack");

        int num = Random.Range(0,6);
        if(num == 0)
        {
            Debug.Log("slam");
            Slam();
        }
        if(num == 1 || num == 3 || num == 4)
        {
            Debug.Log("drop");
            DropEgg();
        }
        if(num == 2 || num == 5)
        {
            Debug.Log("Roll");
            Roll();
        }
    }

    public void Slam()
    {
        anim.Play("Slam");
        Instantiate(dust, EffectPos);
        StartCoroutine(DelayAttacking(1.95f));
        StartCoroutine(SlamAttack(1.95f, 10));
    }

    public void DropEgg()
    {
        transform.Rotate(new Vector3(transform.rotation.x, 180, transform.rotation.z));
        Instantiate(Egg, EggLayer.position, Quaternion.identity);
        
        StartCoroutine(EggTime(2f));
        
    }

    public void Roll()
    {
        if (!rolling)
        {
            anim.Play("roll");
            StartCoroutine(rollyTime(5f));
            
        }
    }


    public IEnumerator DelayAttacking(float time)
    {
        allowAttacking = false;
        yield return new WaitForSeconds(time);
        allowAttacking = true;
        
    }

    public IEnumerator rollyTime(float time)
    {
        allowAttacking = false;

        rolling = true;
        yield return new WaitForSeconds(time);
        anim.Play("unRoll");
        rolling = false;
        allowAttacking = true;

    }


    public IEnumerator SlamAttack(float time, int force)
    {
        allowAttacking = false;
        yield return new WaitForSeconds(time);
        allowAttacking = true;
        Instantiate(dust, EffectPos);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        distance = Vector3.Distance(player.transform.position, head.transform.position);
        if (distance < slamRange)
        {
            float impact = slamForce;
            Vector3 dir = gameObject.transform.rotation * Vector3.forward;
            player.GetComponent<ImpactReceiver>().AddImpact(dir, impact);
            PlayerStats.instance.health -= slamDamage;
        }
    }

    public IEnumerator EggTime(float time)
    {
        allowAttacking = false;
        egging = true;
        yield return new WaitForSeconds(time);
        egging = false;
        allowAttacking = true;
    }


    public void Impacter()
    {
        Vector3 impact = Vector3.zero;
        Vector3 dir = gameObject.transform.rotation * Vector3.back;
        //moveScript.enabled = false;
        dir.Normalize();

        dir *= 200;
        dir.y = 100;
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * 250 / mass;

        if (impact.magnitude > 0.2F) character.Move(impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }


    public void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player" && rolling == true)
        {
            //Vector3 dir = other.transform.position - gameObject.transform.position;
            Vector3 dir = gameObject.transform.rotation * Vector3.forward;
            float impact = 500;//Mathf.Clamp(force / 3, 0, 201);
            collision.gameObject.GetComponent<ImpactReceiver>().AddImpact(dir, impact);
            PlayerStats.instance.health -= rollDamage;
            Debug.Log("AHHHHH");
        }
        Debug.Log("missed");

    }
    
}
