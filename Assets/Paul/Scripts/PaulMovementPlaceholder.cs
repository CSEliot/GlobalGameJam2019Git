using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[System.Serializable]
public enum PlayerState
{
    Normal, Holding, Stunned, Boosted
}

[System.Serializable]
public enum Location
{
    Outside, Store, LivingRoom, Kitchen, Bedroom, Bathroom
}

public class PaulMovementPlaceholder : MonoBehaviour
{
    public PlayerState playerState;
    public Location location;

    public NeighbourhoodManager neighbourhoodMan;

    public Animator charAnim;

    public int myPlayerID;//0-8
    public int myHome;//0-8
    public int currentHome;//0-8

    public Transform camRef;
    public Rigidbody rby;
    public float speed = 1f;
    private float animSpeed;
    private float targetSpeed = 0f;

    public DetectCollects detector;

    public Transform holdPos;
    public Transform tempAtk;
    public Transform tempNorm;

    public Collectable cItem;
    public bool canAttack;

    public TextMeshPro locationTxt;

    public List<GameObject> hats = new List<GameObject>();
    private Vector3 rVelocity;

    private bool lClickDown;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SelectHat(Random.Range(0, 8));
        //PhotonArenaManager.Instance.Connect();

       
    }

    void SelectHat(int num)
    {
        for(int i = 0; i < hats.Count; i++)
        {
            if(i != num)
            {
                hats[i].SetActive(false);
            }
            else
            {
                hats[i].SetActive(true);
            }
        }
    }

    void Update()
    {
        float GravY = rby.velocity.y;

        rVelocity = ((this.transform.forward * speed) * Input.GetAxis("Vertical")) + 
            ((this.transform.right * speed) * Input.GetAxis("Horizontal"));

        if(Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            targetSpeed = 0f;
        }
        else
        {
            targetSpeed = 1f;
        }

        animSpeed = Mathf.Lerp(animSpeed, targetSpeed, Time.deltaTime * 5f);
        charAnim.SetFloat("Speed", animSpeed);

        rVelocity.y = GravY;
        rby.velocity = rVelocity;

        this.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));


        

        if (Input.GetMouseButtonDown(0))
        {
            if (!lClickDown)
            {
                lClickDown = true;
                Click();
            }
        }
        else
        {
            lClickDown = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            RightClick();
        }

        if (Input.GetButton("Jump"))
        {
            Debug.Log("simulate get hit");
        }
    }

    void GetHit()
    {
        //
    }

    void Click()
    {
        switch (playerState)
        {
            case PlayerState.Normal:
                {
                    Pickup();
                    break;
                }
            case PlayerState.Holding://click while holding
                {
                    if (canAttack)
                    {
                        Attack();
                        canAttack = false;
                    }
                    break;
                }
            case PlayerState.Stunned:
                {
                    break;
                }
            case PlayerState.Boosted:
                {
                    break;
                }
        }
    }

    void RightClick()
    {
        switch (playerState)
        {
            case PlayerState.Normal:
                {
                    //yell
                    break;
                }
            case PlayerState.Holding://click while holding
                {
                    DropOrPlace();
                    break;
                }
            case PlayerState.Stunned:
                {
                    break;
                }
            case PlayerState.Boosted:
                {
                    break;
                }
        }
    }

    void Pickup()
    {
        if (detector.hasNearObj)
        {
            cItem = detector.closeObj;
            cItem.transform.parent = holdPos;
            cItem.transform.localPosition = cItem.localPos;
            cItem.transform.localEulerAngles = cItem.localErot;
            cItem.collido.enabled = false;
            cItem.rby.isKinematic = true;
            canAttack = true;
            charAnim.SetBool("Packing", true);

            playerState = PlayerState.Holding;
        }
    }

    void Attack()
    {
        //holdPos.position = tempAtk.position;
        //holdPos.rotation = tempAtk.rotation;
        charAnim.SetTrigger("Attack");
        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        float t = 0;

        while(t < 1f)
        {
            t += Time.deltaTime * 2.5f;

            //holdPos.position = Vector3.Lerp(holdPos.position, tempNorm.position, t);
            //holdPos.rotation = Quaternion.Slerp(holdPos.rotation, tempNorm.rotation, t);
            yield return null;
        }

        canAttack = true;
    }

    void DropOrPlace()
    {
        int roomLocation = 0;

        switch (location)
        {
            case Location.Store:
                {
                    neighbourhoodMan.DropItemOutside(myPlayerID, cItem);
                    break;
                }
            case Location.Outside:
                {
                    neighbourhoodMan.DropItemOutside(myPlayerID, cItem);
                    break;
                }
            case Location.LivingRoom:
                {
                    roomLocation = 0;
                    neighbourhoodMan.DropItemInHouseRoom(myPlayerID, currentHome, roomLocation, cItem);
                    break;
                }
            case Location.Kitchen:
                {
                    roomLocation = 1;
                    neighbourhoodMan.DropItemInHouseRoom(myPlayerID, currentHome, roomLocation, cItem);
                    break;
                }
            case Location.Bedroom:
                {
                    roomLocation = 2;
                    neighbourhoodMan.DropItemInHouseRoom(myPlayerID, currentHome, roomLocation, cItem);
                    break;
                }
            case Location.Bathroom:
                {
                    roomLocation = 3;
                    neighbourhoodMan.DropItemInHouseRoom(myPlayerID, currentHome, roomLocation, cItem);
                    break;
                }
        }

        StopAllCoroutines();
        charAnim.SetBool("Packing", false);
        detector.hasNearObj = false;
        canAttack = false;
        cItem = null;
        playerState = PlayerState.Normal;
    }

}
