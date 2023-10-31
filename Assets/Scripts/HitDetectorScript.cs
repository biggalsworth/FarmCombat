using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetectorScript : MonoBehaviour
{
    public bool Stander;
    public Collider coll;
    public GameObject player;

    public static bool hit;

    public CharacterController controller;

    public ThirdPersonMovement move;
    
    // Start is called before the first frame update
    void Start()
    {
        coll = gameObject.GetComponent<BoxCollider>();
        controller = GameObject.Find("Player").GetComponent<CharacterController>();
        move = GameObject.Find("Player").GetComponent<ThirdPersonMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        transform.rotation = player.transform.rotation;
        if (PlayerAbilities.instance.standing)
        {
            if (Stander)
            {

                coll.enabled = true;
            }
        }
        else
        {
            if (Stander)
            {
                coll.enabled = false;
            }
        }
    }


    //Getting hit by enemy cow
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("AHHHH");

        if (other.gameObject.tag == "Enemy")
        {
            //hit = true;
            move.enabled = false;

            //gameObject.GetComponent<BoxCollider>().isTrigger = false;
            //StartCoroutine(Hit());

            GameObject enemy = other.gameObject;
            Transform enemyTr = enemy.transform;
            if (other.gameObject.GetComponent<EnemyMovment>().attacking == true)
            {
                if (PlayerStats.instance.health > 0)
                {
                    PlayerStats.instance.health -= other.gameObject.GetComponent<EnemyStats>().damage;
                }
                else
                {
                    PlayerStats.instance.health = 0;
                }

                CharacterController ctrl = other.gameObject.GetComponent(typeof(CharacterController)) as CharacterController;
                //if (ctrl)
                //{
                Vector3 direction = (transform.position - enemyTr.position);
                Vector3 position = enemyTr.position;
                //velocity.y += 10;
                //Move(direction, 250f);
                //controller.SimpleMove(new Vector3(100f, enemy.transform.rotation.y * enemy.GetComponent<EnemyMovment>().force));
                //controller.Move(direction * 500 * Time.deltaTime);

                controller.enabled = false;
                player.AddComponent<Rigidbody>();
                coll.isTrigger = false;
                Rigidbody rb = player.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                //rb.useGravity = true;

                //direction *= 100;
                direction.y = 150;
                //controller.SimpleMove(direction * Time.deltaTime);
                rb.AddForce(direction * 100 * Time.deltaTime);

                //rb.AddExplosionForce(10000f, enemyTr.position, 1f, 100000f, ForceMode.Acceleration);



                //}
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && hit == true)
        {

            controller.enabled = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            coll.isTrigger = false;

        }


    }


    //if the player is getting hit back for too long decrease this time
    IEnumerator Hit()
    {
        hit = true;
        yield return new WaitForSeconds(1f);
        coll.isTrigger = true;
        move.enabled = true;
        Rigidbody rb = player.GetComponent<Rigidbody>();
        Destroy(rb);

        //controller.enabled = true;
        hit = false;
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("AHHHH");
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //gameObject.transform.position = transform.position;
            //Rigidbody rb = player.GetComponent<Rigidbody>();
            Destroy(player.GetComponent<Rigidbody>());
            coll.isTrigger = true;
            controller.enabled = true;
            player.transform.position = gameObject.transform.position;
            move.enabled = true;

            hit = false;
        }
    }
    
}
