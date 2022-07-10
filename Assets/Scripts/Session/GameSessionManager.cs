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
    [SerializeField] HUDStageLocationShower stageLocationShower;

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
        player.unitData.hp = player.unitData.maxHp = 100;

        // 대충 맵 뷰 및 ui 세팅

        unitManager.InitializePlayerUnit(player.unitData);
        stageLocationShower.SetCurrentStage(0)
                           .Init();

        StartCoroutine(MoveToRoutine(0));
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
        yield return null;
        unitManager.SetToMoveState();
        fieldMap.SetToMoveState();

        stageLocationShower.SetCurrentStage(player.currentDugeonNodeId)
                           .MoveToTargetStage(nextNode, 2f);
        yield return CommonUtility.GetYieldSec(2f);


        unitManager.SetToIdleState();
        fieldMap.SetToStopState();

        player.currentDugeonNodeId = nextNode;

        EnterBattle();
    }

    private void EnterBattle()
    {
        battleLogic.AddActionOnEndBattle(OnEndBattle);
        battleLogic.Init(player, player.currentDugeonNodeId);
    }

    private void OnEndBattle(bool win)
    {
        battleLogic.RemoveActionOnEndbattle(OnEndBattle);

        StartCoroutine(WaitAfterClear());
    }

    IEnumerator WaitAfterClear()
    {
        if(player.unitData.isDead)
        {

        }
        // IngameUIManager의 StartPopup()의 대기시간보다 길어야 함
        yield return new WaitForSeconds(2f);
        unitManager.ClearMonsters();

        if(player.unitData.isDead)
        {
            // 게임 오버
            //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        else
        {
            MoveToNext();
        }
    }
    private void ClearDungeon()
    {
        Debug.Log("클리어!");


    }
}