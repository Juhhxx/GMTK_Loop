using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 100;
    [SerializeField] private float _jumpSpeed = 200;
    [SerializeField] private float _maxJumpTime = 0.1f;
    [SerializeField] private float _jumpGravity = 0.5f;
    [SerializeField] private float _groundCheckDistance = 1f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private bool _debugDrawn = false;

    public UnityEvent OnPlayerDeath;

    private Rigidbody2D _rb;
    private Vector2 _velocity;
    private float _jumpTime;
    private float _defaultGravity;
    private bool _isGrounded;
    private Animator _anim;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _defaultGravity = _rb.gravityScale;

        _anim = GetComponent<Animator>();
    }
    private void Update()
    {
        // Set velocity to the current _rb velocity
        _velocity = _rb.linearVelocity;

        CheckIfGrounded();

        MovementX();
        Jump();
        ApplyMovement(_velocity);

        if (Input.GetKeyDown(KeyCode.O))
            KillSelf();

        _anim.SetFloat("SpeedX", Mathf.Abs(_velocity.x));
        _anim.SetFloat("SpeedY", _velocity.y);
        _anim.SetBool("IsGrounded", _isGrounded);

        if ((_velocity.x < 0) && (transform.right.x > 0)) transform.rotation = Quaternion.Euler(0, 180, 0);
        else if ((_velocity.x > 0) && (transform.right.x < 0)) transform.rotation = Quaternion.identity;
    }

    public void KillSelf()
    {
        Debug.Log("PLAYR DIED!!!");
        OnPlayerDeath.Invoke();
    }

    private void MovementX()
    {
        float deltaX = Input.GetAxis("Horizontal");

        _velocity.x = _maxSpeed * deltaX;
    }
    private void Jump()
    {
        if ((Input.GetButtonDown("Jump")) && (_isGrounded))
        {
            _velocity.y = _jumpSpeed;
            _rb.gravityScale = _jumpGravity;
            _jumpTime = Time.time;
        }
        else if ((Input.GetButton("Jump")) && ((Time.time - _jumpTime) < _maxJumpTime))
        {
            _rb.gravityScale = _jumpGravity;
        }
        else
        {
            _rb.gravityScale = _defaultGravity;
        }
        
        if (_rb.linearVelocity.y < 0f)
        {
            _rb.gravityScale = _defaultGravity;
        }
    }
    private void ApplyMovement(Vector2 speed)
    {
        _rb.linearVelocity = speed;
    }

    private void CheckIfGrounded()
    {
        _isGrounded = Physics2D.Raycast(transform.position,
        Vector3.down, _groundCheckDistance, _groundLayer);

        if (_debugDrawn) DrawGroundedCheckLine();
    }
    private void DrawGroundedCheckLine()
    {
        Color color = _isGrounded ? Color.blue : Color.red;
        Vector3 endPoint = transform.position + (Vector3.down * _groundCheckDistance);

        Debug.DrawLine(transform.position, endPoint, color);
    }
}
