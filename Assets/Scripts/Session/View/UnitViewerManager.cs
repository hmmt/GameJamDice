using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유닛 연출도 여기서
/// </summary>
public class UnitViewerManager : MonoBehaviour
{
    [SerializeField] DamageEffectPool damageEffectPool;

    [SerializeField] UnitViewer unitPrefab;

    [SerializeField] Transform playerPosition;
    [SerializeField] Transform[] monsterPositions;

    UnitViewer playerUnit;
    List<UnitViewer> monsterUnits = new List<UnitViewer>();

    public bool waiting { private set; get; }

    /// <summary>
    /// 한 번만 호출할것
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
            Debug.LogError("position 크기가 이상함");

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

    public void InitBattle(IngameLogicManager logic)
    {
        //logic.InvokeOnUseDice()
        logic.AddActionOnUseDice(OnUseDice);
    }

    public void ClearMonsters()
    {
        foreach(var monster in monsterUnits)
        {
            Destroy(monster.gameObject);
        }
        monsterUnits.Clear();
    }

    private UnitViewer FindUnit(UnitStatusData unit)
    {
        if(playerUnit.data == unit)
        {
            return playerUnit;
        }

        return monsterUnits.Find(x => x.data == unit);
    }

    public void SetToMoveState()
    {
        playerUnit.SetMotionToMove();
    }

    public void SetToIdleState()
    {
        playerUnit.SetMotionToIdle();
    }

    #region callback
    private void OnUseDice(UnitStatusData unit, DiceConsequenceData diceData, List<ActionResultData> resultList)
    {
        StartCoroutine(PlayDamageEffect(unit, diceData, resultList));
    }

    IEnumerator PlayDamageEffect(UnitStatusData unit, DiceConsequenceData diceData, List<ActionResultData> resultList)
    {
        waiting = true;
        yield return null;
        try
        {
            foreach (var result in resultList)
            {
                var viewer = FindUnit(result.unit);
                if (viewer != null)
                {
                    damageEffectPool.PlayDamageEffect(result.damage, viewer.transform.position);
                }
            }
        }
        catch(System.Exception e)
        {

        }

        yield return new WaitForSeconds(1);
        waiting = false;

    }
    #endregion
}
