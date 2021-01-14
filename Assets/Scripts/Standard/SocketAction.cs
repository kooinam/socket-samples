using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO;
using UnityEngine.Events;

public class SocketAction
{
    private BaseSocket socket = null;
    private string actionName = null;
    private string payload = null;
    private UnityAction successAction = null;
    private UnityAction<string> errorAction = null;

    public SocketAction(BaseSocket socket, string actionName, string payload)
    {
        this.socket = socket;
        this.actionName = actionName;
        this.payload = payload;
    }

    public SocketAction OnSuccess(UnityAction successAction)
    {
        this.successAction = successAction;

        return this;
    }

    public SocketAction OnError(UnityAction<string> errorAction) {
        this.errorAction = errorAction;

        return this;
    }

    public void Emit() {
        // LoggerManager.I.Info(string.Format("Emitting {0}#{1} - #{2}", this.socket.Nsp, this.actionName, this.payload));

        if (!this.socket.Socket.IsOpen) {
            throw new System.Exception(string.Format("Socket #{0} is closed", this.socket.Nsp));
        }

        this.socket.Socket.Emit(this.actionName, (Socket socket, Packet originalPacket, object[] args) =>
        {
            Dictionary<string, object> response = args[0] as Dictionary<string, object>;

            if (response["status"].Equals("success")) {
                if (this.successAction != null)
                {
                    this.successAction();
                }
            }

            if (response["status"].Equals("error")) {
                if (this.errorAction != null) {
                    this.errorAction(response["errorMsg"] as string);
                }
            }
        }, payload);
    }
}
