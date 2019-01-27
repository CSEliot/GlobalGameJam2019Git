using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTrucks : MonoBehaviour
{

    public Transform inner;
    public Transform outer;

    public float speed;

    void Update()
    {
        inner.Rotate(new Vector3(0, speed,0));

        outer.Rotate(new Vector3(0, -speed * .8f, 0));
    }
}
