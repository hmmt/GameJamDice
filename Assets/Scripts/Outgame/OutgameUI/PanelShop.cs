using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelShop : PopupBase
{
    [SerializeField] List<UIItemShopDiceViewer> diceViewerList;
    [SerializeField] Text txtCurrency;

    public void UpdateInfo()
    {
        txtCurrency.SetText($"{PermanentPlayer.instance.currency}");
        var shopDataList = PermanentPlayer.instance.shopDataList;
        var count = shopDataList.Count;

        for (int i = 0; i < count; i++)
        {
            var shopData = shopDataList[i];
            var diceViewer = diceViewerList[i];

            S3BehaviourDiceData behaviourDiceData = null;
            S3ActingPowerDiceData actingPowerDiceData = null;
            var price = 0;

            if (shopData.type == 1)
            {
                behaviourDiceData = StaticDataManager.instance.GetBehaviourDice(data => data.index == shopData.index);
                price = behaviourDiceData.shopPrice;
            }
            else
            {
                actingPowerDiceData = StaticDataManager.instance.GetActionPowerDice(data => data.index == shopData.index);
                price = actingPowerDiceData.shopPrice;
            }

            diceViewer.SetDimed(shopData.hasPurchased)
                      .SetType(shopData.type)
                      .SetIndex(shopData.index)
                      .SetTextPrice($"{price}")
                      .SetIconViewer((iconViewerList) =>
                      {
                          if (behaviourDiceData != null)
                          {
                              var list = behaviourDiceData.BehavioursToList();
                              for (int i = 0; i < 6; i++)
                              {
                                  var behaviourState = list[i];
                                  var iconViewer = iconViewerList[i];
                                  var sprite = SpriteManager.instance.GetBevaiourIconSprite((int)behaviourState);
                                  iconViewer.SetSpriteIcon(sprite);
                              }
                          }
                          else
                          {
                              var list = actingPowerDiceData.ActingPowerToList();
                              for (int i = 0; i < 6; i++)
                              {
                                  var actingPower = list[i];
                                  var iconViewer = iconViewerList[i];
                                  var sprite = SpriteManager.instance.GetActingPowerIconSprite(actingPower);
                                  iconViewer.SetSpriteIcon(sprite);
                              }
                          }
                      });
        }
    }

    public override void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
        UpdateInfo();
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
