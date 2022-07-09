using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StaticS3DataBase<T> : StaticS3DataBase
    where T : S3DataBase, new()
{
    [SerializeField] protected string textDataPath;
    [ReadOnly] public List<T> datas;

    [Button]
    public override void LoadTextData()
    {
        var textAsset = Resources.Load<TextAsset>($"S3Datas/{textDataPath}");
        var lines = textAsset.text.Split('\n');

        datas = new List<T>();
        var count = lines.Length;
        for (int i = 0; i < count; i++)
        {
            var data = new T();
            var targetData = lines[i].Split('\t');
            data.InitData(targetData);
            datas.Add(data);
        }
    }
}

public abstract class StaticS3DataBase : SerializedScriptableObject 
{
    public abstract void LoadTextData();
}
