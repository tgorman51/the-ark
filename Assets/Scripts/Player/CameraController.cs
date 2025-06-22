using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public float lookSensitivity = 25f;
    public float minDistance = 3f;
    public float maxDistance = 5f;
    public float scrollSensitivity = 0.2f;
    public Vector3 offset;
    public float obstacleCorrectionOffset = 0.1f;
    
    private float _pitch = 0f;
    private float _yaw = 0f;
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
        _scrollAction = InputSystem.actions.FindAction("Scroll");
        
        _pitch = transform.eulerAngles.x;
        _yaw = transform.eulerAngles.y;

        float zOffset = (minDistance + maxDistance) / 2f;
        offset = new Vector3(offset.x, offset.y, -zOffset);
    }
    
    private void UpdatePitchYaw()
    {
        Vector2 lookValue = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * lookSensitivity;
        _yaw += lookValue.x;
        _yaw = (_yaw + 360f) % 360f;  // keep _yaw from growing indefinitely
        _pitch = Mathf.Clamp(_pitch - lookValue.y, -90f, 90f);
    }

    private void HandleObstructions()
    {
        Vector3 targetPosition = target.transform.position;
        Ray ray = new Ray(targetPosition, transform.position - targetPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Vector3.Distance(targetPosition, transform.position), ~0, QueryTriggerInteraction.Ignore)) {
            Vector3 moveTowardPosition = Vector3.Lerp(hit.point, targetPosition, obstacleCorrectionOffset);
            transform.position = moveTowardPosition;
        }
    }

    private void Look()
    {
        Vector2 scrollInput = _scrollAction.ReadValue<Vector2>();
        offset = new Vector3(
            offset.x,
            offset.y,
            Mathf.Clamp(offset.z + (scrollInput.y * scrollSensitivity), -maxDistance, -minDistance)
        );
        
        UpdatePitchYaw();
        
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
        Vector3 position = rotation * offset + target.transform.position;
        
        transform.position = position;
        transform.rotation = rotation;

        HandleObstructions();
    }

    private void LateUpdate()
    {
        Look();
    }
}
