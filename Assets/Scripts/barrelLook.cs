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
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + player.transform.eulerAngles.y;
                //float angle = Mathf.SmoothDampAngle(transform.localEulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                float angle = Mathf.SmoothDampAngle(player.transform.localEulerAngles.y, targetAngle, ref player.GetComponent<ThirdPersonMovement>().turnSmoothVelocity, 0.1f);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
        }
        
    }
}
