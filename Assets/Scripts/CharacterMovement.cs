using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private Animator _animator;

    private bool _isFacingRight = true;
    private Rigidbody2D _rigidBody2D;

    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float HorizontalMove = Input.GetAxis("Horizontal");
        _rigidBody2D.velocity = new Vector2(HorizontalMove * _speed, _rigidBody2D.velocity.y);
        _animator.SetFloat("HorizontalMove", Mathf.Abs(HorizontalMove));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rigidBody2D.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
        }

        if (HorizontalMove < 0 && _isFacingRight)
        {
            Flip();
        }
        else if (HorizontalMove > 0 && !_isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}