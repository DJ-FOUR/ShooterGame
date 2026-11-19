using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerHealth : MonoBehaviour
{
    public float PlayerStartingHealth = 100;
    public AudioClip PlayerDeathClip;
    public bool IsPlayerDead = false;

    private float playerCurrentHealth;
    private AudioSource playerAudio;
    private Animator anim;
    private MyPlayerShooting playerShooting;
    private PlayerMovement playerMovement;

    void Awake()
    {
        playerCurrentHealth = PlayerStartingHealth;
        playerAudio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        playerShooting = GetComponentInChildren<MyPlayerShooting>();
        playerMovement = GetComponent<PlayerMovement>();
    }


    void Update()
    {
        
    }
    public void TakeDamage(float amount)
    {
        if (IsPlayerDead == false)
        {
            //Íæ¼Ò¿ÛÑª
            playerCurrentHealth -= amount;
            Debug.Log(playerCurrentHealth);
            if (playerCurrentHealth <= 0)
            {
                playerCurrentHealth = 0;
                IsPlayerDead = true;
                Death();
            }
            else
            {
                //ÉùÒô
                playerAudio.Play();
            }
        }


    }
    private void Death()
    {
        //ÉùÒô
        playerAudio.clip = PlayerDeathClip;
        playerAudio.Play();
        //¶¯»­
        anim.SetTrigger("Death");
        //ÔÝÍ£ÒÆ¶¯ºÍÉä»÷×é¼þ
        playerShooting.enabled = false;
        playerMovement.enabled = false;

    }
    public void RestartLevel()
    {
    }
}
