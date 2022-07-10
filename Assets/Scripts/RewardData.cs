using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RewardData
{
    public bool isBehaviour;
    public int staticDataIndex;

    public RewardData(bool isBehaviour, int staticDataIndex)
    {
        this.isBehaviour = isBehaviour;
        this.staticDataIndex = staticDataIndex;
    }
}
