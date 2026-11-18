using UnityEngine;
using UnityEngine.InputSystem; //Acordarse de esto

public class PC : MonoBehaviour
{
    /*Descargar Cinemachine (free look camera),Rellenar los apartados, Camera(Poner el Target), Axis(Colocar en Orbit Look) y El Gain en -si va del reves
    tambien es la sensibilidad, Cambiar Orbital Follow para acomodar la camara.
    Layer de suelo al Suelo, Componente Character Controller al personaje, Ground Sensor. [Aim Movement] */

    private CharacterController _controller;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private Vector2 _moveInput;

    [SerializeField] private float _movementSpeed = 10;
    [SerializeField] private float _jumpHeight = 2;

    private float _gravity;
    [SerializeField] private Vector3 _playerGravity;

    //Variables Ground Sensor
    [SerializeField] private Transform _sensorTransform;
    [SerializeField] private float _sensorRadius;
    [SerializeField] private LayerMask _groundLayer;

    private Animator _animator; // Activa animaciones ( 2D Freeform Directional, Horizontal, Vertical(float), Nombre + Loop time(menos salto))
    private Transform _cameraTransform;

    private float _turnSmoothVelocity; //Hace que no sea brusco el cambio
    [SerializeField] float _smoothTime = 1;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();

        _moveAction = InputSystem.actions["Move"];
        _jumpAction = InputSystem.actions["Jump"];

        _cameraTransform = Camera.main.transform;
    }
    void Start()
    {
        _gravity = Physics.gravity.y;
    }

    // Update is called once per frame
    void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>(); //Movimiento

        Gravity();

        if(_jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            Jump();
        }
        _animator.SetBool("IsJumping", !IsGrounded()); //Para la animacion de salto que se reproduzca.

        AimMovement();
        
    }
    void AimMovement()
    {
        Vector3 direction = new Vector3(_moveInput.x, 0, _moveInput.y);

        _animator.SetFloat("Horizontal", _moveInput.x, 0.2f, Time.deltaTime);  //Es para las animaciones con la rotacion adecuada.
        _animator.SetFloat("Vertical", _moveInput.y,  0.2f, Time.deltaTime);

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _cameraTransform.eulerAngles.y, ref _turnSmoothVelocity, _smoothTime);

        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

        if(direction != Vector3.zero)
        {
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            _controller.Move(moveDirection * _movementSpeed * Time.deltaTime);
        }
    }
    void Gravity()
    {
        if(!IsGrounded())
        {
            _playerGravity.y += _gravity * Time.deltaTime;
        }
        else if(IsGrounded() && _playerGravity.y < 0)
        {
            _playerGravity.y = _gravity;
        }
         _controller.Move(_playerGravity * Time.deltaTime);
    }
    bool IsGrounded()
    {
        return Physics.CheckSphere(_sensorTransform.position, _sensorRadius, _groundLayer);
    }
    void Jump()
    {
        _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
    }
}
