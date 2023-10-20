using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplodingEgg : MonoBehaviour
{
    public GameObject boomEffect;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(tick(2));
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator tick(float time)
    {
        yield return new WaitForSeconds(time);
        BlowUp();
    }

    public void BlowUp()
    {
        Instantiate(boomEffect, transform.position, Quaternion.identity);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
        if (distance <= 20)
        {
            float impact = 250;
            Vector3 dir = gameObject.transform.rotation * Vector3.forward;
            player.GetComponent<ImpactReceiver>().AddImpact(dir, impact);
            PlayerStats.instance.health -= 15;
        }
        Destroy(gameObject);
    }
}
