using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ShipController : MonoBehaviour
{
    public Transform camera;
    public float forwardForce = 10;
    public float translationalForce = 5f;
    public float rotateForce = 2f;
    public float cameraFollowStrength = 10f;
    public float cameraFollowDamping = 20f;
    public Vector3 centerOfMass;
    
    private Rigidbody _rigidbody;
    private InputAction _horizontalMoveAction;
    private InputAction _verticalMoveAction;
    private InputAction _rollAction;
    private InputAction _yawAction;
    private InputAction _pitchAction;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        // Allow for center of mass correction based on the model and collider
        // TODO: It's probably better to fix the model somehow? I don't know how though :)
        if (centerOfMass != Vector3.zero)
            _rigidbody.centerOfMass = centerOfMass;
        
        ObjectResetter.RegisterObject(gameObject);
        
        _horizontalMoveAction = InputSystem.actions.FindAction("HorizontalMove");
        _verticalMoveAction = InputSystem.actions.FindAction("VerticalMove");
        _rollAction = InputSystem.actions.FindAction("Roll");
        _yawAction = InputSystem.actions.FindAction("Yaw");
        _pitchAction = InputSystem.actions.FindAction("Pitch");
    }

    private void Move()
    {
        // TODO: combine horizontal and vertical movement into one Vector3 input
        // Horizontal Movement
        Vector2 horizontalMoveValue = _horizontalMoveAction.ReadValue<Vector2>();
        _rigidbody.AddForce(transform.forward * (horizontalMoveValue.y * forwardForce));
        _rigidbody.AddForce(transform.right * (horizontalMoveValue.x * translationalForce));
        
        // Vertical Movement
        float verticalMoveValue = _verticalMoveAction.ReadValue<float>();
        _rigidbody.AddForce(transform.up * (verticalMoveValue * translationalForce));

        // Roll
        // float rollValue = _rollAction.ReadValue<float>();
        // _rigidbody.AddTorque(transform.forward * (rollValue * rotateForce));
        
        // Yaw
        Vector3 rotationVector = _rigidbody.rotation * transform.up;
        float yawValue = _yawAction.ReadValue<float>();
        _rigidbody.AddTorque(rotationVector * (yawValue * rotateForce));
        
        // Pitch
        // float pitchValue = _pitchAction.ReadValue<float>();
        // _rigidbody.AddTorque(transform.right * (pitchValue * rotateForce));
        
        // Vector3 localAngularVelocity = transform.InverseTransformDirection(_rigidbody.angularVelocity);
        
        // // Pitch/Yaw Camera Follow
        // Vector3 angleDiff = Vector3.Cross(transform.forward, camera.forward);
        // Vector3 angularVelocityNoRoll =
        //     transform.TransformDirection(new Vector3(localAngularVelocity.x, localAngularVelocity.y, 0));
        // _rigidbody.AddTorque(cameraFollowStrength * angleDiff - cameraFollowDamping * angularVelocityNoRoll);
        
        // Roll Camera Follow
        // Vector3 angleDiff = Vector3.Cross(transform.up, camera.up); 
        // Vector3 rollVelocity = transform.TransformDirection(new Vector3(0, 0, localAngularVelocity.z));
        // _rigidbody.AddTorque(cameraFollowStrength * angleDiff - cameraFollowDamping * rollVelocity);
    }

    void FixedUpdate()
    {
        Move();
    }
}
