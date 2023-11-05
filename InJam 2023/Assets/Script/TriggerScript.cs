using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TriggerScript : MonoBehaviour
{
    [SerializeField] private Image panel;
    [SerializeField] private GameObject presentedBy;
    [SerializeField] private Animator animator;

    private Rigidbody2D _rb;
    private float _originalGravity;
    private float fadeColor = 1f;

    public float fadeTime;
    public float waitTime;
    public int speedLimit;
    public bool fall;
    public bool isSpeedLimit;
    public bool canMove;

    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _originalGravity = _rb.gravityScale;
        if(fall)
        {
            StartCoroutine(FadeOut());
            animator.SetBool("fallCutscene", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("SlowFall"))
        {
            _rb.gravityScale = 1;
            canMove = false;
        }

        if(other.gameObject.CompareTag("StandUp"))
        {
            panel.color = new Color(0, 0, 0, 1);
            fadeColor = 1f;
            StartCoroutine(Logo(waitTime));
            Destroy(other.gameObject);
            animator.SetBool("fallCutscene", false);
            isSpeedLimit = true;
        }

        if(other.gameObject.CompareTag("GNOMED"))
        {
            isSpeedLimit = false;
        }

        if(other.gameObject.CompareTag("ExitRoom"))
        {
            int nextRoom = other.gameObject.GetComponent<ExitRoomTrigger>().nextRoom;
            SceneManager.LoadScene(nextRoom);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("SlowFall"))
        {
            _rb.gravityScale = _originalGravity;
        }
    }

    private IEnumerator FadeOut()
    {
        while (fadeColor >= 0)
        {
            panel.color = new Color(0, 0, 0, fadeColor);
            fadeColor -= 0.1f;
            yield return new WaitForSeconds(fadeTime);
        }
    }
    private IEnumerator Logo(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        presentedBy.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        presentedBy.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        while (fadeColor >= 0)
        {
            panel.color = new Color(0, 0, 0, fadeColor);
            fadeColor -= 0.1f;
            yield return new WaitForSeconds(fadeTime);
        }
        canMove = true;
    }
}
