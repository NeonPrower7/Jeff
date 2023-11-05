using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Reference")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TriggerScript ts;
    [SerializeField] private Animator animator;
    private Rigidbody2D _rb;

    [Header("Walk")]
    public int walkingSpeed;
    private float _horizontal;
    private int speed;
    private bool _isFacingRight = true;

    [Header("Jump")]
    public int jumpForce;
    private float _checkRadius = 0.2f;
    private bool _isGround;

    [Header("Dash")]
    public TrailRenderer tr;
    public float dashingPower;
    public float dashingTime;
    private float _originalGravity;
    private bool _canDash = true;
    private bool _isDashing = false;

    [Header("Attack")]
    public float attackingTime;
    public float attackCoolDown;
    private bool _canAttack = true;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _originalGravity = _rb.gravityScale;
    }

    void Update()
    {
        if(!ts.canMove || _isDashing) return;

        _horizontal = Input.GetAxisRaw("Horizontal");
        _isGround = Physics2D.OverlapCircle(groundCheck.position, _checkRadius, groundLayer);

        if(_horizontal != 0) animator.SetBool("isMoving", true);
        else animator.SetBool("isMoving", false);

        if(_isGround) animator.SetBool("isJumping", false);
        
        if(_isFacingRight && _horizontal < 0f || !_isFacingRight && _horizontal > 0f)
        {Flip();}
        if(_isGround) _canDash = true;

        if(ts.isSpeedLimit) return;

        if(Input.GetKeyDown(KeyCode.Space) && _isGround) Jump();
        if(Input.GetMouseButtonDown(0) && _canAttack) StartCoroutine(Attack());
        if(Input.GetMouseButtonDown(1) && _canDash) StartCoroutine(Dash());
    }

    void FixedUpdate()
    {
        if(!ts.canMove || _isDashing) return;

        if(ts.isSpeedLimit) speed = ts.speedLimit;
        else speed = walkingSpeed;
        _rb.velocity = new Vector2(_horizontal*speed, _rb.velocity.y);
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void Jump()
    {
        _rb.AddForce(Vector2.up * jumpForce);
        animator.SetBool("isJumping", true);
    }

    private IEnumerator Attack()
    {
        _canAttack = false;
        animator.SetBool("isAttack", true);
        yield return new WaitForSeconds(attackingTime);
        animator.SetBool("isAttack", false);
        yield return new WaitForSeconds(attackCoolDown);
        _canAttack = true;
    }

    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;
        animator.SetBool("isDashing", true);
        tr.emitting = true;
        _rb.gravityScale = 0f;
        _rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        _rb.gravityScale = _originalGravity;
        tr.emitting = false;
        animator.SetBool("isDashing", false);
        _isDashing = false;
    }
}
