using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameLogicManager : MonoBehaviour
{
    public static IngameLogicManager instance { get; private set; }

    [SerializeField] SessionPlayer sessionPlayer;
    [SerializeField] IngameUIManager ingameUIManager;

    event Action onStartBattle;
    event Action onEndBattle;
    event Action<List<DiceConsequenceData>> onStartMyTurn;
    event Action<List<DiceConsequenceData>> onEndMyTurn;
    event Action<List<DiceConsequenceData>> onStartEnemyTurn;
    event Action<List<DiceConsequenceData>> onEndEnemyTurn;

    private void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        ingameUIManager.SetPlayer(sessionPlayer)
                       .Init(this);
    }

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

    /// <param name="callback"> 현재 적용 중인 내 상태 </param>
    public IngameLogicManager AddActionOnStartMyTurn(Action<List<DiceConsequenceData>> callback)
    {
        onStartMyTurn += callback;
        return this;
    }

    public IngameLogicManager RemoveActionOnStartMyTurn(Action<List<DiceConsequenceData>> callback)
    {
        onStartMyTurn -= callback;
        return this;
    }

    /// <param name="callback"> 내가 주사위를 돌린 결과 값 </param>
    public IngameLogicManager AddActionOnEndMyTurn(Action<List<DiceConsequenceData>> callback)
    {
        onEndEnemyTurn += callback;
        return this;
    }


    public IngameLogicManager RemoveActionOnEndMyTurn(Action<List<DiceConsequenceData>> callback)
    {
        onEndMyTurn -= callback;
        return this;
    }

    /// <param name="callback"> 현재 적용 중인 적의 상태  </param>
    public IngameLogicManager AddActionOnStartEnemyTurn(Action<List<DiceConsequenceData>> callback)
    {
        onStartEnemyTurn += callback;
        return this;
    }

    public IngameLogicManager RemoveActionOnStartEnemyTurn(Action<List<DiceConsequenceData>> callback)
    {
        onStartEnemyTurn -= callback;
        return this;
    }

    /// <param name="callback"> 적이 주사위를 돌린 결과 값 </param>
    public IngameLogicManager AddActionOnEndEnemyTurn(Action<List<DiceConsequenceData>> callback)
    {
        onEndEnemyTurn += callback;
        return this;
    }

    public IngameLogicManager RemoveActionOnEndEnemyTurn(Action<List<DiceConsequenceData>> callback)
    {
        onEndEnemyTurn -= callback;
        return this;
    }

    public void InvokeOnStartBattle()
    {
        onStartBattle?.Invoke();
    }

    public void InvokeOnEndBattle()
    {
        onEndBattle?.Invoke();
    }

    public void InvokeOnStartMyTurn(List<DiceConsequenceData> sessionDecks)
    {
        onStartMyTurn?.Invoke(sessionDecks);
    }

    public void InvokeOnEndMyTurn(List<DiceConsequenceData> sessionDecks)
    {
        onEndMyTurn?.Invoke(sessionDecks);
    }

    public void InvokeOnStartEnemyTurn(List<DiceConsequenceData> sessionDecks)
    {
        onStartEnemyTurn?.Invoke(sessionDecks);
    }

    public void InvokeOnEndEnemyTurn(List<DiceConsequenceData> sessionDecks)
    {
        onEndEnemyTurn?.Invoke(sessionDecks);
    }
}
