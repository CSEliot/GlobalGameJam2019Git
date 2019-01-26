using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollects : MonoBehaviour
{
    public bool nearHouse;
    public bool hasNearObj;
    public Transform closeObj;

    void OnTriggerEnter(Collider col)
    {
        switch (col.tag)
        {
            case "Collectable":
                {
                    closeObj = col.gameObject.transform;
                    hasNearObj = true;
                    break;
                }
            case "Home":
                {
                    nearHouse = true;
                    break;
                }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        switch (col.tag)
        {
            case "Collectable":
                {
                    closeObj = null;
                    hasNearObj = false;
                    break;
                }
            case "Home":
                {
                    nearHouse = false;
                    break;
                }
        }
        
    }
}
