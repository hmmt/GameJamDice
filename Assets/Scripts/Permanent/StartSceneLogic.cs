using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneLogic : MonoBehaviour
{
    void Start()
    {
        if(!PermanentPlayer.instance.initialized)
        {
            InitializePermenentData();
        }
    }

    private void InitializePermenentData()
    {
        PermanentPlayer.instance.InitializePlayer();
    }
    public void StartGame()
    {
        
    }

    public void ResetGame()
    {
        InitializePermenentData();
    }
}
