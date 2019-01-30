using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

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

public class PaulMovementPlaceholder : MonoBehaviourPun, IPunObservable {
    
    public PersonalityType myPersonality;
    public PlayerState playerState;
    public Location location;

    public HUDManager hudMan;
    public NeighbourhoodManager neighbourhoodMan;

    public Animator charAnim;

    public int myPlayerID;//0-8
    public int myHome;//0-8
    public int currentHome;//0-8

    private bool _playerInitialized = false;

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

    public Transform meshRotater;

    public bool waitForStart;

    public Transform downCaster;


    /// <summary>
    /// flag to force latest data to avoid initial drifts when player is instantiated.
    /// </summary>
    private bool m_firstTake = true; //PHoton-y thing

    public bool blockNet;
    private bool onGround;
    private bool previouslyInAir;

    public Transform hitboxAttack;

    public int myScore;

    public TextMeshPro floatingUsernameTxt;

    public GameObject positiveObj;//Particle effects
    public GameObject negativeObj;

    //new movement
    public float isGroundedRayLength = 0.1f;
    public LayerMask layerMaskForGrounded;

    public float VerticalVector;
    public float GravityVector;
    public float JumpVector;
    public Vector3 transVector;
    public Vector3 sideVector;
    public Vector3 MovementVector;


    void Awake() {

        if (!blockNet)
        {
            StartCoroutine("StartNet");
        }
    }

    void Start() {
        
        SelectHat((int)myPersonality);
        hitboxAttack.parent = meshRotater;

        if (photonView.IsMine) {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else {
            camRef.gameObject.SetActive(false);
        }

        if (blockNet)
        {
            Cursor.lockState = CursorLockMode.Locked;
            camRef.gameObject.SetActive(true);
        }

        floatingUsernameTxt.enableCulling = true;
    }

    private void InitPlayer()
    {
        var status = PhotonArenaManager.Instance.GetCurrentDepthLevel();

        if ((status == PhotonArenaManager.ServerDepthLevel.InRoom) && (neighbourhoodMan != null))
        {
            myPlayerID = PhotonArenaManager.Instance.GetLocalPlayerID();
            floatingUsernameTxt.text = PhotonArenaManager.Instance.GetLocalUsername();
            myPersonality = GetMyPersonality();

            SelectHat((int)myPersonality);

            _playerInitialized = true;
        }
    }

    private PersonalityType GetMyPersonality()
    {
        var room = PhotonArenaManager.Instance.GetRoom();

//        var playerNums = new List<int>(8); // Replace with call to get Player Numbers
        var playerNums = room.Players.Keys;
        var playerScores = new Dictionary<int, int[]>();

        foreach (var playerNum in playerNums) {
            var score = PhotonArenaManager.Instance.GetData($"score!{playerNum}") as int[];

            if (score != null) {
                playerScores.Add(playerNum, score);
            }
        }

        var personalities = PersonalityController.SortingHat(playerScores);
        neighbourhoodMan.Personalities = personalities;

        if (personalities.ContainsKey(myPlayerID))
        {
            return personalities[myPlayerID];
        }

        return PersonalityType.Nerd;
    }


    public void SelectHat(int num) {
        for (int i = 0; i < hats.Count; i++) {
            if (i != num) {
                hats[i].SetActive(false);
            }
            else {
                hats[i].SetActive(true);
            }
        }
    }

    //new movement:
    public bool isGrounded
    {
        get
        {
            Vector3 position = transform.position;
            position.y = GetComponent<Collider>().bounds.min.y + 0.1f;
            float length = isGroundedRayLength + 0.1f;
            Debug.DrawRay(position, Vector3.down * length);
            bool grounded = Physics.Raycast(position, Vector3.down, length, layerMaskForGrounded.value);

            return grounded;
        }
    }
    
    void CalculateFalling()
    {
        if (isGrounded)
        {
            GravityVector = 0;
            if (Input.GetButton("Jump"))
            {
                Jump();
            }
            VerticalVector = rby.velocity.y - GravityVector + JumpVector;
            if (JumpVector < .1f)
            {
                JumpVector = 0;
            }
            else
            {
                JumpVector = Mathf.Lerp(JumpVector, 0f, Time.deltaTime * Mathf.Clamp(9f / JumpVector, .1f, 2f));
            }

        }
        else
        {
            VerticalVector = rby.velocity.y - GravityVector + JumpVector;
            GravityVector = Mathf.Clamp(GravityVector + (Time.deltaTime * 3), 0f, 50f);

            JumpVector = Mathf.Lerp(JumpVector, 0f, Time.deltaTime * Mathf.Clamp(9f / JumpVector, .1f, 2f));
        }
    }

    
    void CalculateCamPlayerRotation()
    {
        //CamRotation = transform.localEulerAngles;

        //CamRotation.y += Input.GetAxis("Mouse_X") * 1.5f;

        //transform.localEulerAngles = CamRotation;
    }

    void Jump()
    {
        JumpVector = .7f;
    }

    void CalculatePlayerMovement()
    {
        transVector = this.transform.TransformDirection(0, 0, 1);
        sideVector = this.transform.TransformDirection(1, 0, 0);

        if (isGrounded)
        {

            MovementVector = (transVector * Input.GetAxis("Vertical") * speed) + (sideVector * Input.GetAxis("Horizontal") * speed);
        }
        MovementVector.y = VerticalVector;

        if (MovementVector.x == 0 && MovementVector.z == 0)
        {
            meshRotater.rotation = Quaternion.LookRotation(this.transform.forward, Vector3.up);
        }
        else
        {
            meshRotater.rotation = Quaternion.LookRotation(new Vector3(MovementVector.x, 0, MovementVector.z), Vector3.up);
        }


    }

    void Update() {

        /* CalculateFalling();
        CalculateCamPlayerRotation();
        CalculatePlayerMovement();

        rb.velocity = Vector3.Lerp(rb.velocity, MovementVector, Time.deltaTime * 100);
        */


        if (!_playerInitialized)
        {
            InitPlayer();
        }

        float GravY = rby.velocity.y;

        if (photonView.IsMine || blockNet) {

            if (!waitForStart)
            {

                CalculateFalling();
                //CalculateCamPlayerRotation();

                if (playerState == PlayerState.Normal || playerState == PlayerState.Holding)
                {

                    CalculatePlayerMovement();

                    if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
                    {
                        targetSpeed = 0f;
                    }
                    else
                    {
                        targetSpeed = 1f;
                    }
                    
                   
                    this.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * 1.5f, 0));

                   
                    if (Input.GetButtonDown("Attack"))
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
                }

                animSpeed = Mathf.Lerp(animSpeed, targetSpeed, Time.deltaTime * 5f);
                charAnim.SetFloat("Speed", animSpeed);


                rby.velocity = Vector3.Lerp(rby.velocity, MovementVector, Time.deltaTime * 100);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Attack")
        {
            GetHit();
        }
    }


    void GetHit()
    {
        charAnim.SetBool("Stunned", true);
        charAnim.SetTrigger("GetHit");

        if (playerState == PlayerState.Holding)
        {
            //DROP IT
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
        }

        rby.velocity = new Vector3(0, -5, 0);
        playerState = PlayerState.Stunned;
        StartCoroutine(ResetStunned());
    }

    void AddScore()
    {
        myScore += 1;
        hudMan.scoreText.text = myScore.ToString();
    }

    IEnumerator ResetStunned()
    {
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime;

            //holdPos.position = Vector3.Lerp(holdPos.position, tempNorm.position, t);
            //holdPos.rotation = Quaternion.Slerp(holdPos.rotation, tempNorm.rotation, t);
            yield return null;
        }

        charAnim.SetBool("Stunned", false);
        playerState = PlayerState.Normal;
    }

