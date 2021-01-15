using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelView : MonoBehaviour
{
    [SerializeField]
    private Text infoText = null;

    void FixedUpdate() {
        RoomSocket roomSocket = NetworkManager.Instance.RoomSocket;

        this.infoText.text = string.Format("Room: {0}\nHost: {1}\nTime: {2}", roomSocket.RoomID, roomSocket.HostID, roomSocket.GetTime());
    }
}
