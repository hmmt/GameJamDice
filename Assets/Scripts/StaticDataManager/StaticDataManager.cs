using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticDataManager : MonoBehaviour
{
    [SerializeField] List<S3DataBase> staticDataList;

    public S3BehaviourDice GetBehaviourDice(Func<S3BehaviourDice, bool> predicate)
        => GetS3Data<StaticS3BehaviourDice>().datas.Find((data) => predicate(data));

    public T GetS3Data<T>() where T : class
    => staticDataList.FirstOrDefault(data => data is T) as T;
}
