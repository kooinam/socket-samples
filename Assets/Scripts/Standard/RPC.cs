using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPCName {
    public static string JoinGame = "joinGame";
    public static string Move = "move";
}

public class RPC
{
    // rpc name
    private string name;
    public string Name {
        get {
            return this.name;
        }
    }

    // client who fire this RPC
    private string clientID;
    public string ClientID {
        get {
            return this.clientID;
        }
    }

    // rpc parameters
    private Dictionary<string, object> parameters;
    public Dictionary<string, object> Parameters {
        get {
            return this.parameters;
        }
    }

    // rpc offset starting from 0
    private int sequenceID;
    public int SequenceID {
        get {
            return this.sequenceID;
        }
    }

    // rpc execute timestamp
    private double timestamp;
    public double Timestamp {
        get {
            return NetworkManager.Instance.RoundTime(this.timestamp);
        }
    }

    public static RPC Parse(Dictionary<string, object> response) {
        // LoggerManager.Instance.LogDictionary(response);

        string rpcName = response["rpcName"] as string;
        string clientID = response["clientId"] as string;
        Dictionary<string, object> parameters = response["parameters"] as Dictionary<string, object>;
        double sequenceID = (double)response["sequenceId"];
        double timestamp =  (double)response["timestamp"];

        RPC rpc = new RPC(
            rpcName,
            clientID,
            parameters,
            (int)sequenceID,
            timestamp
        );

        return rpc;
    }

    public RPC(string name, string clientID, Dictionary<string, object> parameters, int sequenceID, double timestamp) {
        this.name = name;
        this.clientID = clientID;
        this.parameters = parameters;
        this.sequenceID = sequenceID;
        this.timestamp = timestamp;
    }

    public bool Equals(string name) {
        return this.Name.Equals(name);
    }

    public bool IsFirstPlayer() {
        return NetworkManager.Instance.RoomSocket.Socket.Id.Equals(this.clientID);
    }

    public string ParamStr(string key) {
        if (!this.parameters.ContainsKey(key)) {
            return null;
        }

        return this.parameters[key] as string;
    }

    public int ParamInt(string key, int fallback)
    {
        if (!this.parameters.ContainsKey(key))
        {
            return fallback;
        }

        return System.Convert.ToInt32(this.parameters[key]);
    }
}
