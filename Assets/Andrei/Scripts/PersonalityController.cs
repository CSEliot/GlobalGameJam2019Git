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

    [Header("Questions")]
    public string question1 = "";
    public string question2 = "";
    public string question3 = "";
    public string question4 = "";
    public string question5 = "";

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

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void UpdateMoneyDisplay(){
        moneyText.text = "$" + money;
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