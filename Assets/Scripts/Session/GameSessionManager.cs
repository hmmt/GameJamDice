using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �����ϴ� ��
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
        // ���� �� �� �� ui ����
    }

    void MoveTo(int nodeId)
    {
        // ���� ��� �̵�
    }
}