using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class playerShoot : MonoBehaviour
{
    private PlayerStats stats;
    public static playerShoot instance;

    public CinemachineFreeLook cam;
    public GameManager camera;

    [Header("Milk Shooting")]
    public GameObject udder;
    public GameObject milk;
    public GameObject milkShootEffect;

    [Space]
    [Header("Pistol")]
    public GameObject pistolBarrel;
    public GameObject pistolBullet;
    public GameObject shootEffect;

    [Space]

    private Notify message;
    private bool weaponised;

    public bool aiming;
    public bool shoot = true;


    // Start is called before the first frame update
    void Start()
    {
        stats = this.GetComponent<PlayerStats>();
        instance = this.GetComponent<playerShoot>();
        message = Notify.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(stats.activeAbil != "")
        {
            weaponised = true;
        }
        else
        {
            weaponised = false;
        }
        if (Input.GetMouseButtonDown(0) && PlayerStats.instance.activeAbil != "")
        {
            Shoot();
        }




        if (Input.GetMouseButtonDown(1) && weaponised == true)
        {
            cam.m_Lens.FieldOfView = 20;
            aiming = true;
        }
        if (Input.GetMouseButtonUp(1) && PlayerAbilities.instance.dashing == false)
        {
            cam.m_Lens.FieldOfView = 45;
            aiming = false;
        }
    }


    public void Shoot()
    {
        //transform.rotation = Quaternion.LookRotation(camera.transform.rotation.eulerAngles);
        if (shoot)
        {
            if (stats.activeAbil == "milk")
            {
                if (PlayerAbilities.instance.standing == true)
                {
                    GameObject bull = Instantiate(milk, udder.transform.position, udder.transform.rotation);
                    Instantiate(milkShootEffect, udder.transform.position, udder.transform.rotation);
                    StartCoroutine(shotDelay(0.5f));
                    Destroy(bull, 5);
                }
                else
                {
                    message.Message("You need to stand to use milk", 5);
                }
            }
            else if (stats.activeAbil == "pistol")
            {
                if (PlayerAbilities.instance.standing == false)
                {
                    GameObject bull = Instantiate(pistolBullet, pistolBarrel.transform.position, pistolBarrel.transform.rotation);
                    Instantiate(shootEffect, pistolBarrel.transform.position, pistolBarrel.transform.rotation);
                    Destroy(bull, 5);
                    StartCoroutine(shotDelay(0.5f));
                }
                else
                {
                    message.Message("You need to stop standing to use this weapon", 5);
                }
            }
        }
        
    }

    public IEnumerator shotDelay(float time)
    {
        shoot = false;
        yield return new WaitForSeconds(time);
        shoot = true;
    }
}
