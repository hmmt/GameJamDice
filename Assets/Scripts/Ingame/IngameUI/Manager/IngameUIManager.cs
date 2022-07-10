using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIManager : MonoBehaviour
{
    public enum Turn
    {
        None,
        MyTurn,
        OpponentTurn,
    }


    SessionPlayer player;

    [Header("UI inspector")]
    [SerializeField] UIButton rollButton;
    [SerializeField] IngameRolledInfo[] rolledInfos;
    [SerializeField] DiceAnimatator diceAnimator;

    [Header("turn alram")]
    [SerializeField] GameObject alaramRoot;
    [SerializeField] GameObject myTurnImage;
    [SerializeField] GameObject opponentTurnImage;
    [SerializeField] GameObject winImage;


    [SerializeField] GameObject losePopup;
    [SerializeField] IngamePanelInventory ingamePanelLaboratory;


    private Turn _lastTurn = Turn.None;

    #region selection
    private int _selectedIndex = -1;
    #endregion

    public bool waiting { private set; get; }

    private void Awake()
    {
        foreach (var rolledInfo in rolledInfos)
        {
            rolledInfo.SetToEmpty();
        }
    }

    public void OnClickPanelLaboratory()
    {
        ingamePanelLaboratory.UpdateDiceSummaryList(SessionPlayer.instance.deck)
                             .UpdateInventory(SessionPlayer.instance.inventory.behaviourDiceList, SessionPlayer.instance.inventory.actingPowerDiceList)
                             .Open();
    }

    public void Init(IngameLogicManager ingameLogicManager)
    {
        losePopup.SetActive(false);
        waiting = false;
        _selectedIndex = -1;
        if (rollButton != null)
            rollButton.gameObject.SetActive(false);

        foreach(var rolledInfo in rolledInfos)
        {
            rolledInfo.SetToEmpty();
        }

        ingameLogicManager.AddActionOnStartBattle(OnStartBattle);    // ���� ���۽� ���� ���� ��� ���� ���� �۾�
        ingameLogicManager.AddActionOnEndBattle(OnEndBattle);      // ���� ����� ���� �˾� ���� ���� �۾�


        ingameLogicManager.AddActionOnStartTurn(OnStartTurn);    // ���� ���۵� �� �� ���� Ÿ��Ʋ�� ���� ���� �۾�
        ingameLogicManager.AddActionOnRollCompleteTurn(OnRollCompleteTurn); // �ֻ��� ������ ���� �۾�
        ingameLogicManager.AddActionOnUseDice(OnUseDice);
        ingameLogicManager.AddActionOnEndTurn(OnEndTurn); // � �ֻ��� ����� ���Դ��� �˾� ���� ���� �۾�

        ingameLogicManager.AddActionOnSelectDiceSlot(OnSelectDice);

        //ingameLogicManager.AddActionOnEndMyTurn();      // �� �Ͽ��� � �ֻ��� ����� ���Դ��� �˾� ���� ���� �۾�
        //ingameLogicManager.AddActionOnStartEnemyTurn(); // �� ���� ���۵� �� �� �� ���� Ÿ��Ʋ�� ���� ���� �۾�
        //ingameLogicManager.AddActionOnEndEnemyTurn();   // �� �Ͽ��� � �ֻ��� ����� ���Դ��� �˾� ���� ���� �۾�
    }

    public void Clear()
    {

    }

    public IngameUIManager SetPlayer(SessionPlayer player)
    {
        this.player = player;
        return this;
    }

    IEnumerator ShowRollDice(DiceConsequenceData dcdata, int diceIndex)
    {
        // ���� ������ ����
        waiting = true;
        yield return null;

        diceAnimator.PlayAnimation(dcdata.behaviourState, dcdata.actingPower, diceIndex, delegate ()
        {
            waiting = false;
            try
            {
                rolledInfos[diceIndex].SetDice(dcdata,
                    delegate ()
                    {
                        IngameLogicManager.instance.SelectDiceSlot(diceIndex);
                    });

            }
            catch (Exception e)
            {

            }
        });

    }

#region callback
    private void OnStartBattle()
    {
        _lastTurn = Turn.None;
    }
    private void OnStartTurn(UnitStatusData unit)
    {
        if (rollButton != null)
            rollButton.gameObject.SetActive(true);

        for (int i = 0; i < rolledInfos.Length; i++)
        {
            rolledInfos[i].SetToEmpty();
        }
        Turn turn = unit.isPlayer ? Turn.MyTurn : Turn.OpponentTurn;

        if (turn != _lastTurn)
        {
            StartCoroutine(StartPopup(turn == Turn.MyTurn ? myTurnImage : opponentTurnImage));
        }

        _lastTurn = turn;
    }
    IEnumerator StartPopup(GameObject targetPopupImg)
    {
        myTurnImage.SetActive(false);
        opponentTurnImage.SetActive(false);
        winImage.SetActive(false);

        alaramRoot.SetActive(true);
        targetPopupImg.SetActive(true);

        waiting = true;
        // GameSessionManager�� WaitAfterClear ���ð����� ª�ƾ� ��
        yield return new WaitForSeconds(1.8f);

        waiting = false;
        alaramRoot.SetActive(false);
    }

    private void OnEndBattle(bool b)
    {
        if(b)
        {
            StartCoroutine(StartPopup(winImage));
        }
        else
        {
            losePopup.SetActive(true);
        }
    }
    private void OnRollCompleteTurn(UnitStatusData unit, DiceConsequenceData dcdata, int diceIndex)
    {
        StartCoroutine(ShowRollDice(dcdata, diceIndex));
    }

    private void OnSelectDice(int diceIndex)
    {
        foreach(var r in rolledInfos)
        {
            r.SetSelected(false);
        }

        if(diceIndex >= 0 && diceIndex < rolledInfos.Length)
        {
            rolledInfos[diceIndex].SetSelected(true);
        }
    }
    private void OnUseDice(UnitStatusData unit, DiceConsequenceData diceData, int diceIndex, List<ActionResultData> resultList)
    {
        foreach (var r in rolledInfos)
        {
            r.SetSelected(false);
        }
        try
        {
            rolledInfos[diceIndex].SetToUsed();
        }
        catch(Exception e)
        {

        }
    }
    private void OnEndTurn(UnitStatusData unit)
    {
        //rollButton.gameObject.SetActive(true);
    }
#endregion


#region event trigger
    public void OnClickRollTheDice()
    {
        IngameLogicManager.instance.RollMyDice();
    }

    public void OnClickTestButton_battleSkip()
    {
    }

    public void OnClickGotoTitle()
    {
        losePopup.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

#endregion
}
