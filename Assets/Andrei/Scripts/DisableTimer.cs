using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTimer : MonoBehaviour{
    public float timer = 5.0f;

    void Start()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer(){
        yield return new WaitForSeconds(timer);
        GetComponent<GameObject>().SetActive(false);
    }
}
