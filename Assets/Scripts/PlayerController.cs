using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController _characterController;

    private InputAction _moveAction;
    private Vector2 _moveInput;
    private InputAction _jumpAction;

    [SerializeField] private float _movementSpeed = 10;
    [SerializeField] private float _jumpHeight = 2;
    private float _turnSmoothVelocity;
    [SerializeField] private float _smoothTime = 1;

    private float _gravity;
    [SerializeField] private Vector3 _playerGravity;

    [SerializeField] private Transform _sensorTransform;
    [SerializeField] private float _sensorRadius;
    [SerializeField] private LayerMask _groundLayer;

    private Transform _cameraTransform;
    void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _moveAction = InputSystem.actions["Move"];
        _jumpAction = InputSystem.actions["Jump"];

        _cameraTransform = Camera.main.transform;               
    }
    void Start()
    {
        _gravity = Physics.gravity.y;   
    }

   
    void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();

        TPMovement();

        Gravity();

        if (_jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            Jump();
        }
    }

    void Movement()
    {
        Vector3 moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y);

        if (moveDirection != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _smoothTime);

            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            _characterController.Move(moveDirection * _movementSpeed * Time.deltaTime);
        }
    }
    void TPMovement()
    {
        Vector3 direction = new Vector3(_moveInput.x, 0, _moveInput.y);

        if (direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _smoothTime);

            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            _characterController.Move(moveDirection * _movementSpeed * Time.deltaTime);
        }
    }

    void Gravity()
    {
        if (!IsGrounded())
        {
            _playerGravity.y += _gravity * Time.deltaTime;
        }
        else if (IsGrounded() && _playerGravity.y < 0)
        {
            _playerGravity.y = _gravity;
        }

        _characterController.Move(_playerGravity * Time.deltaTime);
        
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(_sensorTransform.position, _sensorRadius, _groundLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_sensorTransform.position, _sensorRadius);
    }

    void Jump()
    {
        _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
    }
}
