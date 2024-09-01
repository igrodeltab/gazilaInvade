using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Rigidbody2D _playerRigidbody;
    [SerializeField] private Animator _playerAnimator;

    private Vector2 _movement;

    // Update is called once per frame
    private void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        _playerAnimator.SetFloat("Horizontal", _movement.x);
        _playerAnimator.SetFloat("Vertical", _movement.y);
        _playerAnimator.SetFloat("Speed", _movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        _playerRigidbody.MovePosition(_playerRigidbody.position + _movement * _moveSpeed * Time.fixedDeltaTime);
    }
}
