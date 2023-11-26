using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Collider2D collider;
    [SerializeField] private PlayerController pc;
    public float damage;
    public float health;
    public float invincibleTime;
    private float _invincibleTimer;
    private bool _isInvincible = false;

    private void Update()
    {
        if (_isInvincible)
        {
            _invincibleTimer -= Time.deltaTime;
            if (_invincibleTimer <= 0)
            {
                _isInvincible = false;
            }
        }

        if (pc.isDashing)
        { collider.isTrigger = true; }
        else
        { collider.isTrigger = false; }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.ReduceHealth(damage);
        }
    }

    public void ReduceHealth(float damage)
    {
        if (_isInvincible) return;

        health -= damage;
        _isInvincible = true;
        _invincibleTimer = invincibleTime;
        if (health <= 0) { Destroy(gameObject); }
    }
}
