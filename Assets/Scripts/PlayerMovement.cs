using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 100;
    [SerializeField] private float _jumpSpeed = 200;
    [SerializeField] private float _maxJumpTime = 0.1f;
    [SerializeField] private float _jumpGravity = 0.5f;
    [SerializeField] private float _groundCheckDistance = 1f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private bool _debugDrawn = false;

    private Rigidbody2D _rb;
    private Vector2 _velocity;
    private float _jumpTime;
    private float _defaultGravity;
    private bool _isGrounded;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _defaultGravity = _rb.gravityScale;
    }
    private void Update()
    {
        // Set velocity to the current _rb velocity
        _velocity = _rb.linearVelocity;

        CheckIfGrounded();

        MovementX();
        Jump();
        ApplyMovement(_velocity);
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
            _rb.gravityScale = 1.0f;
            _jumpTime = Time.time;
        }
        else if ((Input.GetButton("Jump")) && ((Time.time - _jumpTime) < _maxJumpTime))
        {
            _rb.gravityScale = 1.0f;
        }
        else
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
