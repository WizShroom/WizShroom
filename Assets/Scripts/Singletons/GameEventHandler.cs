using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventHandler : Singleton<GameEventHandler>
{
    public void SendEvent(GameObject source, EVENT eventSent)
    {
        OnEventReceived?.Invoke(source, eventSent);
    }

    public delegate void EventHandler(GameObject source, EVENT eventSent);
    public event EventHandler OnEventReceived;
}

public enum EVENT
{
    RESUMED,
    PAUSED,
    LOADINGLEVEL,
    LOADEDLEVEL,
}
