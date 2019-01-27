using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteMyself : MonoBehaviour
{
    void Start()
    {
        Invoke("Die", .2f);
    }

    void Die()
    {
        Destroy(this.gameObject);
    }
}
