using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ��
/// 
/// �Ŀ� �ٸ� ��(���� �� ��) Ŭ������ ���ľ� �� ���� ����
/// </summary>
public class StartInventory
{
    // �������� �ϴ� public
    public List<S3BehaviourDice> behaviourDiceList = new List<S3BehaviourDice>();
    public List<S3ActingPowerDice> actionPowerDiceList = new List<S3ActingPowerDice>();

    public void Clear()
    {
        behaviourDiceList.Clear();
        actionPowerDiceList.Clear();
    }
}
