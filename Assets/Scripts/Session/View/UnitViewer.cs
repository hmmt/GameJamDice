using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitViewer : MonoBehaviour
{
    [SerializeField] GameObject rendererRoot;

    public UnitStatusData data { private set; get; }

    public void SetIngameUnitData(UnitStatusData data)
    {
        this.data = data;
    }

    public void LoadPlayerSprites()
    {

    }
    public void LoadMonsterSprites(int index)
    {
    }


    private void Update()
    {
        if (data == null)
            return;
        // 대충 유닛 ui 갱신
    }


    public void SetMotionToMove()
    {
    }

    public void SetMotionToIdle()
    {
    }

}
