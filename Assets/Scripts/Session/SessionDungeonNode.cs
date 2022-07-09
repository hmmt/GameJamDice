using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionDungeonNode
{
    public int id;

    /// <summary>
    /// 위치. 필요 없을 수도 있음
    /// </summary>
    public Vector2 pos;

    public List<int> nextId = new List<int>();
}
