using UnityEngine;
using System.Collections.Generic;

namespace Daze.Log
{
    public class DebugLogDisplay : MonoBehaviour
    {
        private static Dictionary<string, string> _messages = new();
        private GUIStyle _style;

        public void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        public void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        public void Start()
        {
            // Initialize GUIStyle
            _style = new GUIStyle();
            _style.fontSize = 14;
            _style.normal.textColor = Color.red;
        }

        private void HandleLog(string message, string _stackTrace, LogType _type)
        {
            string[] kv = message.Split(':');
            _messages[kv[0]] = kv[1].Trim();
        }

        public void OnGUI()
        {
            GUILayout.BeginArea(new Rect(16, 16, Screen.width - 16, Screen.height - 16));
            foreach (var message in _messages)
            {
                GUILayout.Label($"{message.Key}: {message.Value}", _style);
            }
            GUILayout.EndArea();
        }
    }
}
