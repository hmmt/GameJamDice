using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 로그라이크 한 판 하는 동안 유지되는 정보
/// </summary>
public class SessionPlayer
{
    SessionDeck deck;
    SessionDungeon currentDungeon;
    SessionInventory inventory;

    /// <summary>
    /// 몇번째 던전인지
    /// </summary>
    int dungeonStageLevel;
}
