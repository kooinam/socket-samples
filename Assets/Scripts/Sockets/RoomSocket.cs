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
        NetworkManager.Instance.RoundTime((double)response["timestamp"]);
        this.timestamp = (double)response["timestamp"] - Time.fixedTime;
    }

    public long GetTime() {
        if (this.roomID == null) {
            return  0;
        }

        return NetworkManager.Instance.RoundTime(Time.fixedTime + this.timestamp);
    }

    public RPC PopRPC()
    {
        if (this.rpcs.Count == 0)
        {
            return null;
        }

        RPC rpc = this.rpcs[0];

        if (rpc.Timestamp > this.GetTime())
        {
            return null;
        }

        if ((rpc.Timestamp - this.GetTime()) >= 0.05f)
        {
            throw new System.Exception(string.Format("RPC out of sync - {0}:{1}", this.GetTime(), rpc.Timestamp));
        }

        this.rpcs.RemoveAt(0);

        return rpc;
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
