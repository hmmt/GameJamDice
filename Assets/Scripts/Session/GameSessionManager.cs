using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 진행하는 곳
/// </summary>
public class GameSessionManager : MonoBehaviour
{
    public static GameSessionManager instance;

    [SerializeField] IngameLogicManager battleLogic;
    [SerializeField] UnitViewerManager unitManager;
    [SerializeField] FieldMapViewer fieldMap;

    SessionPlayer player;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        player = SessionPlayer.instance;

        Initialize();
    }



    void Initialize()
    {
        // 대충 맵 뷰 및 ui 세팅
        StartCoroutine(MoveToRoutine(1));
    }

    void MoveToNext()
    {
        // 대충 노드 이동

        var nexts = player.currentDungeon.GetNextNodeIds(player.currentDugeonNodeId);
        if (nexts.Count > 0)
        {
            StartCoroutine(MoveToRoutine(nexts[0]));
        }
        else
        {
            ClearDungeon();
        }
    }

    IEnumerator MoveToRoutine(int nextNode)
    {
        unitManager.SetToMoveState();
        fieldMap.SetToMoveState();
        yield return new WaitForSeconds(4);

        unitManager.SetToIdleState();
        fieldMap.SetToStopState();

        player.currentDugeonNodeId = nextNode;

        EnterBattle();
    }

    private void EnterBattle()
    {
        battleLogic.AddActionOnEndBattle(OnEndBattle);
        battleLogic.Init(player, 0);
    }

    private void OnEndBattle()
    {
        battleLogic.RemoveActionOnEndbattle(OnEndBattle);

        StartCoroutine(WaitAfterClear());
    }

    IEnumerator WaitAfterClear()
    {
        yield return new WaitForSeconds(0.25f);
        MoveToNext();
    }
    private void ClearDungeon()
    {

    }
}