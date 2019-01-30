using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

[System.Serializable]
public class PersonalityController : MonoBehaviour
{
    public int soccermom = 0;
    public int hermit = 0;
    public int cheap = 0;
    public int tacky = 0;
    public int athlete = 0;
    public int expensive = 0;
    public int perfectionist = 0;
    public int hoarder = 0;
    public int nerd = 0;

    private float _seconds = 60;
    private float secondsCountDown = 60;
    private float timeToCompleteTest = 0;
    private bool timerRunning = false;
    private bool hasSoldDog = false;
    private bool _testEnded = false;

    private uint _timer = 65000;
    private uint _gameStartTime;

    public Text timerText;

    [Header("Other display information")]
    public int money = 1000;
    public Text moneyText;

    int quizTimeLength;

    public void AddPointToPersonality(int num){
        switch (num)
        {
            case 0:
                soccermom++;
                break;
            case 1:
                hermit++;
                break;
            case 2:
                cheap++;
                break;
            case 3:
                tacky++;
                break;
            case 4:
                athlete++;
                break;
            case 5:
                expensive++;
                break;
            case 6:
                perfectionist++;
                break;
            case 7:
                hoarder++;
                break;
            case 8:
                nerd++;
                break;
            default:
                print("That personality doesn't exist!");
                break;
        }
    }

    void Update()
    {
        if (PhotonArenaManager.Instance.CurrentServerUserDepth == PhotonArenaManager.ServerDepthLevel.InRoom) {

            if (timerRunning){
                UpdateTimer();
            } else {
                StartTimer();
            }

            if (_testEnded)
            {
                if ((PhotonArenaManager.Instance.GetClock() - _gameStartTime) >= _timer)
                {
                    _testEnded = false;
                    SceneManager.LoadScene("World2");
                }
            }
        }
    }

    void UpdateTimer(){
        uint curtime = checked((uint)(int)PhotonArenaManager.Instance.GetClock());

        if (secondsCountDown > 0){
            //            secondsCountDown -= Time.deltaTime;
            secondsCountDown = _seconds - (curtime - _gameStartTime)/1000f;
            secondsCountDown = Mathf.Round(secondsCountDown * 100f) / 100f;
            timerText.text = "" + secondsCountDown;
        }else{
            EndTest();
        }
    }


    // a few options only have 1 determining factor to reach personality 
    // type, so they have to give the most points
    public void AddExpensivePoints(){
        expensive += 10;
    }

    public void AddHoarderPoints(){
        hoarder += 10;
    }

    public void CheckForCheapPoints(){
        if(hasSoldDog && money >= 1600){ // means you skipped 3 or more questions
            cheap += 10;
        }else if(!hasSoldDog && money <= 400){ // not expensive, but skipped 3 or more so cheap
            cheap += 10;
        }
    }

    public void UpdateMoneyDisplay(){
        moneyText.text = "$" + money;
    }

    public void AddDogMoney(){
        money += 1200;
        UpdateMoneyDisplay();
    }

    public void StartTimer(){
        _gameStartTime = checked((uint)(int)PhotonArenaManager.Instance.GetData("GameStartTime"));
        timerRunning = true;
    }

    public void SubtractMoney(){
        money -= 200;
        UpdateMoneyDisplay();

        if(money <= 0 && moneyText){
            Destroy(moneyText.GetComponent<GameObject>());
        }
    }

    public void EndTest(){
        timerRunning = false;
        timeToCompleteTest = secondsCountDown;
        timeToCompleteTest = 60 - timeToCompleteTest;
        timeToCompleteTest = timeToCompleteTest * 1000;
        athlete = Mathf.RoundToInt(timeToCompleteTest);

        var score = GetScore();

        int playerNum = PhotonArenaManager.Instance.GetLocalPlayerID();
        var data = new KeyValuePair<int, int[]>(playerNum, score);
        PhotonArenaManager.Instance.SaveData($"score!{playerNum}", score);

        _testEnded = true;

        PhotonNetwork.CurrentRoom.IsVisible = false;

/*        //test code
        var scores = new Dictionary<int, int[]>();
        scores.Add(0, score);

        var soccermom = new PersonalityController();
        soccermom.soccermom = 8;
        soccermom.athlete = 99999;
        scores.Add(11, soccermom.GetScore());

        var hoarder = new PersonalityController();
        hoarder.hoarder = 10;
        hoarder.athlete = 88888;
        scores.Add(22, hoarder.GetScore());

        var expensive = new PersonalityController();
        expensive.expensive = 10;
        expensive.athlete = 77777;
        scores.Add(33, expensive.GetScore());

        var cheap = new PersonalityController();
        cheap.cheap = 10;
        cheap.athlete = 66666;
        scores.Add(44, cheap.GetScore());

        var results = SortingHat(scores);
*/

    }

