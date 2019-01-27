using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImitatePlayerOrAi : MonoBehaviour
{
    public PersonalityType aiPersonality;
    public Transform currentTarget;
    public Rigidbody rby;

    private Vector3 pVelocity;
    public float speed;
    
    void Update()
    {
        float yVelocity = rby.velocity.y;
        this.transform.LookAt(new Vector3(currentTarget.position.x, this.transform.position.y, currentTarget.position.z), Vector3.up);

        pVelocity = this.transform.forward * speed;
        pVelocity.y = yVelocity;

        rby.velocity = pVelocity;
    }
}
