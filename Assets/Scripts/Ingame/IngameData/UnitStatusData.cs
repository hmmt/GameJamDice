using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���� ������
/// </summary>
public class UnitStatusData
{
    /// <summary>
    /// �� ���� ���ȸ� �����Ǵ� ���̵�
    /// </summary>
    public int instanceId;

    public int maxHp;
    public int hp;
    public int shield;

    public bool isPlayer = false;
    public int faction = 0;

    /// <summary>
    /// �ϴ� ���� ����
    /// </summary>
    public List<SessionDeck> deck = new List<SessionDeck>();

    public List<UnitBuf> bufs = new List<UnitBuf>();

    public bool isDead => hp <= 0;

    public void ClearForEndBattle()
    {
        shield = 0;
        bufs.Clear();
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log(string.Format("hp : {0}, damage : {1}, player : {2}", hp, damage, isPlayer));
    }
}

public class UnitBuf
{

}


