﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System;

public class TimelineController : MonoBehaviour
{
    public GameObject[] dates;
    public GameObject[] doors =  new GameObject[3];

    public GameObject intro;
    VideoPlayer introVidPlayer;

    [SerializeField]
    int curDate = 0;

    GameObject timeline;

    public AudioSource doorSound;

    //variable related to opening door
    bool doorOpen = false;
    float timer = 0;
    public float timeToOpenDoor = 2;

    //video
    bool introDone = false;

    //buttons
    public GameObject moveToPath;

    public GameObject posRef;

    void Awake()
    {
        //hide all the dates
        foreach (GameObject d in dates)
        {
            d.SetActive(false);
        }

        //set timeline object
        timeline = dates[0].transform.parent.gameObject;
        introVidPlayer = intro.GetComponent<VideoPlayer>();

       
    }




    private void Update()
    {
        if (!introDone)
        {
            CheckVidsEnd();
        }

        if (!doorOpen && curDate == dates.Length)
        {
            if (timer == 0)
            {
                Debug.Log("Door timer started");
            }
           
            if(timer < timeToOpenDoor){
                timer += Time.deltaTime;
            } else
            {
                Debug.Log("Doors open");
                OpenDoor();
                doorOpen = true;
               
            }
        }
    }

    public void ShowTimeline()
    { 
        //Show first 3 dates
        dates[0].SetActive(true);
        dates[1].SetActive(true);
        dates[2].SetActive(true);

        //save the current door position
        LocationInfo.Instance.SetDoorPos(doors[0]);
        LocationInfo.Instance.PrintDoorPos();
        LocationInfo.Instance.SetScenePos(timeline);
        LocationInfo.Instance.SetPillarLocation(posRef);

    }

    public void OpenDoor()
    {
        doorSound.Play();
        StartCoroutine(OpenDoorCo());
    }

    IEnumerator OpenDoorCo()
    {
        //Open doors

        Vector3 offset = new Vector3(0,0,-1.5f);
        float duration = 6.0f;
        float time = 0.0f;

        Vector3[] ogPos = {doors[0].transform.position,
                           doors[1].transform.position,
                           doors[2].transform.position};
        

        while (time <= duration)
        {
            time  = time + Time.deltaTime;
            float percent = Mathf.Clamp01(time / duration);
      
            doors[0].transform.position = Vector3.Lerp(ogPos[0], ogPos[0] + offset, percent);
            doors[1].transform.position = Vector3.Lerp(ogPos[1], ogPos[1] + offset, percent);
            doors[2].transform.position = Vector3.Lerp(ogPos[2], ogPos[2] + offset, percent);

            yield return null;
        }
        //Switch scenes
        //StartCoroutine(Timer(4, LoadPathScene));
        moveToPath.SetActive(true);
        yield return null;

    }

    IEnumerator Timer(float t, Action DoneWait)
    {
        //print("WaitAndPrint " + Time.time);
        yield return new WaitForSeconds(t);
        //print("WaitAndPrint " + Time.time);
        DoneWait?.Invoke();

    }


    //Show the marker at the current index
    public void ShowNextDate()
    {
        if (curDate < dates.Length - 3)
        {
            dates[curDate+3].SetActive(true);
        }

        curDate += 1;


    }

    void CheckVidsEnd()
    {
        if (introVidPlayer.time > introVidPlayer.clip.length - 6)
        {
            Debug.Log("vid done");
            introDone = true;
            ShowTimeline();

        }
    }

    public void LoadPathScene()
    {
        SceneManager.LoadScene("Path");
    }



}
