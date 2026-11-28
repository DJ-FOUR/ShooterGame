using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayerHealth : MonoBehaviour
{
    public float PlayerStartingHealth = 100;
    public AudioClip PlayerDeathClip;
    public bool IsPlayerDead { get; private set; }

    public Text PlayerHealthUI;
    public Image DamageImage;
    public Color FlashColor = new(1f, 0f, 0f, 1);
    public float FlashTime = 0.1f;
    public Slider Slider;

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
            //玩家扣血
            playerCurrentHealth -= amount;
            //闪红屏
            StartCoroutine(DamageColor());
            if (playerCurrentHealth <= 0)
            {
                playerCurrentHealth = 0;
                IsPlayerDead = true;
                Death();
            }
            else
            {
                //声音
                playerAudio.Play();
            }
            //更新玩家血量UI
            PlayerHealthUI.text = playerCurrentHealth.ToString();
            Slider.value = playerCurrentHealth;
        }


    }
    private void Death()
    {
        //声音
        playerAudio.clip = PlayerDeathClip;
        playerAudio.Play();
        //动画
        anim.SetTrigger("Death");
        //暂停移动和射击组件
        playerShooting.enabled = false;
        playerMovement.enabled = false;

    }
    public void RestartLevel()
    {
    }
IEnumerator DamageColor()
{
    DamageImage.color = FlashColor;
    yield return new WaitForSeconds(FlashTime);
    while (DamageImage.color.a > 0.01f)
    {
        DamageImage.color = Color.Lerp(DamageImage.color, Color.clear, 5f * Time.deltaTime);
        yield return null;
    }
    DamageImage.color = Color.clear;   // 兜底精确归零
}
}
