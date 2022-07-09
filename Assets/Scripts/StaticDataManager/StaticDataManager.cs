using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticDataManager : MonoBehaviour
{
    [SerializeField] List<StaticS3DataBase> staticDataList;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public S3BehaviourDiceData GetBehaviourDice(Func<S3BehaviourDiceData, bool> predicate)
        => GetS3Data<StaticS3BehaviourDiceData>().datas.Find((data) => predicate(data));

    public T GetS3Data<T>() where T : class
        => staticDataList.FirstOrDefault(data => data is T) as T;
}
