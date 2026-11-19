using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyEnemyHealth : MonoBehaviour
{
    public int StartingHealth = 100;
    public AudioClip DeathClip;
    public ParticleSystem deathParticle;

    private AudioSource enemyAudioSource;
    private ParticleSystem enemyParticles;
    private Animator enemyAnimator;
    private MyEnemyMovement deathMovement;
    private CapsuleCollider enemyColider;
    private bool IsDead = false;
    private bool IsSinking = false;

    //受击闪烁
    private SkinnedMeshRenderer body;
    private Color originalEmission;
    private float flashTimer = 0.1f;

    void Awake()
    {
        enemyAudioSource = GetComponent<AudioSource>();
        enemyParticles = GetComponentInChildren<ParticleSystem>();
        body = GetComponentInChildren<SkinnedMeshRenderer>();
        originalEmission = body.material.GetColor("_EmissionColor");
        enemyAnimator = GetComponentInChildren<Animator>();
        deathMovement = GetComponentInChildren<MyEnemyMovement>();
        enemyColider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSinking)
        {
            transform.Translate(-transform.up * Time.deltaTime);
        }
    }
    //受伤
    public void TakeDamage(int amount, RaycastHit hit)
    {
        if (IsDead == false)
        {
            //血量扣除
            StartingHealth -= amount;
            if (StartingHealth < 0)
            {
                StartingHealth = 0;
                IsDead = true;
            }
            //播放受击音效
            enemyAudioSource.Play();
            //播放粒子特效
            enemyParticles.transform.position = hit.point;
            enemyParticles.Play();
            //受击闪烁0.1秒
            StartCoroutine(HitFlash());
            //受击减速
            deathMovement.Slow();
            //敌人死亡
            if (IsDead == true)
            {
                Death();
            }
        }

    }
    private void Death()
    {
        //取消敌人寻路
        deathMovement.enabled = false;
        deathMovement.StopAgent();
        //取消敌人的碰撞
        enemyColider.enabled = false;
        //取消子弹的碰撞
        gameObject.layer = 0;
        //减少系统开销
        GetComponent<Rigidbody>().isKinematic = true;
        //播放敌人死亡动画
        enemyAnimator.SetTrigger("Death");
        //播放敌人死亡音效
        enemyAudioSource.clip = DeathClip;
        enemyAudioSource.Play();
        //播放死亡粒子特效
        deathParticle.Play();
    }
    IEnumerator HitFlash()
    {
        body.material.SetColor("_EmissionColor", new Color(4f, 4f, 4f, 1f));
        yield return new WaitForSeconds(flashTimer);
        body.material.SetColor("_EmissionColor", originalEmission);
    }
    public int CurrentHealth()
    {
        return StartingHealth;
    }
    
    public void StartSinking()
    {
        IsSinking = true;
        Destroy(gameObject, 1f);
    }
}
