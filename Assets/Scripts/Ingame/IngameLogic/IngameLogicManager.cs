using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameLogicManager : MonoBehaviour
{
    public static IngameLogicManager instance { get; private set; }

    //[SerializeField] SessionPlayer sessionPlayer;
    [SerializeField] IngameUIManager ingameUIManager;
    [SerializeField] UnitViewerManager unitViewerManager;

    event Action onStartBattle;
    event Action onEndBattle;


    event Action onStartMyTurn;
    event Action<List<DiceConsequenceData>> onRollCompleteMyTurn;
    event Action<int> onReadyToUseDice;
    event Action onEndMyTurn;

    event Action<UnitStatusData> onStartEnemyTurn;
    event Action<UnitStatusData, List<DiceConsequenceData>> onRollCompleteEnemyTurn;
    event Action<UnitStatusData> onEndEnemyTurn;

    /// <summary>
    /// 연출중인지 체크
    /// </summary>
    public event Func<bool> isEffectPhase;

    UnitStatusData playerData = new UnitStatusData();
    List<UnitStatusData> monsterDataList = new List<UnitStatusData>();


    #region phase
    bool rollDiceClicked;

    List<DiceConsequenceData> currentDiceResultList = new List<DiceConsequenceData>();
    int currentRolledDiceIndex = 0;
    bool currentRolledDiceUsed = false;
    #endregion

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

        currentDiceResultList.Clear();

        monsterDataList.Clear();
        for (int i=0; i<2; i++)
        {
            UnitStatusData monster = new UnitStatusData();
            var monsterData = staticDataManager.GetMonster(x => x.index == i);

            monster.hp = monster.maxHp = monsterData.hitpoint;

            monster.deck = CreateMonsterDeck(staticDataManager, monsterData);
            monsterDataList.Add(monster);
        }

        playerData = player.unitData;
        playerData.deck.Clear();
        playerData.deck.AddRange(player.deck);

        isEffectPhase = null;

        inBattle = true;

        unitViewerManager.InitializeMonsterUnits(monsterDataList);
        ingameUIManager.SetPlayer(player)
                       .Init(this);
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
    #region event

    /// <summary>
    /// 전투가 시작 될 때
    /// </summary>
    public IngameLogicManager AddActionOnStartBattle(Action callback)
    {
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
        onEndBattle += callback;
        return this;
    }

    public IngameLogicManager RemoveActionOnEndbattle(Action callback)
    {
        onEndBattle -= callback;
        return this;
    }

    public IngameLogicManager AddActionOnStartMyTurn(Action callback)
    {
        onStartMyTurn += callback;
        return this;
    }

    public IngameLogicManager RemoveActionOnStartMyTurn(Action callback)
    {
        onStartMyTurn -= callback;
        return this;
    }

    public IngameLogicManager AddActionOnRollCompleteMyTurn(Action<List<DiceConsequenceData>> callback)
    {
        onRollCompleteMyTurn += callback;
        return this;
    }

    public IngameLogicManager RemoveActionOnRollCompleteMyTurn(Action<List<DiceConsequenceData>> callback)
    {
        onRollCompleteMyTurn -= callback;
        return this;
    }

    public IngameLogicManager AddActionOnEndMyTurn(Action callback)
    {
        onEndMyTurn += callback;
        return this;
    }


    public IngameLogicManager RemoveActionOnEndMyTurn(Action callback)
    {
        onEndMyTurn -= callback;
        return this;
    }

    /// <param name="callback"> 현재 적용 중인 적의 상태  </param>
    public IngameLogicManager AddActionOnStartEnemyTurn(Action<UnitStatusData> callback)
    {
        onStartEnemyTurn += callback;
        return this;
    }

    public IngameLogicManager RemoveActionOnStartEnemyTurn(Action<UnitStatusData> callback)
    {
        onStartEnemyTurn -= callback;
        return this;
    }

    /// <param name="callback"> 적이 주사위를 돌린 결과 값 </param>
    public IngameLogicManager AddActionOnEndEnemyTurn(Action<UnitStatusData> callback)
    {
        onEndEnemyTurn += callback;
        return this;
    }

    public IngameLogicManager RemoveActionOnEndEnemyTurn(Action<UnitStatusData> callback)
    {
        onEndEnemyTurn -= callback;
        return this;
    }
    #endregion

    #region logic

    bool CheckWait()
    {
        if (isEffectPhase == null)
            return false;
        return isEffectPhase.Invoke();
    }

    IEnumerator RollDiceRoutine()
    {
        yield return null;

        var consequnceList = new List<DiceConsequenceData>();
        playerData.deck.ForEach(deck =>
        {
            consequnceList.Add(new DiceConsequenceData(deck.behaviourDice.GetRandomBehaviourState(),
                                                       deck.actingPowerDice.GetRandomActingPower()));
        });

        currentDiceResultList.Clear();
        currentDiceResultList.AddRange(consequnceList);
        currentRolledDiceIndex = 0;

        onRollCompleteMyTurn?.Invoke(consequnceList);

        // 굴리는 연출 대기
        while (CheckWait())
            yield return null;

        while (currentRolledDiceIndex < currentDiceResultList.Count)
        {
            var result = currentDiceResultList[currentRolledDiceIndex];

            currentRolledDiceUsed = false;

            OnReadyToUseDice(currentRolledDiceIndex);
            while (!currentRolledDiceUsed)
                yield return null;

            while (CheckWait())
                yield return null;

            currentRolledDiceIndex++;
        }
    }

    public void RollMyDice()
    {
        StartCoroutine(RollDiceRoutine());
    }

    public void UseDice(int slotIndex)
    {

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

    public void OnReadyToUseDice(int rolledDiceIndex)
    {
        onReadyToUseDice?.Invoke(rolledDiceIndex);
    }

    public void InvokeOnStartBattle()
    {
        onStartBattle?.Invoke();
    }

    public void InvokeOnStartMyTurn()
    {
        onStartMyTurn?.Invoke();
    }

    public void InvokeOnEndMyTurn(List<DiceConsequenceData> sessionDecks)
    {
        //onEndMyTurn?.Invoke(sessionDecks);
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
