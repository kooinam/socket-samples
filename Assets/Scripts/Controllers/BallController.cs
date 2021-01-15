using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.05f;

    private int direction = -1;

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
    }

    public void SetDirection(int direction) {
        this.direction = direction;
    }
}