    void Click() {
        switch (playerState) {
            case PlayerState.Normal: {
                    if(detector.hasNearObj)
                    {
                        photonView.RPC("Pickup", RpcTarget.AllBuffered, detector.closeObj.myItemRef);
                    }
                    break;
                }
            case PlayerState.Holding://click while holding
                {
                    if (canAttack) {
                        Attack();
                        canAttack = false;
                    }
                    break;
                }
            case PlayerState.Stunned: {
                    break;
                }
            case PlayerState.Boosted: {
                    break;
                }
        }
    }

    void RightClick() {
        switch (playerState) {
            case PlayerState.Normal: {
                    //yell
                    break;
                }
            case PlayerState.Holding://click while holding
                {
                    DropOrPlace();
                    break;
                }
            case PlayerState.Stunned: {
                    break;
                }
            case PlayerState.Boosted: {
                    break;
                }
        }
    }

    [PunRPC]
    void Pickup(int itemId, PhotonMessageInfo info) {
        cItem = neighbourhoodMan.allItems[itemId] = detector.closeObj;

        CBUG.Do("MY PHOTRON ID IS " + photonView.ViewID);

        charAnim.SetBool("Packing", true);

        cItem.transform.parent = holdPos;
        cItem.transform.localPosition = cItem.localPos;
        cItem.transform.localEulerAngles = cItem.localErot;
        cItem.collido.enabled = false;
        cItem.rby.isKinematic = true;

        if (photonView.IsMine) {
            canAttack = true;
            hudMan.SetInstructionText(1);
            playerState = PlayerState.Holding;

            bool pos = false;
            for(int i = 0; i < cItem.traits.Count; i++)
            {
                if(cItem.traits[i] == myPersonality)
                {
                    pos = true;
                }
            }

            if(pos == true)
            {
                positiveObj.SetActive(true);
                Invoke("TurnOffParticleFx", 3f);
            }
            else
            {
                bool neg = false;
                for (int i = 0; i < cItem.negativeTraits.Count; i++)
                {
                    if (cItem.negativeTraits[i] == myPersonality)
                    {
                        neg = true;
                    }
                }

                if (neg == true)
                {
                    negativeObj.SetActive(true);
                    Invoke("TurnOffParticleFx", 3f);
                }
            }
        }
    }
    
