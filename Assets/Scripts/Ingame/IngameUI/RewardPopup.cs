using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardPopup : MonoBehaviour
{

    [SerializeField] RewardPopupDice[] rewardsSlots;
    [SerializeField] RewardPopupDice confirmDice;
    [SerializeField] GameObject windowRoot;
    [SerializeField] GameObject confirmWindowRoot;

    private List<RewardData> _rewards = new List<RewardData>();
    private int currency;

    private System.Action<RewardData> _callback;



    private int _selectedIndex;

    public void Open(int gainCurrency, List<RewardData> rewards, System.Action<RewardData> onClose)
    {
        windowRoot.SetActive(true);
        confirmWindowRoot.SetActive(false);
        gameObject.SetActive(true);

        currency = gainCurrency;
        _rewards.Clear();
        _rewards.AddRange(rewards);

        _callback = onClose;

        for(int i=0; i < rewardsSlots.Length; i++)
        {
            if (i < _rewards.Count)
            {
                rewardsSlots[i].gameObject.SetActive(true);

                if(_rewards[i].isBehaviour)
                {
                    var data = StaticDataManager.instance.GetBehaviourDice(x => x.index == _rewards[i].staticDataIndex);
                    if(data != null)
                    {
                        rewardsSlots[i].SetData(data);
                    }
                    else
                    {
                        rewardsSlots[i].gameObject.SetActive(false);
                    }
                }
                else
                {
                    var data = StaticDataManager.instance.GetActionPowerDice(x => x.index == _rewards[i].staticDataIndex);
                    if (data != null)
                    {
                        rewardsSlots[i].SetData(data);
                    }
                    else
                    {
                        rewardsSlots[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                rewardsSlots[i].gameObject.SetActive(false);
            }
            //rewardIcons[i].sprite = 
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void OnClickReward(int index)
    {
        if (index < 0 || index >= _rewards.Count)
            return;

        if (_rewards[index].isBehaviour)
        {
            var data = StaticDataManager.instance.GetBehaviourDice(x => x.index == _rewards[index].staticDataIndex);
            if (data != null)
            {
                confirmDice.SetData(data);
            }
        }
        else
        {
            var data = StaticDataManager.instance.GetActionPowerDice(x => x.index == _rewards[index].staticDataIndex);
            if (data != null)
            {
                confirmDice.SetData(data);
            }
        }

        _selectedIndex = index;

        windowRoot.SetActive(false);
        confirmWindowRoot.SetActive(true);
    }

    public void OnClickOk()
    {
        _callback?.Invoke(_rewards[_selectedIndex]);
        Close();
    }

    public void OnClickCancel()
    {
        windowRoot.SetActive(true);
        confirmWindowRoot.SetActive(false);
    }
}
