using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 640f;    //玩家移动速度

    private Rigidbody rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();     //通过rigidbody组件实现移动和旋转
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        //键盘控制移动
        Move(h,v);

        //鼠标控制旋转
        Turning();

        //切换动画
        Animating(h, v);
    }
    private void Move(float h,float v)
    {
        Vector3 movementV3 = new Vector3(h, 0, v);
        movementV3 = movementV3.normalized * Speed * Time.deltaTime;

        rb.MovePosition(transform.position + movementV3);
    }
    private void Turning()
    {
        //创建相机射线（鼠标位置）
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //得到地板图层所在的编号
        int floorLayer = LayerMask.GetMask("Floor");
        //射线检测,返回射线穿透的数据给floorHit
        RaycastHit floorHit;
        bool isTouchFloor = Physics.Raycast(cameraRay, out floorHit, 100, floorLayer);      //（射线，射线碰到的物体-输出的，射线长度，射线碰到的物体所在的图层）
        
        if (isTouchFloor)
        {
            Vector3 v3 = floorHit.point - transform.position;
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