    void TurnOffParticleFx()
    {
        positiveObj.SetActive(false);
        negativeObj.SetActive(false);
    }

    void Attack() {
        //holdPos.position = tempAtk.position;
        //holdPos.rotation = tempAtk.rotation;
        //hitboxAttack.SetActive(true);

        PhotonArenaManager.Instance.SpawnObject("AttackBox", hitboxAttack.position, hitboxAttack.rotation);
        charAnim.SetTrigger("Attack");
        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack() {
        float t = 0;

        while (t < 1f) {
            t += Time.deltaTime * 2.5f;

            //holdPos.position = Vector3.Lerp(holdPos.position, tempNorm.position, t);
            //holdPos.rotation = Quaternion.Slerp(holdPos.rotation, tempNorm.rotation, t);
            yield return null;
        }

        //hitboxAttack.SetActive(false);
        canAttack = true;
    }

    void DropOrPlace() {
        int roomLocation = 0;

        switch (location) {
            case Location.Store: {
                    neighbourhoodMan.DropItemOutside(myPlayerID, cItem);
                    break;
                }
            case Location.Outside: {
                    neighbourhoodMan.DropItemOutside(myPlayerID, cItem);
                    break;
                }
            case Location.LivingRoom: {
                    roomLocation = 0;
                    neighbourhoodMan.DropItemInHouseRoom(myPlayerID, currentHome, roomLocation, cItem);
                    AddScore();
                    break;
                }
            case Location.Kitchen: {
                    roomLocation = 1;
                    neighbourhoodMan.DropItemInHouseRoom(myPlayerID, currentHome, roomLocation, cItem);
                    AddScore();
                    break;
                }
            case Location.Bedroom: {
                    roomLocation = 2;
                    neighbourhoodMan.DropItemInHouseRoom(myPlayerID, currentHome, roomLocation, cItem);
                    AddScore();
                    break;
                }
            case Location.Bathroom: {
                    roomLocation = 3;
                    neighbourhoodMan.DropItemInHouseRoom(myPlayerID, currentHome, roomLocation, cItem);
                    AddScore();
                    break;
                }
        }

        StopAllCoroutines();
        //hitboxAttack.SetActive(false);
        charAnim.SetBool("Packing", false);
        detector.hasNearObj = false;
        canAttack = false;
        cItem = null;
        playerState = PlayerState.Normal;
        hudMan.SetInstructionText(0);
    }
    #region IPunObservable implementation

    /// <summary>
    /// this is where data is sent and received for this Component from the PUN Network.
    /// </summary>
    /// <param name="stream">Stream.</param>
    /// <param name="info">Info.</param>
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        // currently there is no strategy to improve on bandwidth, just passing the current distance and speed is enough, 
        // Input could be passed and then used to better control speed value
        //  Data could be wrapped as a vector2 or vector3 to save a couple of bytes
        //if (stream.IsWriting) {
        //    stream.SendNext(this.curr);
        //    stream.SendNext(this.CurrentSpeed);
        //    stream.SendNext(this.m_input);
        //}
        //else {
        //    if (this.m_firstTake) {
        //        this.m_firstTake = false;
        //    }

        //    rby.velocity = (Vector3)stream.ReceiveNext();
        //    rby.rotation = (q)stream.ReceiveNext();
        //    rby.m_input = (float)stream.ReceiveNext();
        //}
    }
    #endregion IPunObservable implementation
    private IEnumerator StartNet() {
        // Wait until a Player Number is assigned
        // PlayerNumbering component must be in the scene.
        yield return new WaitUntil(() => this.photonView.Owner.ActorNumber >= 0);

        // depending on wether we control this instance locally, we force the car to become active ( because when you are alone in the room, serialization doesn't happen, but still we want to allow the user to race around)
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) {
            this.m_firstTake = false;
        }
    }
}
