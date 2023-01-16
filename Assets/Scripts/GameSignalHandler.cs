using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSignalHandler
{
    private static GameSignalHandler _instance;

    public static GameSignalHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameSignalHandler();
            }

            return _instance;
        }
    }

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
