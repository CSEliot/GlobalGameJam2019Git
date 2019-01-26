﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
     [Header("Player Object Checklists")]
    public GameObject[] checklistPanels;
    public int pageIndex = 0;
    public int startingChecklist = 0;

    [Header("Countdown Timer Settings")]
    public Text timerText;
    private float time = 1200;

    [Header("HUD Settings")]
    public Text instructionText;
    public string instgetitem = "";
    public string instcorrectitem = "";
    public string instbluecouch = "";
    public string instwrongitem = "";

    public Camera mainCam;

    void Start()
    {
        for(int i = 0; i < checklistPanels.Length; i++)
        {
            checklistPanels[i].SetActive(false);
        }

        pageIndex = startingChecklist;
        checklistPanels[startingChecklist].SetActive(true);

        StartCoundownTimer();
    }

    void Update()
    {
        if(Input.GetButtonDown("Page Right")){
            if(pageIndex < 2){
                checklistPanels[pageIndex].SetActive(false);
                pageIndex++;
                checklistPanels[pageIndex].SetActive(true);
            }else{
                checklistPanels[pageIndex].SetActive(false);
                pageIndex = 0;
                checklistPanels[pageIndex].SetActive(true);
            }
        }

        if(Input.GetButtonDown("Page Left")){
            if(pageIndex > 0){
                checklistPanels[pageIndex].SetActive(false);
                pageIndex--;
                checklistPanels[pageIndex].SetActive(true);
            }else{
                checklistPanels[pageIndex].SetActive(false);
                pageIndex = 2;
                checklistPanels[pageIndex].SetActive(true);
            }
        }
        Debug.Log(pageIndex);
    }

    void StartCoundownTimer()
    {
        if (timerText != null)
        {
            time = 360;
            timerText.text = "0:00";
            InvokeRepeating("UpdateTimeLeft", 0.0f, 0.01667f);
        }
    }
 
    void UpdateTimeLeft()
    {
        if (timerText != null)
        {
            time -= Time.deltaTime;
            string minutes = Mathf.Floor(time / 60).ToString("0");
            string seconds = (time % 60).ToString("00");
            timerText.text = minutes + ":" + seconds;
        }
    }

    public void SetInstructionText(int instructionNum){
        if(instructionNum == 0){
            instructionText.text = instgetitem;
        }else if(instructionNum == 1){
            instructionText.text = instcorrectitem;
        }else if(instructionNum == 2){
            instructionText.text = instbluecouch;
        }else if(instructionNum == 3){
            instructionText.text = instwrongitem;
        }
    }
}
