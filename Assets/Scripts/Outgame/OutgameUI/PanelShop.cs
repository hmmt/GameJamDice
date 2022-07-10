using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelShop : PopupBase
{
    [SerializeField] List<UIItemShopDiceViewer> diceViewerList;
    [SerializeField] Text txtCurrency;
    [SerializeField] PopupConfirmation popupConfirmation;

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
                      .SetShopData(shopData)
                      .SetIndex(shopData.index)
                      .SetTextPrice($"{price}")
                      .SetActionOnClickItem(OnClickItem)
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

            void OnClickItem(ShopData shopData, int type, int index)
            {
                var price = 0;
                if (type == 1)
                {
                    behaviourDiceData = StaticDataManager.instance.GetBehaviourDice(data => data.index == index);
                    price = behaviourDiceData.shopPrice;
                }
                else
                {
                    actingPowerDiceData = StaticDataManager.instance.GetActionPowerDice(data => data.index == index);
                    price = actingPowerDiceData.shopPrice;
                }
                if (shopData.hasPurchased)
                {
                    return;
                }
                if (PermanentPlayer.instance.currency < price)
                {
                    return;
                }

                popupConfirmation.SetTextPrice($"{price}")
                                 .SetType(type)
                                 .SetIndex(index)
                                 .SetActionOnClickConfirm((type, index) =>
                                 {
                                     if (type == 1)
                                     {
                                         var behaviourDice = StaticDataManager.instance.GetBehaviourDice(data => data.index == index).ToMemberWiseClone();
                                         PermanentPlayer.instance.AddBehaviourDice(behaviourDice);
                                     }
                                     else
                                     {
                                         var actingPowerDice = StaticDataManager.instance.GetActionPowerDice(data => data.index == index).ToMemberWiseClone();
                                         PermanentPlayer.instance.AddActingPowerDice(actingPowerDice);
                                     }
                                     PermanentPlayer.instance.DecreaseCurrency(price);
                                     shopData.hasPurchased = true;
                                     UpdateInfo();
                                 })
                                 .SetIconViewerList((iconViewerList) =>
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
                                 })
                                 .Open();
            }
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

    public void OnClickClose()
    {
        Close();
    }
}
