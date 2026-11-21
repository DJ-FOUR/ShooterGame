using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyEnemyMovement : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent nav;
    private MyPlayerHealth playerHealth;

    //击退
    private Vector3 knockbackDir;
    private float knockbackTimer;
    public float KnockbackSpeed = 6f;
    public float KnockbackDuration = 0.15f;

    //减速
    private float originalSpeed;
    private float slowTimer;
    public float SlowFactor = 0.4f;
    public float SlowDuration = 0.5f;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nav=GetComponent<NavMeshAgent>();
        originalSpeed = nav.speed;

        playerHealth = player.GetComponent<MyPlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        //玩家死亡不追击
        if (playerHealth.IsPlayerDead)
        {
            nav.enabled = false;
            enabled = false;
            return;
        }
        //减速计时
        if (slowTimer > 0f)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0f)
                nav.speed = originalSpeed;
        }

        //击退期间不追人
        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.deltaTime;
            nav.Move(knockbackDir * KnockbackSpeed * Time.deltaTime);
            return;
        }
        nav.SetDestination(player.transform.position);


    }

    //受击时由 MyEnemyHealth 调用
    public void Knockback(Vector3 direction)
    {
        direction.y = 0f;
        knockbackDir = direction.normalized;
        knockbackTimer = KnockbackDuration;
    }
    //受击减速，由 MyEnemyHealth 调用
    public void Slow()
    {
        nav.speed = originalSpeed * SlowFactor;
        slowTimer = SlowDuration;
    }

    public void StopAgent()
    {
        nav.enabled = false;
    }
}
