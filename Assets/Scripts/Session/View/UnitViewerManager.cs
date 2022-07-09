using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitViewerManager : MonoBehaviour
{
    [SerializeField] List<UnitViewer> playerUnits;

    public void SetToMoveState()
    {
        foreach(var unit in playerUnits)
        {
            unit.SetMotionToMove();
        }
    }

    public void SetToIdleState()
    {
        foreach (var unit in playerUnits)
        {
            unit.SetMotionToIdle();
        }
    }
}
