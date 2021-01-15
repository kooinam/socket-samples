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

        // this.infoText.text = string.Format("Time: {0}", RoomSocket.time);
    }
}
