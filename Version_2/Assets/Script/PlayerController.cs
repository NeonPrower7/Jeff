using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Reference")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TriggerScript ts;
    [SerializeField] private GameObject attackPrefab;
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
    public float checkRadius;
    private bool _isGround;
    private bool _isJumping;

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
        _isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if(_horizontal != 0) animator.SetBool("isMoving", true);
        else animator.SetBool("isMoving", false);

        if(ts.isSpeedLimit) return;

        if (_isGround)
        {
            animator.SetBool("isGrounded", true);
            if(animator.GetBool("isJumping") && !_isJumping)
            {
                animator.SetBool("isJumping", false);
            }
            _canDash = true;
        }
        else animator.SetBool("isGrounded", false);

        if (_isFacingRight && _horizontal < 0f || !_isFacingRight && _horizontal > 0f)
        { Flip(); }

        if(Input.GetKeyDown(KeyCode.Space) && _isGround) StartCoroutine(Jump());
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

    private IEnumerator Jump()
    {
        animator.SetBool("isJumping", true);
        _isJumping = true;
        _rb.AddForce(Vector2.up * jumpForce);
        yield return new WaitForSeconds(jumpTime);
        _isJumping = false;
    }

    private IEnumerator Attack()
    {
        _canAttack = false;
        animator.SetBool("isAttack", true);
        GameObject attackObj = Instantiate(attackPrefab, transform.position, transform.rotation);
        attackObj.transform.localScale = transform.localScale;
        yield return new WaitForSeconds(attackingTime);
        attackObj.transform.position = transform.position;
        StartCoroutine(attackObj.GetComponent<AttackParticle>().Fade());
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
