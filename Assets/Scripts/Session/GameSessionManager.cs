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
    [SerializeField] RewardPopup rewardPopup;

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
            GainReward();
            //MoveToNext();
        }
    }

    private void GainReward()
    {
        var dungeonData = StaticDataManager.instance.GetDungeon(x => x.index == player.currentDugeonNodeId);
        if(dungeonData != null)
        {
            int currencyValueReward = 0;
            List<RewardData> rewards = new List<RewardData>();

            List<int> rewardActing = new List<int>(dungeonData.actingPowerDiceRewardList);
            List<int> rewardBehaviour = new List<int>(dungeonData.behaviourDiceRewardList);

            for(int i=0; i<3; i++)
            {
                if (rewardActing.Count == 0 && rewardBehaviour.Count == 0)
                {
                    break;
                }

                if ((UnityEngine.Random.value < 0.5f && rewardActing.Count > 0)
                    || (rewardActing.Count > 0 && rewardBehaviour.Count == 0))
                {
                    var selected = rewardActing[UnityEngine.Random.Range(0, rewardActing.Count)];

                    rewardActing.Remove(selected);

                    if (selected >= 0)
                    {
                        rewards.Add(new RewardData(false, selected));
                    }
                }
                else
                {
                    var selected = rewardBehaviour[UnityEngine.Random.Range(0, rewardActing.Count)];

                    rewardBehaviour.Remove(selected);

                    if (selected >= 0)
                    {
                        rewards.Add(new RewardData(true, selected));
                    }
                }
            }

            currencyValueReward = dungeonData.dungeonClearCurrencyValue;

            PermanentPlayer.instance.currency += currencyValueReward;

            if(rewards.Count > 0)
            {
                rewardPopup.Open(currencyValueReward, rewards, delegate (RewardData reward)
                {
                    if (reward.staticDataIndex != -1)
                    {
                        if (reward.isBehaviour)
                        {
                            var data = StaticDataManager.instance.GetBehaviourDice(x => x.index == reward.staticDataIndex);
                            if (data != null)
                            {
                                player.inventory.behaviourDiceList.Add(data);
                            }
                        }
                        else
                        {
                            var data = StaticDataManager.instance.GetActionPowerDice(x => x.index == reward.staticDataIndex);
                            if (data != null)
                            {
                                player.inventory.actingPowerDiceList.Add(data);
                            }
                        }
                    }

                    MoveToNext();
                });
            }
            else
            {
                MoveToNext();
            }
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