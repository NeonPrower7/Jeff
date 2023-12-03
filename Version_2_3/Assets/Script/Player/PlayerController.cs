using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Reference")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private GameObject dashPrefab;
    [SerializeField] private Animator animator;
    private Rigidbody2D _rb;

    [Header("Walk")]
    public int walkingSpeed;
    private int speed;
    private float _horizontal;
    private bool _isFacingRight = true;

    [Header("Jump")]
    public int jumpForce;
    public float jumpTime;
    public Vector2 checkSize;
    private bool _isGround;

    [Header("Dash")]
    public TrailRenderer tr;
    public float dashingPower;
    public float dashingTime;
    private float _originalGravity;
    private bool _canDash = true;
    public bool playerHasDash;
    public bool isDashing = false;

    [Header("Attack")]
    public float attackingTime;
    public float attackCoolDown;
    public bool playerHasAttack;
    private bool _canAttack = true;

    [Header("Cutscenes")]
    public int speedLimit;
    public bool canMove;
    public bool isSpeedLimit;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _originalGravity = _rb.gravityScale;
    }

    void Update()
    {
        if(!canMove || isDashing) return;

        _horizontal = Input.GetAxisRaw("Horizontal");
        _isGround = Physics2D.OverlapBox(groundCheck.position, checkSize, 0f, groundLayer);

        if (_horizontal != 0) animator.SetBool("isMoving", true);
        else animator.SetBool("isMoving", false);

        if (_isFacingRight && _horizontal < 0f || !_isFacingRight && _horizontal > 0f)
        { Flip(); }

        if(isSpeedLimit) return;

        if (_isGround)
        {
            animator.SetBool("isGround", true);
            _canDash = true;
        }
        else animator.SetBool("isGround", false);

        if(Input.GetKeyDown(KeyCode.Space) && _isGround) StartCoroutine(Jump());
        if(Input.GetMouseButtonDown(0) && _canAttack && playerHasAttack) StartCoroutine(Attack());
        if(Input.GetMouseButtonDown(1) && _canDash && playerHasDash) StartCoroutine(Dash());
    }

    void FixedUpdate()
    {
        if(!canMove || isDashing) return;

        if(isSpeedLimit) speed = speedLimit;
        else speed = walkingSpeed;

        _rb.velocity = new Vector2(_horizontal * speed, _rb.velocity.y);
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private IEnumerator Jump()
    {
        animator.SetBool("isJumping", true);
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(jumpTime);
        animator.SetBool("isJumping", false);
    }

    private IEnumerator Attack()
    {
        _canAttack = false;
        animator.SetBool("isAttack", true);
        GameObject attackObj = Instantiate(attackPrefab, transform.position, transform.rotation);
        attackObj.transform.localScale = transform.localScale;
        yield return new WaitForSeconds(attackingTime);
        attackObj.transform.localScale = transform.localScale;
        StartCoroutine(attackObj.GetComponent<AttackParticle>().Fade());
        animator.SetBool("isAttack", false);
        yield return new WaitForSeconds(attackCoolDown);
        _canAttack = true;
    }

    private IEnumerator Dash()
    {
        _canDash = false;
        isDashing = true;
        animator.SetBool("isDashing", true);
        tr.emitting = true;
        GameObject dashObj = Instantiate(dashPrefab, transform.position, transform.rotation);
        dashObj.transform.localScale = transform.localScale;
        _rb.gravityScale = 0f;
        _rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        _rb.gravityScale = _originalGravity;
        tr.emitting = false;
        StartCoroutine(dashObj.GetComponent<AttackParticle>().Fade());
        animator.SetBool("isDashing", false);
        isDashing = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(groundCheck.position, checkSize);
    }
}
