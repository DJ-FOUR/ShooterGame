using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MyEnemyAttack : MonoBehaviour
{
    public float Damage = 10;     //攻击伤害
    public float intervalTimer = 2.5f;   //攻击间隔
    private float realTime;
    private bool playerInRange = false;     //玩家是否在扣血范围内
    private MyPlayerHealth myPlayerHealth;
    private GameObject player;      //找到玩家
    private Animator enemyIdleAnim;     //玩家死亡播放待机动画

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");    //通过标签找到玩家
        myPlayerHealth = player.GetComponent<MyPlayerHealth>();
        enemyIdleAnim = GetComponent<Animator>();
        realTime = intervalTimer;
    }

    // Update is called once per frame
    void Update()
    {
        realTime += Time.deltaTime;
        if (playerInRange && realTime >= intervalTimer && !myPlayerHealth.IsPlayerDead)
        {
            Attack();
            realTime = 0f;
        }
        if (myPlayerHealth.IsPlayerDead)
        {
            enemyIdleAnim.SetTrigger("PlayerDead");
        }
    }
    private void FixedUpdate()
    {
        
    }

    private void Attack()
    {
        myPlayerHealth.TakeDamage(Damage);
    }

    //玩家碰到触发器
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;

        }
    }
    //玩家离开触发器
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

}
