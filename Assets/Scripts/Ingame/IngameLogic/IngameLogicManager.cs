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

        // �ֻ��� ���ȴ���
        public bool diceRolled = false;

        // ������ �ֻ�����
        public List<DiceConsequenceData> diceResultList = new List<DiceConsequenceData>();
        public int rolledDiceIndex = 0;
        public bool rolledDiceUsed = false;
        public List<int> usedDices = new List<int>();
    }


    public static IngameLogicManager instance { get; private set; }

    //[SerializeField] SessionPlayer sessionPlayer;
    [SerializeField] IngameUIManager ingameUIManager;
    [SerializeField] UnitViewerManager unitViewerManager;

    event Action onStartBattle;
    event Action<bool> onEndBattle;

    event Action<UnitStatusData> onStartTurn;
    event Action<UnitStatusData, DiceConsequenceData, int> onRollCompleteTurn;
    event Action<UnitStatusData, int> onReadyToUseDice;
    event Action<UnitStatusData, DiceConsequenceData, List<ActionResultData>> onUseDice;
    event Action<UnitStatusData> onEndTurn;

    /// <summary>
    /// ���������� üũ
    /// </summary>
    public event Func<bool> isEffectPhase;

    UnitStatusData playerData = new UnitStatusData();
    List<UnitStatusData> monsterDataList = new List<UnitStatusData>();

    Queue<UnitStatusData> turnOrderQueue = new Queue<UnitStatusData>();
    TurnInfo turnInfo = new TurnInfo();

    /// <summary>
    /// true�� ������
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
        turnInfo.usedDices.Clear();

        monsterDataList.Clear();

        var dungeonData = staticDataManager.GetDungeon(x => x.index == dungeonIndex);
        for (int i=0; i< dungeonData.monsterIndexList.Count; i++)
        {
            UnitStatusData monster = new UnitStatusData();
            var monsterData = staticDataManager.GetMonster(x => x.index == dungeonData.monsterIndexList[i]);

            monster.isPlayer = false;
            monster.faction = 1;
            monster.hp = monster.maxHp = monsterData.hitpoint;
            monster.monsterIndex = monsterData.index;

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

        InvokeOnStartBattle();
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            playerData.TakeDamage(100);
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            foreach(var monster in monsterDataList)
            {
                monster.TakeDamage(100);
            }
        }
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
            // �й�
            EndBattle(false);
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
            // �¸�
            EndBattle(true);
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
        yield return null;
        // �������� ó���� ��� ��κ� ��ŵ ����

        // �� ���� �ʱ�ȭ
        turnInfo = new TurnInfo();
        turnInfo.unit = turnOrderQueue.Dequeue();

        turnInfo.diceResultList.Clear();
        turnInfo.usedDices.Clear();
        turnInfo.rolledDiceIndex = 0;

        if(turnInfo.unit.posionCount > 0)
        {
            turnInfo.unit.posionCount--;
            unitViewerManager.ShowOnlyDamageEffect(turnInfo.unit, turnInfo.unit.maxHp / 10);
            turnInfo.unit.TakeDamage(turnInfo.unit.maxHp / 10);
        }

        if(turnInfo.unit.isDead)
        {
            yield return null;
            if (CheckEndBattle())
            {
                yield break;
            }
            //Debug.Log(10);
            NextUnitTurn();
            yield break;
        }


        // �ǵ� �ʱ�ȭ
        turnInfo.unit.shield = 0;
        InvokeOnStartTurn(turnInfo.unit);

        for (int i=0; i< turnInfo.unit.deck.Count; i++)
        {
            Debug.Log("AA + " + WaitEffectEnd());
            // ���� ���� waiting ������ ���� ����
            while (WaitEffectEnd())
                yield return null;
            if (!turnInfo.unit.isPlayer)
            {
                // �÷��̾ �ƴϸ� �ڵ� ������
                turnInfo.diceRolled = true;
            }
            else
            {
                turnInfo.diceRolled = false;
            }

            //Debug.Log(1);
            //Debug.Log(2);
            while (!turnInfo.diceRolled)
                yield return null;

            var dcdata = new DiceConsequenceData(turnInfo.unit.deck[i].behaviourDice.GetRandomBehaviourState(),
                                                       turnInfo.unit.deck[i].actingPowerDice.GetRandomActingPower());

            turnInfo.diceResultList.Add(dcdata);

            InvokeOnCompleteRollDice(turnInfo.unit, dcdata, i);

            Debug.Log("BB + " +WaitEffectEnd());
            // �ֻ��� ������ ���� ���
            while (WaitEffectEnd())
                yield return null;
        }

        //Debug.Log(4);
        
        //Debug.Log(5);
        while (turnInfo.rolledDiceIndex < turnInfo.diceResultList.Count)
        {
            var result = turnInfo.diceResultList[turnInfo.rolledDiceIndex];

            turnInfo.rolledDiceUsed = false;

            //InvokeOnReadyToUseDice(turnInfo.unit, turnInfo.rolledDiceIndex);

            List<ActionResultData> actionResultList; 

            if (turnInfo.unit.isPlayer &&
                (result.behaviourState == BehaviourState.offense || result.behaviourState == BehaviourState.poison))
            {
                // �÷��̾��� �����̰�, Ÿ���� ������ �ֻ����� �Է� ���
                //InvokeOnReadyToUseDice(turnInfo.unit, turnInfo.rolledDiceIndex);
                actionResultList = UseDice(turnInfo.rolledDiceIndex, null);
            }
            else
            {
                // �� �ܿ��� �ٷ� ���
                actionResultList = UseDice(turnInfo.rolledDiceIndex, null);
            }
            //Debug.Log(6 + "-" + turnInfo.rolledDiceIndex);
            // �ֻ��� ��� ���
            while (!turnInfo.rolledDiceUsed)
                yield return null;

            //Debug.Log(7 + "-" + turnInfo.rolledDiceIndex);

            InvokeOnUseDice(turnInfo.unit, result, actionResultList);

            while (WaitEffectEnd())
                yield return null;

            turnInfo.rolledDiceIndex++;
            //Debug.Log(8 + "-" + turnInfo.rolledDiceIndex);
        }

        //Debug.Log(8);
        InvokeOnEndTurn(turnInfo.unit);
        //Debug.Log(9);


        if (CheckEndBattle())
        {
            yield break;
        }
        //Debug.Log(10);
        NextUnitTurn();
    }

    public void RollMyDice()
    {
        turnInfo.diceRolled = true;
    }

    /// <summary>
    /// �ֻ��� ���
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <param name="unit"></param>
    public List<ActionResultData> UseDice(int slotIndex, UnitStatusData unit)
    {
        if (turnInfo.rolledDiceIndex == slotIndex
            && slotIndex < turnInfo.diceResultList.Count)
        {
            turnInfo.rolledDiceUsed = true;
            var actor = turnInfo.unit;
            var diceResult = turnInfo.diceResultList[slotIndex];
            if (unit == null)
            {
                switch (diceResult.behaviourState)
                {
                    case BehaviourState.offense:
                        unit = actor.isPlayer ? monsterDataList.Where(x => !x.isDead).FirstOrDefault() : playerData;
                        break;
                    case BehaviourState.defense:
                        // ��ȣ��
                        break;
                    case BehaviourState.lightning:
                        // ����
                        break;
                    case BehaviourState.poison:
                        unit = actor.isPlayer ? monsterDataList.Where(x => !x.isDead).FirstOrDefault() : playerData;
                        break;
                    case BehaviourState.recovery:
                        break;
                }
            }
            else
            {
                // ��ȿ�� Ÿ�� ���� ����
            }

            List<ActionResultData> resultList = new List<ActionResultData>();

            switch (diceResult.behaviourState)
            {
                case BehaviourState.offense:
                    if (unit == null)
                        break;
                    unit.TakeDamage(diceResult.actingPower);
                    resultList.Add(new ActionResultData(unit, diceResult.actingPower));
                    break;
                case BehaviourState.defense:
                    // ��ȣ��
                    actor.shield += diceResult.actingPower;
                    break;
                case BehaviourState.lightning:
                    // ����
                    if(actor.isPlayer)
                    {
                        foreach(var monster in monsterDataList)
                        {
                            monster.TakeDamage(diceResult.actingPower);
                            resultList.Add(new ActionResultData(unit, diceResult.actingPower));
                        }
                    }
                    else
                    {
                        if (unit == null)
                            break;
                        unit.TakeDamage(diceResult.actingPower);
                        resultList.Add(new ActionResultData(unit, diceResult.actingPower));
                    }
                    break;
                case BehaviourState.poison:
                    if (unit == null)
                        break;
                    unit.TakeDamage(diceResult.actingPower);
                    unit.posionCount++;
                    resultList.Add(new ActionResultData(unit, diceResult.actingPower));
                    break;
                case BehaviourState.recovery:
                    actor.RecoverHp(diceResult.actingPower);
                    break;
            }
            
            return resultList;
        }
        return new List<ActionResultData>();
    }


    public void EndBattle(bool win)
    {
        if (!inBattle)
            return;
        inBattle = false;
        unitViewerManager.ClearMonsters();
        onEndBattle?.Invoke(win);
    }
    #endregion
    #region event

    /// <summary>
    /// ������ ���� �� ��
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
    /// ������ ������ ���� ���� ��
    /// </summary>
    public IngameLogicManager AddActionOnEndBattle(Action<bool> callback)
    {
        onEndBattle -= callback;
        onEndBattle += callback;
        return this;
    }

    public IngameLogicManager RemoveActionOnEndbattle(Action<bool> callback)
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

    public IngameLogicManager AddActionOnRollCompleteTurn(Action<UnitStatusData, DiceConsequenceData, int> callback)
    {
        onRollCompleteTurn -= callback;
        onRollCompleteTurn += callback;
        return this;
    }

    public IngameLogicManager RemoveActionOnRollCompleteTurn(Action<UnitStatusData, DiceConsequenceData, int> callback)
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

    public void InvokeOnStartTurn(UnitStatusData unit)
    {
        onStartTurn?.Invoke(unit);
    }

    public void InvokeOnCompleteRollDice(UnitStatusData unit, DiceConsequenceData sessionDecks, int diceIndex)
    {
        onRollCompleteTurn?.Invoke(unit, sessionDecks, diceIndex);
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
