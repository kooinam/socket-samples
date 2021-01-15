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
                    List<Dictionary<string, object>> rpcDatas = response["rpcs"] as List<Dictionary<string, object>>;

                    string payload = DictionarySerializer.ToJSON(response);

                    Debug.Log(payload);
                }
            ).
            Emit();
    }
}
