using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �������� ����
/// </summary>
public class SessionDungeon
{
    public List<SessionDungeonNode> nodes;


    public static SessionDungeon CreateDungeonByStaticData()
    {
        return new SessionDungeon();
    }
}
