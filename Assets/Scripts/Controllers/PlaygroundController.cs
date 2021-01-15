using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundController : MonoBehaviour
{
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
        RPC rpc = this.socket.PopRPC();

        if (rpc != null) {
            if (rpc.Equals(RPCName.JoinGame)) {
                this.spawnPlayer(rpc);
            } else if (rpc.Equals(RPCName.Move)) {
                this.move(rpc);
            }
        }
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.UpArrow)) {
            this.socket.Move(0).Emit();
        } else if (Input.GetKeyUp(KeyCode.RightArrow)) {
            this.socket.Move(1).Emit();
        } else if (Input.GetKeyUp(KeyCode.DownArrow)) {
            this.socket.Move(2).Emit();
        } else if (Input.GetKeyUp(KeyCode.LeftArrow)) {
            this.socket.Move(3).Emit();
        }
    }

    private void spawnPlayer(RPC rpc) {
        string playerName = rpc.ParamStr("playerName");

        Debug.Log(this.rpcLog(rpc, playerName));
    }

    private void move(RPC rpc) {
        int direction = rpc.ParamInt("direction", -1);

        Debug.Log(this.rpcLog(rpc, direction));
    }

    private string rpcLog(RPC rpc, object o) {
        return string.Format("Now[{0}]:ExecuteAt[{1}] - {2}", this.socket.GetTime(), rpc.Timestamp, o);
    }
}
