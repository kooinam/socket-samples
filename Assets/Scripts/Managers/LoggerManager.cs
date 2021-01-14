using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoggerManager : Singleton<LoggerManager>
{
    private static string logs = null;

    [SerializeField]
    private bool logInfo = false;

    protected void Start()
    {
        if (Debug.isDebugBuild)
        {
            string fileName = "Logs/logs.html";
            File.WriteAllText(fileName, "");
        }
    }

    public void Info(string log)
    {
        if (this.logInfo)
        {
            Debug.Log(log);
        }

        // EditorPrefs.SetString("socket_logs", log);
        if (Debug.isDebugBuild)
        {
            string fileName = "Logs/logs.html";
            logs = string.Format("<div><b>{0}</b><div><div>{1}</div><hr />", System.DateTime.Now.ToString(), log) + logs;

            File.WriteAllText(fileName, logs);
        }
    }

    public void LogDictionary(Dictionary<string, object> dict) {
        foreach (KeyValuePair<string, object> kvp in dict) {
            Debug.LogFormat("{0}, {1}",  kvp.Key, kvp.Value);
        }
    }
}
