using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsPanelView : BaseView
{
    [SerializeField]
    private string roomID = "test";

    [SerializeField]
    private string playerName = "testplayer";

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
}
