using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유닛 상태 데이터
/// </summary>
public class UnitStatusData
{
    /// <summary>
    /// 한 전투 동안만 유지되는 아이디
    /// </summary>
    public int instanceId;

    public int maxHp;
    public int hp;
    public int shield;

    public bool isPlayer = false;
    public int faction = 0;

    /// <summary>
    /// 일단 몬스터 전용
    /// </summary>
    public List<SessionDeck> deck = new List<SessionDeck>();

    public List<UnitBuf> bufs = new List<UnitBuf>();

    public bool isDead => hp <= 0;

    public void ClearForEndBattle()
    {
        shield = 0;
        bufs.Clear();
    }
}

public class UnitBuf
{

}


