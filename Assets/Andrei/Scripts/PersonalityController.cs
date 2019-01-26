using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private float secondsCountDown = 60;
    private float timeToCompleteTest = 0;
    private bool timerRunning = false;
    private bool hasSoldDog = false;

    public Text timerText;

    [Header("Other display information")]
    public int money = 1000;
    public Text moneyText;

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

    void Update(){
        if(timerRunning){
            UpdateTimer();
        }
    }

    void UpdateTimer(){
        if(secondsCountDown > 0){
            secondsCountDown -= Time.deltaTime;
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
        score[(int)PersonalityType.Athletic] = athlete;
        score[(int)PersonalityType.SoccerMom] = soccermom;

        return score;
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