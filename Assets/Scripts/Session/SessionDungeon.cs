using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 맵의 전반적인 정보
/// </summary>
public class SessionDungeon
{
    public List<SessionDungeonNode> nodes = new List<SessionDungeonNode>();


    public void SetToDummy()
    {
        nodes.Clear();

        for(int i=0; i<5; i++)
        {
            nodes.Add(new SessionDungeonNode() {
                id = i + 1,
                pos = new Vector2(i, 0),
                nextId = new List<int>() { i + 2 },
                eventTypeIndex = 0,
                level = i + 1
            });
        }
    }

    public List<int> GetNextNodeIds(int currentNodeId)
    {
        return nodes.Find(x => x.id == currentNodeId)?.nextId ?? new List<int>();
    }
}
