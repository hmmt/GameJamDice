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

    /// <summary>
    /// �����϶��� ���
    /// </summary>
    public int monsterIndex;

    public int maxHp;
    public int hp;
    public int shield;

    public int posionCount = 0;

    public bool isPlayer = false;
    public int faction = 0;

    /// <summary>
    /// �ϴ� ���� ����
    /// </summary>
    public List<SessionDeck> deck = new List<SessionDeck>();

    public bool isDead => hp <= 0;

    public void ClearForEndBattle()
    {
        shield = 0;
        posionCount = 0;
    }

    public void TakeDamage(int damage)
    {
        if (!isPlayer)
            damage += 100;
        if (shield > 0)
        {
            if(shield >= damage)
            {
                shield -= damage;
            }
            else
            {
                hp -= damage - shield;
                shield = 0;
            }
        }
        else
            hp -= damage;
        Debug.Log(string.Format("hp : {0}, damage : {1}, player : {2}", hp, damage, isPlayer));
    }

    public void RecoverHp(int value)
    {
        hp += value;
        if (hp > maxHp)
            hp = maxHp;
    }
}
