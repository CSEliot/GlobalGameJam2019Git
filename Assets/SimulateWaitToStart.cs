using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulateWaitToStart : MonoBehaviour
{
    public PaulMovementPlaceholder playerGuy;
    public float waitTime;

    private float timo;
    private bool once;

    public GameObject camWait;
    public GameObject pCam;


    void Update()
    {
        if (!once)
        {

            timo += Time.deltaTime;

            if (timo >= waitTime)
            {
                playerGuy.waitForStart = false;
                once = true;
                pCam.SetActive(true);
                camWait.SetActive(false);
            }
        }
    }
}
