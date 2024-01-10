using System.Collections.Generic;
using UnityEngine;

public class Debug
{
    private string _log01 = "";
    private string _log02 = "";
    private string _log03 = "";
    private string _log04 = "";
    private string _log05 = "";

    public void OnGUI()
    {
        #if UNITY_EDITOR
        List<string> logs = new()
        {
            _log01,
            _log02,
            _log03,
            _log04,
            _log05,
        };

        int index = -1;
        foreach (var log in logs)
        {
            index += 1;
            GUI.Label(new Rect(16, 16 + (index * 28), 256, 24), log);
        }
        #endif
    }

    public void Log01(string value)
    {
        _log01 = value;
    }

    public void Log02(string value)
    {
        _log02 = value;
    }

    public void Log03(string value)
    {
        _log03 = value;
    }

    public void Log04(string value)
    {
        _log04 = value;
    }

    public void Log05(string value)
    {
        _log05 = value;
    }
}
