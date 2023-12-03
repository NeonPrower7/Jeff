using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth;
    private float _health;

    void Start()
    {
        _health = maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

    }

    public void ReduceHealth(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
