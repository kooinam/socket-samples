using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundController : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab = null;

    private Dictionary<string, BallController> balls = null;

    private RoomSocket socket {
        get {
            return NetworkManager.Instance.RoomSocket;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.balls = new Dictionary<string, BallController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RPC rpc = this.socket.PopRPC();

        while (rpc != null) {
            if (rpc.Equals(RPCName.JoinGame)) {
                this.SpawnPlayer(rpc);
            } else if (rpc.Equals(RPCName.Move)) {
                this.Move(rpc);
            }

            LoggerManager.Instance.Info(
                string.Format(
                    "receivedAt: {6}<br />executedAt: {0}<br />timestamp: {1}<br />clientID: {2}<br/>sequenceID: {3}<br />rpcName: {4}<br />parameters: {5}",
                    this.socket.GetTime(),
                    rpc.Timestamp,
                    rpc.ClientID,
                    rpc.SequenceID,
                    rpc.Name,
                    DictionarySerializer.ToJSON(rpc.Parameters),
                    rpc.ReceivedAt
                )
            );

            rpc = this.socket.PopRPC();
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

    public void SpawnPlayer(RPC rpc) {
        string playerName = rpc.ParamStr("playerName");

        this.balls[rpc.ClientID] = ResourcesManager.Instance.Instantiate<BallController>(this.ballPrefab, this.transform);
        this.balls[rpc.ClientID].SetName(playerName);
    }

    public void Move(RPC rpc) {
        int direction = rpc.ParamInt("direction", -1);

        this.balls[rpc.ClientID].SetDirection(direction);
    }

    private string rpcLog(RPC rpc, object o) {
        return string.Format("Now[{0}]:ExecuteAt[{1}] - {2}", this.socket.GetTime(), rpc.Timestamp, o);
    }
}
