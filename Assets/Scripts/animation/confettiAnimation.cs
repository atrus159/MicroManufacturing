using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class confettiAnimation : MonoBehaviour
{

    Vector3 rotVelocities;
    Vector3 rotPos;
    Vector3 Velocity;
    Vector3 Gravity;
    // Start is called before the first frame update
    void Start()
    {
        rotVelocities = new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360));
        rotPos = new Vector3(0, 0, 0);
        GetComponent<Image>().color = new Color(0, Random.Range(0.1f, 0.75f), 1);
        if(Random.Range(0,100) > 70)
        {
            GetComponent<Image>().color = new Color(1, 0.97f, 0.51f);
        }
        Velocity = new Vector3(Random.Range(-400, 400), Random.Range(100, 800),0);
        Gravity = new Vector3(0, -2,0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = rotPos + rotVelocities * Time.deltaTime;
        if (newPos.x >= 360)
        {
            newPos.x-=360;
        }
        if (newPos.x < 0)
        {
            newPos.x+=360;
        }
        if (newPos.y >= 360)
        {
            newPos.y -= 360;
        }
        if (newPos.y < 0)
        {
            newPos.y += 360;
        }
        if (newPos.z >= 360)
        {
            newPos.z -= 360;
        }
        if (newPos.z < 0)
        {
            newPos.z += 360;
        }

        Velocity += Gravity;

        rotPos = newPos;

        transform.SetPositionAndRotation(transform.position + Velocity * Time.deltaTime, Quaternion.Euler(rotPos));
        if(transform.position.y <= -500)
        {
            Destroy(gameObject);
        }
    }
}
