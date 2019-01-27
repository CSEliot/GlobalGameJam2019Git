using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public Animator fade;
    bool hasFaded = false;
    bool canFade = false;

    void Start(){
        StartCoroutine(WaitToGiveControl());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey && !hasFaded && canFade){
            hasFaded = true;
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade(){
        fade.SetBool("fade",true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Menu");
    }

    IEnumerator WaitToGiveControl(){
        yield return new WaitForSeconds(2.5f);
        canFade = true;
    }
}
