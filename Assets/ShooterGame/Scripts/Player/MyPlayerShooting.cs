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
        playerMovement = GetComponent<PlayerMovement>();
        //获取射线检测的图层
        shootMask = LayerMask.GetMask("Shootable");
    }

    // Update is called once per frame
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

        //启用射线(视觉）
        gunLine.SetPosition(0, transform.position);
        //射线检测
        shootRay.origin = transform.position - transform.forward * 1.5f;
        shootRay.direction = transform.forward;
        if (Physics.Raycast(shootRay, out shootHit, 100, shootMask))    //Raycast检测射线是否命中碰撞体，返回bool
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
            gunLine.SetPosition(1, transform.position + transform.forward * 100);
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
