using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO;
using UnityEngine.Events;

public class BaseSocket
{
    protected SocketManager socketManager = null;

    protected string nsp = null;
    public string Nsp {
        get {
            return this.nsp;
        }
    }

    protected Socket socket = null;
    public Socket Socket {
        get {
            return this.socket;
        }
    }

    protected List<RPC> rpcs = null;

    public BaseSocket(SocketManager socketManager, string nsp)
    {
        this.nsp = nsp;
        this.socketManager = socketManager;
        this.socket = this.socketManager.GetSocket(string.Format("/{0}", this.nsp));
        this.rpcs = new List<RPC>();

        this.socket.On(SocketIOEventTypes.Connect, (Socket _socket, Packet packet, object[] args) =>
        {
            Debug.LogFormat("connected {0}", this.Socket.Id);
        });

        this.socket.On(SocketIOEventTypes.Disconnect, (Socket _socket, Packet packet, object[] args) =>
        {
            Debug.Log("disconnected");
        });

        this.socket.On(SocketIOEventTypes.Error, (Socket _socket, Packet packet, object[] args) =>
        {
            Error error = args[0] as Error;
            Debug.Log(error.ToString());
        });

        this.socket.On("error", (Socket _socket, Packet packet, object[] args) =>
        {
            Debug.Log("test reconnecting...");
        });
    }

    public void Connect()
    {
        this.socketManager.Open();
    }

    public void Disconnect()
    {
        this.socketManager.Close();
    }

    public void SubscribeRPC()
    {
        this.on(
            "onReceivedRPC",
            (Dictionary<string, object> response) =>
            {
                RPC rpc = RPC.Parse(response);

                this.rpcs.Add(rpc);
            }
        );
    }

    public RPC PopRPC() {
        if (this.rpcs.Count == 0) {
            return null;
        }

        RPC rpc = this.rpcs[0];
        this.rpcs.RemoveAt(0);

        return rpc;
    }

    protected SocketAction action(string actionName, Dictionary<string, object> parameters)
    {
        string payload = DictionarySerializer.ToJSON(parameters);

        SocketAction action = new SocketAction(this, actionName, payload);

        return action;
    }

    protected SocketAction rpcAction(string rpcName, Dictionary<string, object> parameters) {
        Dictionary<string, object> rpcParameters = new Dictionary<string, object>();
        rpcParameters["rpcName"] = rpcName;
        rpcParameters["parameters"] = parameters;

        string payload = DictionarySerializer.ToJSON(rpcParameters);

        SocketAction action = new SocketAction(this, "rpc", payload);

        return action;
    }

    protected void on(string eventName, UnityAction<Dictionary<string, object>> onReceived)
    {
        BestHTTP.SocketIO.Events.SocketIOCallback callback = (Socket _socket, Packet packet, object[] args) =>
        {
            Dictionary<string, object> response = args[0] as Dictionary<string, object>;

            string raw = DictionarySerializer.ToJSON(response);
            LoggerManager.Instance.Info(raw);

            onReceived(response);
        };

        this.Socket.On(eventName, callback);
    }
}
