using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPC
{
    private string name;
    public string Name {
        get {
            return this.name;
        }
    }

    private string clientID;
    public string ClientID {
        get {
            return this.clientID;
        }
    }

    private Dictionary<string, object> parameters;
    public Dictionary<string, object> Parameters {
        get {
            return this.parameters;
        }
    }

    private int sequenceID;
    public int SequenceID {
        get {
            return this.sequenceID;
        }
    }

    private long timestamp;
    public long Timestamp {
        get {
            return this.timestamp;
        }
    }

    public static RPC Parse(Dictionary<string, object> response) {
        // LoggerManager.Instance.LogDictionary(response);

        RPC rpc = new RPC(
            response["rpcName"] as string,
            response["clientId"] as string,
            response["parameters"] as Dictionary<string, object>,
            (int)response["sequenceId"],
            (long)response["timestamp"]
        );

        return rpc;
    }

    public RPC(string name, string clientID, Dictionary<string, object> parameters, int sequenceID, long timestamp) {
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
}
