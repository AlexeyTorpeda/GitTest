using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerPhysicsMovement : MonoBehaviour
{
    [SerializeField] private float _minGroundNormalY = .65f;
    [SerializeField] private float _gravityModifier = 1f;
    [SerializeField] private Vector2 _velocity;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speed = 5f;

    private float _jumpingPower = 6.5f;
    private Vector2 _targetVelocity;
    private bool _isGrounded;
    private Vector2 _groundNormal;
    private Rigidbody2D _rigidbody2D;
    private ContactFilter2D _contactFilter;
    private RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
    private List<RaycastHit2D> _hitBufferList = new List<RaycastHit2D>(16);
    private bool _isFacingRight = true;
    private float _horizontalMove;

    private const float _minMoveDistance = 0.001f;
    private const float _shellRadius = 0.01f;

    private void OnEnable()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(_layerMask);
        _contactFilter.useLayerMask = true;
    }

    private void Update()
    {
        _horizontalMove = Input.GetAxis("Horizontal");
        _targetVelocity = new Vector2(_horizontalMove * _speed, 0);
        int horizontalMoveHash = Animator.StringToHash("HorizontalMove");
        _animator.SetFloat(horizontalMoveHash, Mathf.Abs(_horizontalMove));

        if (Input.GetKey(KeyCode.Space) && _isGrounded)
            _velocity.y = _jumpingPower;
    }

    private void FixedUpdate()
    {
        _velocity += _gravityModifier * Physics2D.gravity * Time.deltaTime;
        _velocity.x = _targetVelocity.x;

        _isGrounded = false;

        Vector2 deltaPosition = _velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;

        DoMovement(move, false);

        move = Vector2.up * deltaPosition.y;

        DoMovement(move, true);

        if (_horizontalMove < 0 && _isFacingRight)
        {
            Flip();
        }
        else if (_horizontalMove > 0 && !_isFacingRight)
        {
            Flip();
        }
    }

    private void DoMovement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > _minMoveDistance)
        {
            int count = _rigidbody2D.Cast(move, _contactFilter, _hitBuffer, distance + _shellRadius);

            _hitBufferList.Clear();

            for (int i = 0; i < count; i++)
            {
                _hitBufferList.Add(_hitBuffer[i]);
            }

            for (int i = 0; i < _hitBufferList.Count; i++)
            {
                Vector2 currentNormal = _hitBufferList[i].normal;

                if (currentNormal.y > _minGroundNormalY)
                {
                    _isGrounded = true;

                    if (yMovement)
                    {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(_velocity, currentNormal);

                if (projection < 0)
                {
                    _velocity = _velocity - projection * currentNormal;
                }

                float modifiedDistance = _hitBufferList[i].distance - _shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        _rigidbody2D.position = _rigidbody2D.position + move.normalized * distance;
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}