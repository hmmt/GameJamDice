using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ActionResultData
{
    public UnitStatusData unit;
    public int damage;

    public ActionResultData(UnitStatusData unit, int damage)
    {
        this.unit = unit;
        this.damage = damage;
    }
}
