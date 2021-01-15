using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomSocket : BaseSocket
{
    private string roomID = null;
    public string RoomID {
        get {
            return this.roomID;
        }
    }

    private string hostID = null;
    public string HostID {
        get {
            return this.hostID;
        }
    }

    private double timestamp = 0;
    public double Timestamp {
        get {
            return this.timestamp;
        }
    }

    public RoomSocket() : base(NetworkManager.Instance.SocketManager, "rooms") {
        // do nothing
    }

    public void Configure(Dictionary<string, object> response)
    {
        this.roomID = response["roomId"] as string;
        this.hostID = response["hostId"] as string;
        this.timestamp = (double)response["timestamp"];
    }

    public double GetTime() {
        return Time.time - this.timestamp;
    }

    public SocketAction CreateRoom(string roomID)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["roomId"] = roomID;

        SocketAction action = this.action("createRoom", parameters);

        return action;
    }

    public SocketAction JoinRoom(string roomID)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["roomId"] = roomID;

        SocketAction action = this.action("joinRoom", parameters);

        return action;
    }

    public SocketAction JoinGame(string playerName) {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["playerName"] = playerName;

        SocketAction action = this.rpcAction(RPCName.JoinGame, parameters);

        return action;
    }

    public SocketAction Move(int direction) {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["direction"] = direction;

        SocketAction action = this.rpcAction(RPCName.Move, parameters);

        return action;
    }
}
