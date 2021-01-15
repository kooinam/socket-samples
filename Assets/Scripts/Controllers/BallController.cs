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

    [SerializeField]
    private Transform hudTransform = null;

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

    void Update() {
        this.hudTransform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.direction == 0) {
            this.transform.position += (Vector3.up * this.speed);
            // this.transform.LookAt(Vector3.up);
        } else if (this.direction == 1) {
            this.transform.position += (Vector3.right * this.speed);
            // this.transform.LookAt(Vector3.right);
        } else if (this.direction == 2) {
            this.transform.position += (Vector3.down * this.speed);
            // this.transform.LookAt(Vector3.down);
        } else if (this.direction == 3) {
            this.transform.position += (Vector3.left * this.speed);
            // this.transform.LookAt(Vector3.left);
        }

        if (this.logPosition) {
            if (this.socket.GetTime() % 100 == 0) {
                Debug.LogFormat("{0} - {1} - {2}",this.socket.GetTime(), this.nameText.text, this.transform.position);
            }
        }
    }

    public void SetDirection(int direction) {
        this.direction = direction;

        this.transform.eulerAngles = new Vector3(0, 0, this.direction * -90);
    }

    public void SetName(string name) {
        this.nameText.text = name;
    }
}
