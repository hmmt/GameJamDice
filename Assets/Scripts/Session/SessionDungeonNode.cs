using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionDungeonNode
{
    public int id;

    /// <summary>
    /// ��ġ. �ʿ� ���� ���� ����
    /// </summary>
    public Vector2 pos;

    public List<int> nextId = new List<int>();
}
