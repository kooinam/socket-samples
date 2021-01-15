using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.05f;

    [SerializeField]
    private Text nameText = null;

    [SerializeField]
    private bool logPosition = false;

    private int direction = -1;

    private RoomSocket socket {
        get {
            return NetworkManager.Instance.RoomSocket;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.direction == 0) {
            this.transform.position += (Vector3.up * this.speed);
        } else if (this.direction == 1) {
            this.transform.position += (Vector3.right * this.speed);
        } else if (this.direction == 2) {
            this.transform.position += (Vector3.down * this.speed);
        } else if (this.direction == 3) {
            this.transform.position += (Vector3.left * this.speed);
        }

        if (this.logPosition) {
            if (this.socket.GetTime() % 100 == 0) {
                Debug.LogFormat("{0} - {1} - {2}",this.socket.GetTime(), this.nameText.text, this.transform.position);
            }
        }
    }

    public void SetDirection(int direction) {
        this.direction = direction;
    }

    public void SetName(string name) {
        this.nameText.text = name;
    }
}
