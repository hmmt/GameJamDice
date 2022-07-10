using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopData
{
    public int type;
    public int index;
    public bool hasPurchased;

    /// <param name="type"> 1 == �ൿ, 0 == �ൿ�� </param>
    public ShopData(int type, int index)
    {
        this.type = type;
        this.index = index;
        hasPurchased = false;
    }
}

/// <summary>
/// �� ���� �������� �����Ǵ� ����
/// </summary>
public class PermanentPlayer
{
    private static PermanentPlayer _instance;
    public static PermanentPlayer instance => _instance ??= new PermanentPlayer();

    public StartInventory startInventory = new StartInventory();

    public int currency;
    public List<ShopData> shopDataList = new List<ShopData>();

    public bool initialized = false;
    public void InitializePlayer()
    {
        // ������ ó�� ������ �� �ʱ�ȭ
        initialized = true;

        // �ӽ÷� FindObjectOfType ��
        StaticDataManager staticDataManager = Object.FindObjectOfType<StaticDataManager>();

        //startInventory.Clear();
        var defaultBehaviourDice = staticDataManager.GetBehaviourDice(x => x.isDefaultDice);
        var actingPowerDice = staticDataManager.GetActionPowerDice(x => x.isDefaultDice);


        startInventory.currentDeckList.Add(new SessionDeck().SetBehaviourDice(defaultBehaviourDice)
                                                            .SetActingPowerDice(actingPowerDice));
        startInventory.currentDeckList.Add(new SessionDeck().SetBehaviourDice(defaultBehaviourDice)
                                                            .SetActingPowerDice(actingPowerDice));
        startInventory.currentDeckList.Add(new SessionDeck().SetBehaviourDice(defaultBehaviourDice)
                                                            .SetActingPowerDice(actingPowerDice));

        currency = 200;
    }

    public void ResetShopData()
    {
        shopDataList.Clear();

        shopDataList.Add(GenerateShopData());
        shopDataList.Add(GenerateShopData());
        shopDataList.Add(GenerateShopData());

        ShopData GenerateShopData()
        {
            var type = 0;
            var index = 0;
            if (Random.value >= 0.5f)
            {
                var avaliableProducts = StaticDataManager.instance.GetS3Data<StaticS3BehaviourDiceData>().datas.Where(data => data.shopPrice > 0).ToList();
                avaliableProducts.Shuffle();
                type = 1;
                index = avaliableProducts.FirstOrDefault().index;
            }
            else
            {
                var avaliableProducts = StaticDataManager.instance.GetS3Data<StaticS3ActingPowerDiceData>().datas.Where(data => data.shopPrice > 0).ToList();
                avaliableProducts.Shuffle();
                type = 0;
                index = avaliableProducts.FirstOrDefault().index;
            }
            return new ShopData(type, index);
        }
    }


    public void AddBehaviourDice(S3BehaviourDiceData behaviourDiceData)
    {
        startInventory.behaviourDiceList.Add(behaviourDiceData);
    }

    public void AddActingPowerDice(S3ActingPowerDiceData actingPowerDiceData)
    {
        startInventory.actingPowerDiceList.Add(actingPowerDiceData);
    }

    public void RemoveBehaviourDice(S3BehaviourDiceData behaviourDiceData)
    {
        startInventory.behaviourDiceList.Remove(behaviourDiceData);
    }

    public void RemoveActingPowerDice(S3ActingPowerDiceData actingPowerDice)
    {
        startInventory.actingPowerDiceList.Remove(actingPowerDice);
    }

    public void IncreaseCurrency(int currency)
    {
        this.currency += currency;
    }

    public void DecreaseCurrency(int currency)
    {
        this.currency -= currency;
    }

    public void SetCurrency(int currency)
    {
        this.currency = currency;
    }
}
