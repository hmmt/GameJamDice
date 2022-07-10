using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticDataManager : MonoBehaviour
{
    public static StaticDataManager instance { get; private set; }

    [SerializeField] List<StaticS3DataBase> staticDataList;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public S3BehaviourDiceData GetBehaviourDice(Func<S3BehaviourDiceData, bool> predicate)
        => GetS3Data<StaticS3BehaviourDiceData>().datas.Find((data) => predicate(data));
    public S3ActingPowerDiceData GetActionPowerDice(Func<S3ActingPowerDiceData, bool> predicate)
        => GetS3Data<StaticS3ActingPowerDiceData>().datas.Find((data) => predicate(data));


    public S3MonsterData GetMonster(Func<S3MonsterData, bool> predicate)
        => GetS3Data<StaticS3MonsterData>().datas.Find((data) => predicate(data));

    public S3DungeonData GetDungeon(Func<S3DungeonData, bool> predicate)
        => GetS3Data<StaticS3DungeonData>().datas.Find((data) => predicate(data));

    public T GetS3Data<T>() where T : class
        => staticDataList.FirstOrDefault(data => data is T) as T;
}
