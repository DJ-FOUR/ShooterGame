using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerShooting : MonoBehaviour
{
    private float time = 0f;
    private float timeBetweenBullets = 0.15f;
    private float flashDuration = 0.02f;
    private AudioSource gunAudio;
    private Light gunLight;
    private LineRenderer gunLine;
    private ParticleSystem gunParticle;

    void Awake()
    {
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
        gunLine = GetComponent<LineRenderer>();
        gunParticle = GetComponent<ParticleSystem>();
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
        //关闭闪光和射线
        if (time >= flashDuration)
        {
            gunLight.enabled = false;
            gunLine.enabled = false;
        }


        //
    }
    void Shoot()
    {
        //重置子弹间隔的计算
        time = 0f;
        //播放声音
        gunAudio.Play();
        //启用闪光
        gunLight.enabled = true;
        //启用射线(视觉）
        gunLine.SetPosition(0, transform.position);
        gunLine.SetPosition(1, transform.position + transform.forward * 100);
        gunLine.enabled = true;
        //播放粒子特效
        gunParticle.Play();
    }
}
