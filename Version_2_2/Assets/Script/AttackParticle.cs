using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackParticle : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private SpriteRenderer sprite;
    private Collider2D collider;
    private bool _move;
    public float fadeColor;
    public float fadeTime;
    public float damage;

    private void Awake()
    {
        _move = true;
        sprite.enabled = false;
        player = GameObject.Find("Player");
        collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if(_move) { transform.position = player.transform.position; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        { enemyHealth.ReduceHealth(damage); }
    }

    public IEnumerator Fade()
    {
        // Destroy(collider);
        _move = false;
        sprite.enabled = true;
        while (fadeColor >= 0)
        {
            sprite.color = new Color(1, 1, 1, fadeColor);
            fadeColor -= 0.05f;
            yield return new WaitForSeconds(fadeTime);
        }
        Destroy(gameObject);
    }
}
