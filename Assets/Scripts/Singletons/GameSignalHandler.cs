using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSignalHandler : Singleton<GameSignalHandler>
{
    public void SendSignal(GameObject source, string signal)
    {
        if (signal == "")
        {
            return;
        }
        OnSignalReceived?.Invoke(source, signal);
    }

    public delegate void SignalHandler(GameObject source, string signal);
    public event SignalHandler OnSignalReceived;
}
