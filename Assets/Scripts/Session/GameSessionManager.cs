using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �����ϴ� ��
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

        // ���� �� �� �� ui ����

        unitManager.InitializePlayerUnit(player.unitData);
        stageLocationShower.SetCurrentStage(0)
                           .Init();

        StartCoroutine(MoveToRoutine(0));
    }

    void MoveToNext()
    {
        // ���� ��� �̵�
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
        // IngameUIManager�� StartPopup()�� ���ð����� ���� ��
        yield return new WaitForSeconds(2f);
        unitManager.ClearMonsters();

        if(player.unitData.isDead)
        {
            // ���� ����
            //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        else
        {
            MoveToNext();
        }
    }
    private void ClearDungeon()
    {
        Debug.Log("Ŭ����!");


    }
}