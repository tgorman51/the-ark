using UnityEngine;
using UnityEngine.InputSystem;

public class RefCameraController : MonoBehaviour
{
    public GameObject target;
    public float lookSensitivity = 25f;
    public Vector3 firstPersonOffset;
    public Vector3 thirdPersonOffset;
    public float thirdPersonMinDistance = 3f;
    public float thirdPersonMaxDistance = 5f;
    public float scrollSensitivity = 0.2f;
    public bool thirdPerson = true;
    // public float smoothTime = 0.05f;
    
    private float _pitch = 0;
    private float _yaw = 0;
    // private Vector2 _smoothedLook;
    // private Vector2 _lookVelocity;

    // private InputAction _lookAction;
    private InputAction _cameraViewAction;
    private InputAction _scrollAction;

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
        // _lookAction = InputSystem.actions.FindAction("Look");
        _scrollAction = InputSystem.actions.FindAction("Scroll");
        
        // Toggle third person camera
        _cameraViewAction = InputSystem.actions.FindAction("CameraView");
        if (_cameraViewAction != null) _cameraViewAction.performed += _ => thirdPerson = !thirdPerson;
    }

    private void UpdatePitchYaw()
    {
        // Vector2 rawLook = _lookAction.ReadValue<Vector2>() * lookSensitivity;
        // Vector2 lookValue = _smoothedLook = Vector2.SmoothDamp(_smoothedLook, rawLook, ref _lookVelocity, smoothTime);
        
        Vector2 lookValue = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * lookSensitivity;
        _yaw += lookValue.x;
        _yaw = (_yaw + 360f) % 360f;  // keep _yaw from growing indefinitely
        _pitch = Mathf.Clamp(_pitch - lookValue.y, -90f, 90f);
    }
    
    private void FirstPersonLook()
    {
        // Set Camera Position
        Vector3 localOffset =
            transform.right * firstPersonOffset.x + transform.up * firstPersonOffset.y + transform.forward * firstPersonOffset.z;
        transform.position = target.transform.position + localOffset;
        
        // Set Camera Rotation
        UpdatePitchYaw();

        Quaternion pitchRotation = Quaternion.AngleAxis(_pitch, Vector3.right);
        Quaternion yawRotation = Quaternion.AngleAxis(_yaw, Vector3.up);

        transform.rotation = yawRotation * pitchRotation;
    }

    private void ThirdPersonLook()
    {
        Vector2 scrollInput = _scrollAction.ReadValue<Vector2>();
        thirdPersonOffset = new Vector3(
            thirdPersonOffset.x,
            thirdPersonOffset.y,
            Mathf.Clamp(thirdPersonOffset.z + (scrollInput.y * scrollSensitivity), -thirdPersonMaxDistance, -thirdPersonMinDistance)
            );
        
        UpdatePitchYaw();
        
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
        Vector3 position = rotation * thirdPersonOffset + target.transform.position;
        
        transform.position = position;
        transform.rotation = rotation;
    }

    void LateUpdate()
    {
        if (thirdPerson) ThirdPersonLook();
        else FirstPersonLook();
    }
}
