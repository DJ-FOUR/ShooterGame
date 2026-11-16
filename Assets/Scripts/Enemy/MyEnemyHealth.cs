using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyEnemyHealth : MonoBehaviour
{
    public int StartingHealth = 100;

    private AudioSource enemyAudioSource;
    private ParticleSystem enemyParticles;

    void Awake()
    {
        enemyAudioSource = GetComponent<AudioSource>();
        enemyParticles = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //受伤
    public void TakeDamage(int amount, RaycastHit hit)
    {
        //血量扣除
        StartingHealth -= amount;
        //播放受击音效
        enemyAudioSource.Play();
        //播放粒子特效
        enemyParticles.transform.position = hit.point;
        enemyParticles.Play();
    }
    public int CurrentHealth()
    {
        return StartingHealth;
    }
}
