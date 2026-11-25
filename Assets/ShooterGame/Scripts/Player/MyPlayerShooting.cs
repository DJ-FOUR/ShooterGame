using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerShooting : MonoBehaviour
{
    public int Damage = 25;
    public float PropForce = 5f;   //子弹推动道具的冲量
    public float ShootSpeed = 4f;    //射击时的移速

    private float time = 0f;
    private float timeBetweenBullets = 0.15f;
    private float flashDuration = 0.05f;
    private AudioSource gunAudio;
    private Light gunLight;
    private LineRenderer gunLine;
    private ParticleSystem gunParticle;
    private PlayerMovement playerMovement;

    //开枪发射射线的变量
    private Ray shootRay;           //定义射线
    private RaycastHit shootHit;    //定义射线碰到的物体
    private int shootMask;          //定义可以与射线相撞的图层

    void Awake()
    {
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
        gunLine = GetComponent<LineRenderer>();
        gunParticle = GetComponent<ParticleSystem>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        //获取射线检测的图层
        shootMask = LayerMask.GetMask("Shootable");
    }

    void Update()
    {
        //设置子弹的间隔时间
        time = time += Time.deltaTime;
        //开火键
        if (Input.GetButton("Fire1") && time >= timeBetweenBullets)
        {
            Shoot();    //射击

        }
    }
    void Shoot()
    {
        //重置子弹间隔的计算
        time = 0f;
        //播放声音
        gunAudio.Play();
        //启用闪光和射线
        StartCoroutine(GunFlash());
        //播放粒子特效
        gunParticle.Play();

        //先用相机射线找出鼠标指着的3D点（数学平面求交，不依赖地板碰撞体）
        Ray aimRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane aimPlane = new Plane(Vector3.up, new Vector3(0f, 0.31f, 0f));

        Vector3 aimPoint;
        if (aimPlane.Raycast(aimRay, out float hitDistance))
            aimPoint = aimRay.GetPoint(hitDistance);
        else
            aimPoint = aimRay.GetPoint(100);

        //脚本挂在枪口节点上，transform.position 即枪口位置
        Vector3 muzzle = transform.position;
        //射线从碰撞体内部出发会漏检：先从身体向枪口补检，被挡住就把开火点拉回障碍物外侧
        Vector3 body = playerMovement.transform.position + Vector3.up * 0.5f;
        Vector3 toMuzzle = muzzle - body;
        if (Physics.Raycast(body, toMuzzle.normalized, out RaycastHit muzzleBlock, toMuzzle.magnitude, shootMask))
        {
            muzzle = muzzleBlock.point - toMuzzle.normalized * 0.01f;
        }
        shootRay.origin = muzzle;
        shootRay.direction = (aimPoint - muzzle).normalized;
        gunLine.SetPosition(0, muzzle);

        if (Physics.Raycast(shootRay, out shootHit, 100, shootMask))    //检测射线是否命中碰撞体
        {
            gunLine.SetPosition(1, shootHit.point);     //截断射线
            if (shootHit.collider.gameObject.tag == "Enemy")
            {
                //伤害扣除
                MyEnemyHealth enemyHealth = shootHit.collider.GetComponent<MyEnemyHealth>();
                enemyHealth.TakeDamage(Damage, shootHit);
            }
            else
            {
                //推动场景道具
                Rigidbody propRb = shootHit.collider.attachedRigidbody;
                if (propRb != null && !propRb.isKinematic)
                {
                    propRb.AddForceAtPosition(shootRay.direction * PropForce, shootHit.point, ForceMode.Impulse);
                }
            }
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * 100);
        }
    }
    IEnumerator GunFlash()
    {
        gunLight.enabled = true;
        gunLine.enabled = true;
        yield return new WaitForSeconds(flashDuration);
        gunLight.enabled = false;
        gunLine.enabled = false;
    }
}
