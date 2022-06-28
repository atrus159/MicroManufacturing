using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    Rigidbody rb;
    CapsuleCollider cl;
    public float speed;

    float xInput;
    float zInput;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cl = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, -Vector3.up, cl.height-1))
        {
            rb.AddForce(Vector3.up * 300);
        }
        //xInput = Input.GetAxisRaw("Horizontal");
        //zInput = Input.GetAxisRaw("Vertical"); 
        rb.velocity = new Vector3(xInput * speed, rb.velocity.y, zInput * speed);

    }

    private void OnMouseDown()
    {
        
    }
}
