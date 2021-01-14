using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO;
using UnityEngine.Events;

public class NetworkManager : Singleton<NetworkManager>
{
    private SocketManager socketManager = null;
    public SocketManager SocketManager {
        get {
            return this.socketManager;
        }
    }

    private RoomSocket roomSocket= null;
    public RoomSocket RoomSocket {
        get {
            return this.roomSocket;
        }
    }

    [SerializeField]
    private string serverEndpoint = "http://127.0.0.1:8000";
    // private string serverEndpoint = "http://52.221.225.62:8000";

    protected override void Awake()
    {
        base.Awake();

        this.Setup();
    }

    public void Setup()
    {
        Debug.Log("Setting up Network Manager...");

        SocketOptions options = new SocketOptions();
        options.AutoConnect = false;
        options.ConnectWith = BestHTTP.SocketIO.Transports.TransportTypes.WebSocket;
        string uri = string.Format("{0}/socket.io/", this.serverEndpoint);
        this.socketManager = new SocketManager(new System.Uri(uri), options);

        this.roomSocket = new RoomSocket();
        this.RoomSocket.Connect();
    }

    // public void Subscribe<M>(BaseView view, SocketEventNames eventName, UnityAction<SocketResponse<M>> successAction, UnityAction<SocketResponse<M>> errorAction, UnityAction startAction) where M : BaseModel {
    //     if (!this.events.ContainsKey(eventName)) {
    //         this.events[eventName] = new FabEvent<SocketResponse<M>>();
    //     }

    //     FabEvent<SocketResponse<M>> fabEvent = this.events[eventName] as FabEvent<SocketResponse<M>>;
    //     UnityAction<SocketStatus, SocketResponse<M>> action = (SocketStatus status, SocketResponse<M> response) =>
    //     {
    //         switch (status)
    //         {
    //             case SocketStatus.Success:
    //                 if (successAction != null)
    //                 {
    //                     successAction(response);
    //                 }

    //                 break;
    //             case SocketStatus.Error:
    //                 if (errorAction != null)
    //                 {
    //                     errorAction(response);
    //                 }

    //                 break;
    //             case SocketStatus.Start:
    //                 if (startAction != null)
    //                 {
    //                     startAction();
    //                 }

    //                 break;
    //             default:
    //                 break;
    //         }
    //     };

    //     fabEvent.AddListener(action);

    //     view.DestroyEvent.AddListener(
    //         () =>
    //         {
    //             fabEvent.RemoveListener(action);
    //         }
    //     );
    // }
}
