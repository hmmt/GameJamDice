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

    /// <summary>
    /// 어떤 이벤트인지(전투 or 기타 등등)
    /// </summary>
    public int eventTypeIndex;

    public int level;
}
