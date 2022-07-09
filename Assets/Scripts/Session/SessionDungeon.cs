using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 맵의 전반적인 정보
/// </summary>
public class SessionDungeon
{
    public List<SessionDungeonNode> nodes;


    public static SessionDungeon CreateDungeonByStaticData()
    {
        return new SessionDungeon();
    }
}
