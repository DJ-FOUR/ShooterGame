using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 6f;    //玩家移动速度

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 movementV3 = new Vector3(h, 0, v);
        movementV3 = movementV3.normalized * Speed * Time.deltaTime;

        rb.MovePosition(transform.position + movementV3);

    }

}
