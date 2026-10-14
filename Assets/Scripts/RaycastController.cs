using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class RaycastController : MonoBehaviour
{
    private InputAction _clickAction;
    private InputAction _moveAction;
    private InputAction _lookAction;

    //private CharacterController _characterController;
    private NavMeshAgent _agent;
    
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    [SerializeField] private float _movementSpeed = 10;

    void Awake()
    {
        _clickAction = InputSystem.actions["Attack"];
        _moveAction = InputSystem.actions["Move"];
        _lookAction = InputSystem.actions["MousePosition"];

        _agent = GetComponent<NavMeshAgent>();
    }

    
    void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();

       // TopDownMovement();

       // Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
        if (_clickAction.WasPressedThisFrame())
        {
           // ShotRaycast();
            MoveToPoint();
        }
    }

    void ShotRaycast()
    {
        RaycastHit hit;

       /* if (Physics.Raycast(transform.position, transform.forward, out hit, 10 ));
        {
            Debug.Log(hit.transform.name);
        }*/
    }
    /*void TopDownMovement()
    {
        Vector3 moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y);

        _lookInput = _lookAction.ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(_lookInput);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 playerForward = hit.point - transform.position;
            playerForward.y = 0;
            transform.forward = playerForward;
        }

        if(moveDirection != Vector3.zero)
        {
            _characterController.Move(moveDirection * _movementSpeed * Time.deltaTime);
        }
    }*/

    void MoveToPoint()
    {
        _lookInput = _lookAction.ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(_lookInput);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            _agent.SetDestination(hit.point);
        }
    }
}
