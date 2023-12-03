using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Collider2D collider;
    [SerializeField] private ParticleSystem ps;
    private GameObject player;
    private PlayerController pc;
    public float damage;
    public float health;
    public float invincibleTime;
    private float _invincibleTimer;
    private bool _isInvincible = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerController>();
    }

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

        ps.transform.localScale = new Vector2(player.transform.localScale.x, 1);

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
        ps.Play();
        _isInvincible = true;
        _invincibleTimer = invincibleTime;
        if (health <= 0) { Destroy(gameObject); }
    }
}
