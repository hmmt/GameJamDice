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

    /// <summary>
    /// �ϴ� ���� ����
    /// </summary>
    public List<SessionDeck> deck = new List<SessionDeck>();

    public List<UnitBuf> bufs = new List<UnitBuf>();

    public void ClearForEndBattle()
    {
        shield = 0;
        bufs.Clear();
    }
}

public class UnitBuf
{

}


