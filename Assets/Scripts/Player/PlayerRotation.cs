using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] 
    private Transform aimCamera;
    private Rigidbody rb;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        velocity.Normalize();
        aimCamera.forward = Vector3.Slerp(aimCamera.forward, velocity, Time.deltaTime * 0.3f);
    }
    
    
}
