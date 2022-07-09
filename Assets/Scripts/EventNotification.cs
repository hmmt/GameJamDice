using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventNotification
{
    public static event Action OnInitializeBattle;
    public static event Action OnClearBattle;
}
