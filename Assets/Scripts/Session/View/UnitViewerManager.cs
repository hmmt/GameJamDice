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
        playerUnit.LoadPlayerSprites();
        playerUnit.SetIngameUnitData(data);
        playerUnit.transform.localPosition = playerPosition.position;


        // 플레이어 클릭은 일단 제외
        //playerUnit.Init(OnClickUnit);
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
            newMonster.LoadMonsterSprites(monster.monsterIndex);
            newMonster.transform.localPosition = position;
            monsterUnits.Add(newMonster);

            newMonster.Init(OnClickUnit);
        }
    }

    public void InitBattle(IngameLogicManager logic)
    {
        //logic.InvokeOnUseDice()
        logic.AddActionOnUseDice(OnUseDice);
        logic.AddActionOnSelectDiceSlot(OnSelectDice);
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

    public void ShowOnlyDamageEffect(UnitStatusData unit, int damage)
    {
        var viewer = FindUnit(unit);
        if (viewer != null)
        {
            damageEffectPool.PlayDamageEffect(damage, viewer.transform.position);
        }
    }

    #region callback

    private void OnClickUnit(UnitViewer unit)
    {
        IngameLogicManager.instance.SelectUnit(unit.data);
    }

    private void OnSelectDice(int diceIndex)
    {
        if(IngameLogicManager.instance.IsUsableDiceSlot(diceIndex))
        {
            var diceData = IngameLogicManager.instance.GetDiceResultData(diceIndex);

            if (diceData.behaviourState == BehaviourState.offense || diceData.behaviourState == BehaviourState.poison)
            {
                foreach (var monster in monsterUnits)
                {
                    monster.ShowSelectionMark();
                }
            }
        }
        else
        {
            foreach (var monster in monsterUnits)
            {
                monster.HideSelectionMark();
            }
        }
    }
    private void OnUseDice(UnitStatusData unit, DiceConsequenceData diceData, int diceIndex, List<ActionResultData> resultList)
    {
        foreach (var monster in monsterUnits)
        {
            monster.HideSelectionMark();
        }
        StartCoroutine(PlayDamageEffect(unit, diceData, resultList));
    }

    IEnumerator PlayDamageEffect(UnitStatusData unit, DiceConsequenceData diceData, List<ActionResultData> resultList)
    {
        waiting = true;
        yield return null;
        try
        {
            if(diceData.behaviourState == BehaviourState.offense ||
                diceData.behaviourState == BehaviourState.poison)
            {
                var actor = FindUnit(unit);
                if (actor != null)
                {
                    actor.SetMotionToAttack();
                }
                foreach (var result in resultList)
                {
                    var viewer = FindUnit(result.unit);
                    if (viewer != null)
                    {
                        damageEffectPool.PlayDamageEffect(result.damage, viewer.transform.position);
                    }
                }
            }
        }
        catch(System.Exception e)
        {

        }

        yield return new WaitForSeconds(0.2f);
        waiting = false;

    }
    #endregion
}
