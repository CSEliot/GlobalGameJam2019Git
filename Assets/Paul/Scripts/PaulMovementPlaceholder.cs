using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PlayerState
{
    Normal, Holding
}

public class PaulMovementPlaceholder : MonoBehaviour
{
    public PlayerState playerState;

    public PlayerHome myHome;

    public Transform camRef;
    public Rigidbody rby;
    public float speed = 1f;

    public DetectCollects detector;

    public Transform holdPos;
    public Transform heldObj;

    public Collectable cItem;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        rby.velocity = ((this.transform.forward * speed) * Input.GetAxis("Vertical")) + 
            ((this.transform.right * speed) * Input.GetAxis("Horizontal"));

        this.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));


        

        if (Input.GetMouseButton(0))
        {
            Click();
        }

    }

    void Click()
    {
        switch (playerState)
        {
            case PlayerState.Normal:
                {
                    if (detector.hasNearObj)
                    {
                        heldObj = detector.closeObj;
                        heldObj.parent = this.transform;
                        heldObj.position = holdPos.position;
                        cItem = heldObj.GetComponent<Collectable>();
                        playerState = PlayerState.Holding;
                    }
                    break;
                }
            case PlayerState.Holding:
                {
                    if (detector.nearHouse)
                    {
                        myHome.DropItemInRoom(0, cItem);
                        playerState = PlayerState.Normal;
                    }
                    break;
                }
        }
    }
}
