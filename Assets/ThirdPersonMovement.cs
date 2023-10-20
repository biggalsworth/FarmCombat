using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{

    public CharacterController controller;
    public static ThirdPersonMovement instance;
    public Transform cam; 

    public float speed = 6;
    public float sprintIncrease = 5;
    bool sprinting = false;
    float FinalSpeed;

    public bool moving;

    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    
    

    [Header("Gravity and Jumping")]
    [SerializeField] private Vector3 velocity;
    public float gravity = -9.81f; // if gravtiy is too slow, try -19.62
    public float jumpHeight = 3;

    [Header("Object grounded variable")]
    public Transform groundCheck;
    public float groundDistance = 0.6f;
    public LayerMask groundMask;
    bool isGrounded;

    private bool hit;


    private void Start()
    {
        instance = this.GetComponent<ThirdPersonMovement>();
    }

    private void Update()
    {
        if(PauseScreen.paused == false)
        {
            if (!hit)
            {
                //activate sprint
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    if (PlayerAbilities.instance.sprint.active == false)
                    {
                        Notify.instance.Message("Aquire sprinting from barn first", 5);
                    }
                    else
                    {
                        sprinting = true;
                    }
                }



                //checks if gravity is required
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
                if (isGrounded && velocity.y < 0 || PlayerAbilities.instance.dashing == true)
                {
                    velocity.y = -2f; //-2 makes it seem a bit more stuck to the ground when the player lands.
                }

                float horizonatal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");

                Vector3 direction = new Vector3(horizonatal, 0f, vertical).normalized;


                //sets speed depending on sprinting
                if (sprinting)
                {
                    FinalSpeed = speed + sprintIncrease;
                }
                else
                {
                    FinalSpeed = speed;
                }

                if (direction.magnitude >= 0.1)
                {
                    moving = true;
                    //Calcutes where to look relative to the camera
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    //float angle = Mathf.SmoothDampAngle(transform.localEulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    float angle = Mathf.SmoothDampAngle(transform.localEulerAngles.y, cam.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    //transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);

                    //Moves player

                    Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    if (PlayerAbilities.instance.dashing)
                    {
                        FinalSpeed += FinalSpeed * 5;
                    }
                    else if (playerShoot.instance.aiming == true)
                    {
                        FinalSpeed -= 2;
                    }
                    controller.Move(moveDirection.normalized * FinalSpeed * Time.deltaTime);


                }
                else
                {
                    sprinting = false;
                    moving = false;
                }

                //jumping if grounded
                if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                {
                    if (PlayerAbilities.instance.hjump.active == true)
                    {
                        velocity.y = Mathf.Sqrt((jumpHeight + 4) * -2f * gravity);
                    }
                    else
                    {
                        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    }
                }

                velocity.y += gravity * Time.deltaTime;
                controller.Move(velocity * Time.deltaTime);
            }
        }


    }

    public void Dash()
    {
        PlayerAbilities.instance.dashing = true;
        SkinnedMeshRenderer bod = GameObject.Find("CowSkin").GetComponent<SkinnedMeshRenderer>();
        bod.enabled = false;
        FinalSpeed = FinalSpeed + FinalSpeed;
    }




    /*
    //Getting hit by enemy cow
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            hit = true;
            //gameObject.GetComponent<BoxCollider>().isTrigger = false;
            StartCoroutine(Hit());

            GameObject enemy = other.gameObject;
            Transform enemyTr = enemy.transform;
            if (other.gameObject.GetComponent<EnemyMovment>().attacking == true)
            {
                if(PlayerStats.instance.health > 0)
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
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                rb.freezeRotation = true;
                //rb.useGravity = true;

                direction *= 150;
                direction.y = 300;
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
        if(other.gameObject.tag == "Enemy")
        {
            gameObject.GetComponent<BoxCollider>().isTrigger = false;

        }
        

    }

    
    //if the player is getting hit back for too long decrease this time
    IEnumerator Hit()
    {
        hit = true;
        yield return new WaitForSeconds(0.75f);
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        Destroy(rb);

        controller.enabled = true;
        hit = false;
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            gameObject.transform.position = transform.position;
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            Destroy(rb);
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            controller.enabled = true;
            hit = false;
        }
    }
    */




}