    public int[] GetScore()
    {
        var score = new int[9];

        score[(int)PersonalityType.Perfectionist] = perfectionist;
        score[(int)PersonalityType.Hoarder] = hoarder;
        score[(int)PersonalityType.Nerd] = nerd;
        score[(int)PersonalityType.Tacky] = tacky;
        score[(int)PersonalityType.Cheap] = cheap;
        score[(int)PersonalityType.Expensive] = expensive;
        score[(int)PersonalityType.Hermit] = hermit;
        score[(int)PersonalityType.Athlete] = athlete;
        score[(int)PersonalityType.SoccerMom] = soccermom;

        return score;
    }


    public static Dictionary<int, PersonalityType> SortingHat(Dictionary<int, int[]> scores)
    {
        var results = new Dictionary<int, PersonalityType>(scores.Count);

        // Find athlete
        if (scores.Count > 0)
        {
            var athlete = scores.OrderBy(s => s.Value[(int)PersonalityType.Athlete]).First();
            scores.Remove(athlete.Key);
            results.Add(athlete.Key, PersonalityType.Athlete);
        }

        // Find expensive
        if (scores.Count > 0)
        {
            var expensive = scores.OrderByDescending(s => s.Value[(int)PersonalityType.Expensive]).ThenBy(s => s.Value[(int)PersonalityType.Athlete]).First();
            scores.Remove(expensive.Key);
            results.Add(expensive.Key, PersonalityType.Expensive);
        }

        // Find cheap
        if (scores.Count > 0)
        {
            var cheap = scores.OrderByDescending(s => s.Value[(int)PersonalityType.Cheap]).ThenBy(s => s.Value[(int)PersonalityType.Athlete]).First();
            scores.Remove(cheap.Key);
            results.Add(cheap.Key, PersonalityType.Cheap);
        }
        // Find hoarder
        if (scores.Count > 0)
        {
            var hoarder = scores.OrderByDescending(s => s.Value[(int)PersonalityType.Hoarder]).ThenBy(s => s.Value[(int)PersonalityType.Athlete]).First();
            scores.Remove(hoarder.Key);
            results.Add(hoarder.Key, PersonalityType.Hoarder);
        }
        // Find nerd
        if (scores.Count > 0)
        {
            var nerd = scores.OrderByDescending(s => s.Value[(int)PersonalityType.Nerd]).ThenBy(s => s.Value[(int)PersonalityType.Athlete]).First();
            scores.Remove(nerd.Key);
            results.Add(nerd.Key, PersonalityType.Nerd);
        }
        // Find tacky
        if (scores.Count > 0)
        {
            var tacky = scores.OrderByDescending(s => s.Value[(int)PersonalityType.Tacky]).ThenBy(s => s.Value[(int)PersonalityType.Athlete]).First();
            scores.Remove(tacky.Key);
            results.Add(tacky.Key, PersonalityType.Tacky);
        }
        // Find Perfectionist
        if (scores.Count > 0)
        {
            var perfectionist = scores.OrderByDescending(s => s.Value[(int)PersonalityType.Perfectionist]).ThenBy(s => s.Value[(int)PersonalityType.Athlete]).First();
            scores.Remove(perfectionist.Key);
            results.Add(perfectionist.Key, PersonalityType.Perfectionist);
        }
        // Find hermit
        if (scores.Count > 0)
        {
            var hermit = scores.OrderByDescending(s => s.Value[(int)PersonalityType.Hermit]).ThenBy(s => s.Value[(int)PersonalityType.Athlete]).First();
            scores.Remove(hermit.Key);
            results.Add(hermit.Key, PersonalityType.Hermit);
        }
        // Find soccermom
        if (scores.Count > 0)
        {
            var soccermom = scores.OrderByDescending(s => s.Value[(int)PersonalityType.SoccerMom]).ThenBy(s => s.Value[(int)PersonalityType.Athlete]).First();
            scores.Remove(soccermom.Key);
            results.Add(soccermom.Key, PersonalityType.SoccerMom);
        }
        return results;
    }
}



/* 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PersonalityController : MonoBehaviour
{
    public int soccermom { get {return pTypes["soccermom"];}}
    public int hermit { get {return pTypes["hermit"];}}
    public int cheap { get {return pTypes["cheap"];}}
    public int tacky { get {return pTypes["tacky"];}}
    public int athlete { get {return pTypes["athlete"];}}
    public int expensive { get {return pTypes["expensive"];}}
    public int perfectionist { get {return pTypes["perfectionist"];}}
    public int hoarder = 0;
    public int nerd = 0;

    public Dictionary<string, int> pTypes = new Dictionary<string, int>();

    public PersonalityController()
    {
        pTypes.Add("soccermom", 0);
        pTypes.Add("hermit", 0);
        pTypes.Add("cheap", 0);
        pTypes.Add("tacky", 0);
        pTypes.Add("athlete", 0);
        pTypes.Add("expensive", 0);
        pTypes.Add("perfectionist", 0);
        pTypes.Add("hoarder", 0);
        pTypes.Add("nerd", 0);

    }

    [Header("Other display information")]
    public int money = 1000;

    public void AddPointToPersonality(string p){
        pTypes[p]++;
    }

    void Start()
    {
    }

    void Update()
    {
        
    }

    public string GetPersonality(string str)
    {
        return str;
    }
}
*/
