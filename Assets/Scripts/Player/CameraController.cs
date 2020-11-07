using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Player {
    public class CameraController : MonoBehaviour {
        [SerializeField]
        GameObject targetObject;
        [SerializeField, Range(0, 10)]
        float targetDistance = 4;
        [SerializeField, Range(0, 10)]
        float movementDuration = 1;

        Vector3 targetPosition;
        Quaternion targetRotation;
        Vector3 currentVelocity;

        void UpdateTarget() {
            var direction = (transform.position - targetObject.transform.position).normalized * targetDistance;
            targetPosition = targetObject.transform.position + direction;
            targetRotation = Quaternion.LookRotation(-direction, Vector3.up);

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, movementDuration);
            transform.rotation = targetRotation;
        }

        [Header("Mouse controls")]
        [SerializeField, Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
        bool invertY = false;
        [SerializeField]
        CinemachineAxisInput axisInput = default;



        InputAction lookAction;
        InputAction scrollAction;
        InputAction middleButtonAction;

        void Awake() {
            OnValidate();
        }
        void OnValidate() {
            if (!axisInput) {
                TryGetComponent(out axisInput);
            }
        }

        void Start() {
            var map = new InputActionMap("Simple Camera Controller");
            lookAction = map.AddAction("look", binding: "<Mouse>/delta");
            scrollAction = map.AddAction("scroll", binding: "<Mouse>/scroll");
            middleButtonAction = map.AddAction("middleButton", binding: "<Mouse>/middleButton");
            middleButtonAction.performed += OnMidleClick;

            lookAction.Enable();
            scrollAction.Enable();
            middleButtonAction.Enable();
        }

        void Update() {
            axisInput.input = Cursor.lockState == CursorLockMode.Locked
                ? lookAction.ReadValue<Vector2>()
                : Vector2.zero;
        }

        void OnMidleClick(InputAction.CallbackContext context) {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked
                ? CursorLockMode.None
                : CursorLockMode.Locked;
        }

        bool leftButtonDown => Mouse.current != null
            ? Mouse.current.leftButton.isPressed
            : false;
        bool rightButtonDown => Mouse.current != null
            ? Mouse.current.rightButton.isPressed
            : false;
    }
}