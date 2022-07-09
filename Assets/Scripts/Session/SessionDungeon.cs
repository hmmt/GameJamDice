using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �������� ����
/// </summary>
public class SessionDungeon
{
    public List<SessionDungeonNode> nodes = new List<SessionDungeonNode>();


    public void SetToDummy()
    {
        nodes.Clear();

        for(int i=0; i<10; i++)
        {
            if(i < 9)
            {
                nodes.Add(new SessionDungeonNode()
                {
                    id = i,
                    pos = new Vector2(i, 0),
                    nextId = new List<int>() { i + 1 }
                });
            }
            else
            {
                nodes.Add(new SessionDungeonNode()
                {
                    id = i,
                    pos = new Vector2(i, 0),
                    nextId = new List<int>() { }
                });
            }
        }
    }

    public List<int> GetNextNodeIds(int currentNodeId)
    {
        return nodes.Find(x => x.id == currentNodeId)?.nextId ?? new List<int>();
    }
}
