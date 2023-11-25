using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackParticle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    public float fadeColor;
    public float fadeTime;

    void Awake()
    {
        sprite.enabled = false;
    }

    public IEnumerator Fade()
    {
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
