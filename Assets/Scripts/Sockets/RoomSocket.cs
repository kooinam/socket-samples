using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomSocket : BaseSocket
{
    public RoomSocket() : base(NetworkManager.Instance.SocketManager, "rooms") {
        // do nothing
    }

    public SocketAction CreateRoom(string roomID)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["roomId"] = roomID;

        SocketAction action = this.action("createRoom", parameters);

        return action;
    }

    public SocketAction JoinGame(string playerName) {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["playerName"] = "playerName";

        SocketAction action = this.rpcAction("joinGame", parameters);

        return action;
    }
}
