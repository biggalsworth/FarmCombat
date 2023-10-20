using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class barrelLook : MonoBehaviour
{
    //public CinemachineFreeLook cam;
    public GameObject cam;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(cam.transform.position, cam.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit");
            Vector3 direction = hit.point - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
            if (Input.GetMouseButtonDown(0) && (player.transform.rotation.y > cam.transform.rotation.eulerAngles.y + 10 || player.transform.rotation.y < cam.transform.rotation.eulerAngles.y - 10))
            {
                player.transform.rotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0);
            }
        }
        
    }
}
