using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyStats : CharacterStats
{

     
    public HealthBar healthBar;
    public bool showHealthBar = false;
    public bool isAlive = true;

    Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.ShowHealthBar(showHealthBar);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        showHealthBar = true;
        healthBar.ShowHealthBar(showHealthBar);
        currentHealth = currentHealth - damage;

        healthBar.SetCurrentHealth(currentHealth);

        animator.Play("Damage");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animator.Play("Death");
            showHealthBar = false;
            healthBar.ShowHealthBar(showHealthBar);
            ScoreScript.scoreValue += 100;
            //for the death logic
            isAlive = false;
        }
    }

}
