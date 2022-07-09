using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IngameLogicManager : MonoBehaviour
{
    public class TurnInfo
    {
        public UnitStatusData unit = null;

        // 주사위 굴렸는지
        public bool diceRolled = false;

        // 굴려진 주사위들
        public List<DiceConsequenceData> diceResultList = new List<DiceConsequenceData>();
        public int rolledDiceIndex = 0;
        public bool rolledDiceUsed = false;
    }


    public static IngameLogicManager instance { get; private set; }

    //[SerializeField] SessionPlayer sessionPlayer;
    [SerializeField] IngameUIManager ingameUIManager;
    [SerializeField] UnitViewerManager unitViewerManager;

    event Action onStartBattle;
    event Action onEndBattle;

    event Action<UnitStatusData> onStartTurn;
    event Action<UnitStatusData, List<DiceConsequenceData>> onRollCompleteTurn;
    event Action<UnitStatusData, int> onReadyToUseDice;
    event Action<UnitStatusData, DiceConsequenceData, List<ActionResultData>> onUseDice;
    event Action<UnitStatusData> onEndTurn;

    /// <summary>
    /// 연출중인지 체크
    /// </summary>
    public event Func<bool> isEffectPhase;

    UnitStatusData playerData = new UnitStatusData();
    List<UnitStatusData> monsterDataList = new List<UnitStatusData>();

    Queue<UnitStatusData> turnOrderQueue = new Queue<UnitStatusData>();
    TurnInfo turnInfo = new TurnInfo();

    /// <summary>
    /// true면 전투중
    /// </summary>
    bool inBattle = false;

    private void Awake()
    {
        instance = this;
    }

    public void Init(SessionPlayer player, int dungeonIndex)
    {
        StaticDataManager staticDataManager = FindObjectOfType<StaticDataManager>();

        turnInfo.diceResultList.Clear();

        monsterDataList.Clear();
        for (int i=0; i<2; i++)
        {
            UnitStatusData monster = new UnitStatusData();
            var monsterData = staticDataManager.GetMonster(x => x.index == i);

            monster.isPlayer = false;
            monster.faction = 1;
            monster.hp = monster.maxHp = monsterData.hitpoint;

            monster.deck = CreateMonsterDeck(staticDataManager, monsterData);
            monsterDataList.Add(monster);
        }

        playerData = player.unitData;
        //playerData.instanceId = 1;
        playerData.deck.Clear();
        playerData.deck.AddRange(player.deck);
        playerData.isPlayer = true;
        playerData.faction = 0;

        isEffectPhase = null;

        inBattle = true;

        unitViewerManager.InitializeMonsterUnits(monsterDataList);
        unitViewerManager.InitBattle(this);
        ingameUIManager.SetPlayer(player)
                       .Init(this);

        NextUnitTurn();
    }

    private List<SessionDeck> CreateMonsterDeck(StaticDataManager staticDataManager, S3MonsterData data)
    {
        List<SessionDeck> output = new List<SessionDeck>();
        output.Add(new SessionDeck().SetBehaviourDice(staticDataManager.GetBehaviourDice(x => x.index == data.behaviourDice_1))
            .SetActingPowerDice(staticDataManager.GetActionPowerDice(x => x.index == data.actingPowerDice_1)));
        output.Add(new SessionDeck().SetBehaviourDice(staticDataManager.GetBehaviourDice(x => x.index == data.behaviourDice_2))
            .SetActingPowerDice(staticDataManager.GetActionPowerDice(x => x.index == data.actingPowerDice_2)));
        output.Add(new SessionDeck().SetBehaviourDice(staticDataManager.GetBehaviourDice(x => x.index == data.behaviourDice_3))
            .SetActingPowerDice(staticDataManager.GetActionPowerDice(x => x.index == data.actingPowerDice_3)));
        output.Add(new SessionDeck().SetBehaviourDice(staticDataManager.GetBehaviourDice(x => x.index == data.behaviourDice_4))
            .SetActingPowerDice(staticDataManager.GetActionPowerDice(x => x.index == data.actingPowerDice_4)));
        output.Add(new SessionDeck().SetBehaviourDice(staticDataManager.GetBehaviourDice(x => x.index == data.behaviourDice_5))
            .SetActingPowerDice(staticDataManager.GetActionPowerDice(x => x.index == data.actingPowerDice_5)));

        output.RemoveAll(x => x.behaviourDice == null || x.actingPowerDice == null);

        return output;
    }
    #region logic

    bool WaitEffectEnd()
    {
        return ingameUIManager.waiting || unitViewerManager.waiting;
    }

    private void ResetTurnOrderQueue()
    {
        turnOrderQueue.Enqueue(playerData);

        foreach (var data in monsterDataList)
        {
            turnOrderQueue.Enqueue(data);
        }
    }

    private bool CheckEndBattle()
    {
        if (playerData.isDead)
        {
            // 패배
            InvokeOnEndBattle();
            return true;
        }


        bool alldead = true;
        foreach (var unit in monsterDataList)
        {
            if (!unit.isDead)
            {
                alldead = false;
                break;
            }
        }
        if (alldead)
        {
            // 승리
            InvokeOnEndBattle();
            return true;
        }
        return false;
    }
    private void NextUnitTurn()
    {
        if (turnOrderQueue.Count == 0)
        {
            ResetTurnOrderQueue();
        }

        if (turnOrderQueue.Count > 0)
        {
            StartCoroutine(ExecuteUnitTurn());
        }
        else
        {
            Debug.LogError("turn queue error");
        }
    }

    IEnumerator ExecuteUnitTurn()
    {
        // 서버에서 처리할 경우 대부분 스킵 가능

        // 턴 정보 초기화
        turnInfo = new TurnInfo();
        turnInfo.unit = turnOrderQueue.Dequeue();


        if(!turnInfo.unit.isPlayer)
        {
            // 플레이어가 아니면 자동 굴리기
            turnInfo.diceRolled = true;
        }

        Debug.Log(1);
        InvokeOnStartTurn(turnInfo.unit);
        Debug.Log(2);
        while (!turnInfo.diceRolled)
            yield return null;

        Debug.Log(3);


        // 주사위 굴리기
        var consequnceList = new List<DiceConsequenceData>();
        playerData.deck.ForEach(deck =>
        {
            consequnceList.Add(new DiceConsequenceData(deck.behaviourDice.GetRandomBehaviourState(),
                                                       deck.actingPowerDice.GetRandomActingPower()));
        });

        turnInfo.diceResultList.Clear();
        turnInfo.diceResultList.AddRange(consequnceList);
        turnInfo.rolledDiceIndex = 0;

        InvokeOnCompleteRollDice(turnInfo.unit, consequnceList);

        Debug.Log(4);
        // 주사위 굴리는 연출 대기
        while (WaitEffectEnd())
            yield return null;
        Debug.Log(5);
        while (turnInfo.rolledDiceIndex < turnInfo.diceResultList.Count)
        {
            var result = turnInfo.diceResultList[turnInfo.rolledDiceIndex];

            turnInfo.rolledDiceUsed = false;

            //InvokeOnReadyToUseDice(turnInfo.unit, turnInfo.rolledDiceIndex);

            List<ActionResultData> actionResultList; 

            if (turnInfo.unit.isPlayer &&
                (result.behaviourState == BehaviourState.offense || result.behaviourState == BehaviourState.poison))
            {
                // 플레이어의 차례이고, 타겟팅 가능한 주사위면 입력 대기
                //InvokeOnReadyToUseDice(turnInfo.unit, turnInfo.rolledDiceIndex);
                actionResultList = UseDice(turnInfo.rolledDiceIndex, null);
            }
            else
            {
                // 그 외에는 바로 사용
                actionResultList = UseDice(turnInfo.rolledDiceIndex, null);
            }
            Debug.Log(6 + "-" + turnInfo.rolledDiceIndex);
            // 주사위 사용 대기
            while (!turnInfo.rolledDiceUsed)
                yield return null;

            Debug.Log(7 + "-" + turnInfo.rolledDiceIndex);

            InvokeOnUseDice(turnInfo.unit, result, actionResultList);
            while (WaitEffectEnd())
                yield return null;

            turnInfo.rolledDiceIndex++;
            Debug.Log(8 + "-" + turnInfo.rolledDiceIndex);
        }

        Debug.Log(8);
        InvokeOnEndTurn(turnInfo.unit);
        Debug.Log(9);


        if (CheckEndBattle())
        {
            yield break;
        }
        Debug.Log(10);
        NextUnitTurn();
    }

    public void RollMyDice()
    {
        turnInfo.diceRolled = true;
    }

    /// <summary>
    /// 주사위 사용
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <param name="unit"></param>
    public List<ActionResultData> UseDice(int slotIndex, UnitStatusData unit)
    {
        if (turnInfo.rolledDiceIndex == slotIndex
            && slotIndex < turnInfo.diceResultList.Count)
        {
            var actor = turnInfo.unit;
            var diceResult = turnInfo.diceResultList[slotIndex];
            if (unit == null)
            {
                switch (diceResult.behaviourState)
                {
                    case BehaviourState.offense:
                        unit = actor.isPlayer ? monsterDataList.Where(x => !x.isDead).FirstOrDefault() : playerData;
                        if (unit == null)
                        {
                            Debug.LogError("???");
                            return new List<ActionResultData>();
                        }
                        break;
                    case BehaviourState.defense:
                        // 보호막
                        break;
                    case BehaviourState.lightning:
                        // 연쇄
                        break;
                    case BehaviourState.poison:
                        unit = actor.isPlayer ? monsterDataList.Where(x => !x.isDead).FirstOrDefault() : playerData;
                        if (unit == null)
                        {
                            Debug.LogError("???");
                            return new List<ActionResultData>();
                        }
                        break;
                    case BehaviourState.recovery:
                        break;
                }
            }
            else
            {
                // 유효한 타겟 랜덤 선택
            }

            List<ActionResultData> resultList = new List<ActionResultData>();

            switch (diceResult.behaviourState)
            {
                case BehaviourState.offense:
                    // 횟수로 바꿔야 함
                    //unit.hp -= diceResult.actingPower;
                    unit.TakeDamage(diceResult.actingPower);
                    resultList.Add(new ActionResultData(unit, diceResult.actingPower));
                    break;
                case BehaviourState.defense:
                    // 보호막
                    break;
                case BehaviourState.lightning:
                    // 연쇄
                    break;
                case BehaviourState.poison:
                    // 횟수로 바꿔야 함
                    unit.TakeDamage(diceResult.actingPower);
                    resultList.Add(new ActionResultData(unit, diceResult.actingPower));
                    break;
                case BehaviourState.recovery:
                    break;
            }
            turnInfo.rolledDiceUsed = true;
            return resultList;
        }
        return new List<ActionResultData>();
    }


    public void EndBattle()
    {
        if (!inBattle)
            return;
        inBattle = false;
        unitViewerManager.ClearMonsters();
        onEndBattle?.Invoke();
    }
    #endregion
    #region event

    /// <summary>
    /// 전투가 시작 될 때
    /// </summary>
    public IngameLogicManager AddActionOnStartBattle(Action callback)
    {
        onStartBattle -= callback;
        onStartBattle += callback;
        return this;
    }

    public IngameLogicManager RemoveActionOnStartBattle(Action callback)
    {
        onStartBattle -= callback;
        return this;
    }

    /// <summary>
    /// 전투가 끝나고 보상 받을 때
    /// </summary>
    public IngameLogicManager AddActionOnEndBattle(Action callback)
    {
        onEndBattle -= callback;
        onEndBattle += callback;
        return this;
    }

    public IngameLogicManager RemoveActionOnEndbattle(Action callback)
    {
        onEndBattle -= callback;
        return this;
    }

    public IngameLogicManager AddActionOnStartTurn(Action<UnitStatusData> callback)
    {
        onStartTurn -= callback;
        onStartTurn += callback;
        return this;
    }

    public IngameLogicManager RemoveActionOnStartTurn(Action<UnitStatusData> callback)
    {
        onStartTurn -= callback;
        return this;
    }

    public IngameLogicManager AddActionOnRollCompleteTurn(Action<UnitStatusData, List<DiceConsequenceData>> callback)
    {
        onRollCompleteTurn -= callback;
        onRollCompleteTurn += callback;
        return this;
    }

    public IngameLogicManager RemoveActionOnRollCompleteTurn(Action<UnitStatusData, List<DiceConsequenceData>> callback)
    {
        onRollCompleteTurn -= callback;
        return this;
    }

    public IngameLogicManager AddActionOnUseDice(Action<UnitStatusData, DiceConsequenceData, List<ActionResultData>> callback)
    {
        onUseDice -= callback;
        onUseDice += callback;
        return this;
    }

    public IngameLogicManager RemoveActionOnRollCompleteTurn(Action<UnitStatusData, DiceConsequenceData, List<ActionResultData>> callback)
    {
        onUseDice -= callback;
        return this;
    }

    public IngameLogicManager AddActionOnEndTurn(Action<UnitStatusData> callback)
    {
        onEndTurn += callback;
        return this;
    }


    public IngameLogicManager RemoveActionOnEndTurn(Action<UnitStatusData> callback)
    {
        onEndTurn -= callback;
        return this;
    }

    #endregion

    public void InvokeOnReadyToUseDice(UnitStatusData unit, int rolledDiceIndex)
    {
        onReadyToUseDice?.Invoke(unit, rolledDiceIndex);
    }

    public void InvokeOnStartBattle()
    {
        onStartBattle?.Invoke();
    }
    public void InvokeOnEndBattle()
    {
        onEndBattle?.Invoke();
    }

    public void InvokeOnStartTurn(UnitStatusData unit)
    {
        onStartTurn?.Invoke(unit);
    }

    public void InvokeOnCompleteRollDice(UnitStatusData unit, List<DiceConsequenceData> sessionDecks)
    {
        onRollCompleteTurn?.Invoke(unit, sessionDecks);
    }

    public void InvokeOnUseDice(UnitStatusData unit, DiceConsequenceData diceData, List<ActionResultData> actionResultList)
    {
        onUseDice?.Invoke(unit, diceData, actionResultList);
    }

    public void InvokeOnEndTurn(UnitStatusData unit)
    {
        //onEndMyTurn?.Invoke(sessionDecks);
        onEndTurn?.Invoke(unit);
    }

    public void InvokeOnStartEnemyTurn(List<DiceConsequenceData> sessionDecks)
    {
        //onStartEnemyTurn?.Invoke(sessionDecks);
    }

    public void InvokeOnEndEnemyTurn(List<DiceConsequenceData> sessionDecks)
    {
        //onEndEnemyTurn?.Invoke(sessionDecks);
    }
}
