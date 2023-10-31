using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public Rigidbody rb;

    public static bullet instance;

    public GameObject cam;

    public int damage = 2;

    [Space]

    public int speed;
    public LayerMask groundLayer;

    public int dropTime = 1;

    public GameObject breakEffect;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Drop(dropTime));
        instance = this.GetComponent<bullet>();
        
    }
    
    

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed);
    }





    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == 8)
        {
            Destroy(this.gameObject);
            Instantiate(breakEffect, transform.position, Quaternion.identity);
        }
        else
        {
            if(collision.gameObject.tag == "Enemy")
            {
                Instantiate(breakEffect, transform.position, Quaternion.identity);
            }
        }
    }
    

    IEnumerator Drop(int time)
    {
        yield return new WaitForSeconds(time);
        rb.useGravity = true;
    }
}
