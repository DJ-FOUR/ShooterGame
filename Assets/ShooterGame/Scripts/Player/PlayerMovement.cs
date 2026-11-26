using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 6f;    //玩家移动速度

    private Rigidbody rb;
    private Animator anim;
    private float h;
    private float v;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();     //通过rigidbody组件实现移动和旋转
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        //鼠标控制旋转
        Turning();
        //切换动画
        Animating(h, v);
    }
    private void FixedUpdate()
    {
        //键盘控制移动
        Move(h,v);

    }
    private void Move(float h,float v)
    {
        Vector3 movementV3 = new Vector3(h, 0, v);
        movementV3 = movementV3.normalized * Speed * Time.deltaTime;

        rb.MovePosition(transform.position + movementV3);
    }
    private void Turning()
    {
        if (GlobalMenu.IsOpen) return; // 面板打开时鼠标不控制转向
        //创建相机射线（鼠标位置）
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //用数学平面求交代替地板碰撞体，避免碰撞体阻挡其他Rigidbody
        Plane aimPlane = new Plane(Vector3.up, new Vector3(0f, 0.31f, 0f));
        if (aimPlane.Raycast(cameraRay, out float hitDistance))
        {
            Vector3 v3 = cameraRay.GetPoint(hitDistance) - transform.position;
            v3.y = 0;
            Quaternion quaternion = Quaternion.LookRotation(v3);
            rb.MoveRotation(quaternion);
        }
    }
    void Animating(float h,float v)
    {
        bool isWalking = h!=0 || v!=0;
        anim.SetBool("IsWalking", isWalking);
    }
}
