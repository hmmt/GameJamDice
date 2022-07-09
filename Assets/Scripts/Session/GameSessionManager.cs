using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 진행하는 곳
/// </summary>
public class GameSessionManager : MonoBehaviour
{
    public static GameSessionManager instance;

    private void Awake()
    {
        instance = this;
    }



    void Initialize()
    {
        // 대충 맵 뷰 및 ui 세팅
    }

    void MoveTo(int nodeId)
    {
        // 대충 노드 이동
    }
}