using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class CinematicControlRemover : MonoBehaviour
{
    [SerializeField] private bool PlayOnStart;
    GameObject player;


    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        if (PlayOnStart)
        {
            GetComponent<PlayableDirector>().Play();
        }
    }

    private void OnEnable()
    {
        GetComponent<PlayableDirector>().played += DisableController;
        GetComponent<PlayableDirector>().stopped += EnableController;
    }

    private void OnDisable()
    {
        GetComponent<PlayableDirector>().played -= DisableController;
        GetComponent<PlayableDirector>().stopped -= EnableController;
    }

    private void EnableController(PlayableDirector director)
    {
        player.GetComponent<PlayerController>().enabled = true;
    }

    private void DisableController(PlayableDirector director)
    {
        player.GetComponent<ActionScheduler>().CancleCurrentAction();
        player.GetComponent<PlayerController>().enabled = false;
    }
}