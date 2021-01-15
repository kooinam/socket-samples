using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsPanelView : BaseView
{
    [SerializeField]
    private string roomID = "test";

    [SerializeField]
    private string playerName = "testplayer";

    [SerializeField]
    private PlaygroundController playgroundController = null;

    public void OnClickCreateRoom() {
        NetworkManager.Instance.RoomSocket.CreateRoom(this.roomID).
            OnSuccess(
                (Dictionary<string, object> response) => {
                    NetworkManager.Instance.RoomSocket.Configure(response);
                    NetworkManager.Instance.RoomSocket.SubscribeRPC();

                    NetworkManager.Instance.RoomSocket.JoinGame(this.playerName).
                        Emit();
                }
            ).
            OnError(
                (string errorMessage) => {
                    ErrorPopupView popupView = ResourcesManager.Instance.InstantiateUI<ErrorPopupView>(ResourcesManager.Instance.ErrorPopupPrefab);

                    popupView.Setup(errorMessage);
                }
            ).
            Emit();
    }

    public void OnClickJoinRoom()
    {
        NetworkManager.Instance.RoomSocket.JoinRoom(this.roomID).
            OnSuccess(
                (Dictionary<string, object> response) =>
                {
                    NetworkManager.Instance.RoomSocket.Configure(response);
                    NetworkManager.Instance.RoomSocket.SubscribeRPC();

                    this.setup();

                    NetworkManager.Instance.RoomSocket.JoinGame(this.playerName).
                        Emit();
                }
            ).
            OnError(
                (string errorMessage) =>
                {
                    ErrorPopupView popupView = ResourcesManager.Instance.InstantiateUI<ErrorPopupView>(ResourcesManager.Instance.ErrorPopupPrefab);

                    popupView.Setup(errorMessage);
                }
            ).
            Emit();
    }

    private void setup() {
        NetworkManager.Instance.RoomSocket.GetRPCs().
            OnSuccess(
                (Dictionary<string, object> response) =>
                {
                    List<object> rpcDatas = response["rpcs"] as List<object>;

                    foreach (object rpcData in rpcDatas) {
                        RPC rpc = RPC.Parse(rpcData as Dictionary<string, object>);

                        if (rpc.Equals(RPCName.JoinGame)) {
                            this.playgroundController.SpawnPlayer(rpc);
                        }
                    }
                }
            ).
            Emit();
    }
}
