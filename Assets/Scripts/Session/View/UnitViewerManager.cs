using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���⵵ ���⼭
/// </summary>
public class UnitViewerManager : MonoBehaviour
{
    [SerializeField] UnitViewer unitPrefab;

    [SerializeField] Transform playerPosition;
    [SerializeField] Transform[] monsterPositions;

    UnitViewer playerUnit;
    List<UnitViewer> monsterUnits = new List<UnitViewer>();


    /// <summary>
    /// �� ���� ȣ���Ұ�
    /// </summary>
    public void InitializePlayerUnit(UnitStatusData data)
    {
        if (playerUnit != null)
            Destroy(playerUnit.gameObject);
        playerUnit = Instantiate(unitPrefab, transform);
        playerUnit.gameObject.SetActive(true);
        playerUnit.SetIngameUnitData(data);
    }

    public void InitializeMonsterUnits(List<UnitStatusData> monsters)
    {
        ClearMonsters();

        if (monsters.Count > monsterPositions.Length)
            Debug.LogError("position ũ�Ⱑ �̻���");

        for(int i=0; i<monsters.Count && i < monsterPositions.Length; i++)
        {
            var monster = monsters[i];
            var position = monsterPositions[i].position;
            var newMonster = Instantiate(unitPrefab, transform);
            newMonster.gameObject.SetActive(true);
            newMonster.SetIngameUnitData(monster);
            newMonster.transform.localPosition = position;
            monsterUnits.Add(newMonster);
        }
    }

    public void ClearMonsters()
    {
        foreach(var monster in monsterUnits)
        {
            Destroy(monster.gameObject);
        }
        monsterUnits.Clear();
    }

    public void SetToMoveState()
    {
        playerUnit.SetMotionToMove();
    }

    public void SetToIdleState()
    {
        playerUnit.SetMotionToIdle();
    }
}
