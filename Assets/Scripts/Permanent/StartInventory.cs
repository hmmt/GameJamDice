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
    public List<S3BehaviourDiceData> behaviourDiceList = new List<S3BehaviourDiceData>();
    public List<S3ActingPowerDiceData> actionPowerDiceList = new List<S3ActingPowerDiceData>();

    public void Clear()
    {
        behaviourDiceList.Clear();
        actionPowerDiceList.Clear();
    }
}
