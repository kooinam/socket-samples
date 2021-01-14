using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsPanelView : BaseView
{
    [SerializeField]
    private string roomID = "test";

    [SerializeField]
    private string playerName = "testplayer";

    public void OnClickJoinRoom() {
        NetworkManager.Instance.RoomSocket.CreateRoom(this.roomID).
            OnSuccess(
                () => {
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
}
