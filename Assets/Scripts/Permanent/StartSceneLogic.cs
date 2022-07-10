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
            PermanentPlayer.instance.ResetShopData();
        }
        StartGame();
    }

    private void InitializePermenentData()
    {
        PermanentPlayer.instance.InitializePlayer();

    }
    public void StartGame()
    {
        SessionPlayer.instance.StartGameSession(PermanentPlayer.instance);

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void ResetGame()
    {
        InitializePermenentData();
        PermanentPlayer.instance.ResetShopData();
    }
}
