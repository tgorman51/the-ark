using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera camera;
    public bool rotateWithCamera = true;
    public float groundSpeed = 2000f;
    public float airSpeed = 0.5f;
    public float sprintMultiplier = 2f;
    public float crouchMultiplier = 0.4f;
    public float groundDrag = 5f;
    public float airDrag = 0f;
    public float groundCheckRadius = 0.6f;
    public float groundCheckDistance = 0.1f;
    
    [Header("Jumping")]
    public float jumpUpPercent = 0.6f;
    public float jumpMultiplier = 8f;
    public float jumpCheckRadius = 0.6f;
    public float maxGroundAngle = 45f;
    
    [Header("Wall Jumping")]
    public bool enableWallJump = true;
    public float wallJumpMultiplier = 200f;
    
    private Rigidbody _rigidbody;
    private float _movementX;
    private float _movementY;
    private float _speed;
    
    private bool _sprinting = false;
    private bool _crouching = false;
    
    private InputAction _moveAction;
    private InputAction _sprintAction;
    private InputAction _jumpAction;
    private InputAction _crouchAction;

    private Vector3 _debugGroundCheckSpherePosition;


    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        _moveAction = InputSystem.actions.FindAction("Move");

        _sprintAction = InputSystem.actions.FindAction("Sprint");
        _sprintAction.performed += _ => _sprinting = true;
        _sprintAction.canceled += _ => _sprinting = false;
        
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _jumpAction.performed += Jump;
        
        _crouchAction = InputSystem.actions.FindAction("Crouch");
        _crouchAction.performed += _ => _crouching = !_crouching;
    }
    
    private void Move()
    {
        Vector2 moveValue = _moveAction.ReadValue<Vector2>();
        _movementX = moveValue.x;
        _movementY = moveValue.y;
        
        float currentSpeed = _speed;
        if (_sprinting)
        {
            _crouching = false;
            if (IsGrounded())  // Disallow sprinting while airborne
                currentSpeed = _speed * sprintMultiplier;
        } else if (_crouching)
        {
            currentSpeed = _speed * crouchMultiplier;
        }
        Vector3 movement = (transform.forward * _movementY + transform.right * _movementX).normalized * (currentSpeed * Time.fixedDeltaTime);
        // _rigidbody.MovePosition(_rigidbody.position + movement);
        _rigidbody.AddForce(movement, ForceMode.Force);
        
        // Rotate to face camera direction if player is moving
        if (rotateWithCamera && (_movementX != 0 || _movementY != 0))
        {
            Vector3 forward = camera.transform.forward;
            if (forward == Vector3.up)
                forward = -camera.transform.up;
            if (forward == -Vector3.up)
                forward = camera.transform.up;
            forward.y = 0f;
            
            forward.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(forward);
            _rigidbody.MoveRotation(targetRotation);
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        Collider[] colliderCandidates = GetJumpableColliders();
        if (colliderCandidates.Length > 0)
        {
            foreach (Collider jumpCollider in colliderCandidates)
            {
                Vector3 surfaceNormal = GetNormal(jumpCollider);
                bool surfaceIsGround = Vector3.Angle(surfaceNormal, Vector3.up) < maxGroundAngle;
                if (!enableWallJump && !surfaceIsGround) return;  // No wall jumping if disabled
                
                if (IsGrounded() && !surfaceIsGround) continue;  // Disallow wall jumps while grounded
                if (!IsGrounded() && surfaceIsGround) continue;  // Disallow ground jumps while not grounded
                
                // Cancel y velocity for better wall jump *feel*
                // May change this later depending on *feel*
                if (!surfaceIsGround)
                    _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);
                
                // Calculate jump direction
                Vector2 moveValue = _moveAction.ReadValue<Vector2>();
                Vector3 moveDirection = (transform.forward * moveValue.y + transform.right * moveValue.x).normalized;
                Debug.DrawRay(transform.position, moveDirection, Color.blue, 5);
                Debug.DrawRay(transform.position, surfaceNormal, Color.white, 5);
                Vector3 jumpDirection = Vector3.Slerp(surfaceNormal, moveDirection, 0.5f);
                jumpDirection = Vector3.Slerp(jumpDirection, Vector3.up, jumpUpPercent).normalized;
                
                float multiplier = surfaceIsGround ? jumpMultiplier : wallJumpMultiplier;
                Debug.DrawRay(transform.position, jumpDirection * multiplier, Color.black, 5);
                _rigidbody.AddForce(jumpDirection * multiplier, ForceMode.Impulse);
                break;
            }
        }
    }
    
    private Collider[] GetJumpableColliders()
    {
        return Physics.OverlapSphere(GetCapsuleBottomSpherePosition(), jumpCheckRadius, 1 << LayerMask.NameToLayer("Ground"));
    }

    private Vector3 GetNormal(Collider target)
    {
        Vector3 raySourcePoint = GetCapsuleBottomSpherePosition();
        Vector3 direction = (target.ClosestPoint(raySourcePoint) - raySourcePoint).normalized;
        if (Physics.Raycast(raySourcePoint, direction, out RaycastHit rayHit))
        {
            return rayHit.normal;
        }

        Debug.LogWarning("Failed to find surface normal");
        return Vector3.zero;
    }

    private Vector3 GetCapsuleBottomSpherePosition()
    {
        return transform.position - Vector3.up * (transform.localScale.y / 2f);
    }

    private Vector3 GetCapsuleBottomPoint()
    {
        return transform.position - Vector3.up * (transform.localScale.y / 2f + transform.localScale.x / 2f);
    }
    
    private bool IsGrounded()
    {
        Vector3 sphereCenter = GetCapsuleBottomSpherePosition();
        float sphereY = sphereCenter.y - (transform.localScale.x / 2) + groundCheckRadius - groundCheckDistance;
        sphereCenter = _debugGroundCheckSpherePosition = new Vector3(sphereCenter.x, sphereY, sphereCenter.z);
        return Physics.CheckSphere(sphereCenter, groundCheckRadius, 1 << LayerMask.NameToLayer("Ground"));
    }

    private void AirborneUpdate()
    {
        if (IsGrounded())
        {
            _rigidbody.linearDamping = groundDrag;
            _speed = groundSpeed;
        }
        else
        {
            _rigidbody.linearDamping = airDrag;
            _speed = airSpeed;
        }
    }

    void Update()
    {
        AirborneUpdate();
    }

    void FixedUpdate()
    {
        Move();
    }

    void OnDrawGizmos()
    {
        Vector3 capsuleBottomSpherePosition = GetCapsuleBottomSpherePosition();
        
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_debugGroundCheckSpherePosition, groundCheckRadius);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(capsuleBottomSpherePosition, jumpCheckRadius);
    }
}
