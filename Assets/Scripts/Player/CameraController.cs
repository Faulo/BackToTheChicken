using Runtime.Physics;
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

        [Header("Wind controls")]
        [SerializeField]
        bool invertX = false;
        [SerializeField]
        bool invertY = false;
        [SerializeField]
        CinemachineAxisInput axisInput = default;
        [SerializeField]
        WindSource wind = default;



        InputAction lookAction;
        InputAction scrollAction;
        InputAction middleButtonAction;

        InputAction leftStickAction;
        InputAction rightStickAction;

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

            leftStickAction = map.AddAction("leftStick", binding: "<Gamepad>/leftStick");
            rightStickAction = map.AddAction("rightStick", binding: "<Gamepad>/rightStick");

            lookAction.Enable();
            scrollAction.Enable();
            middleButtonAction.Enable();

            leftStickAction.Enable();
            rightStickAction.Enable();
        }

        void Update() {
            Vector2 look;
            Vector2 direction;
            if (Cursor.lockState == CursorLockMode.Locked) {
                look = lookAction.ReadValue<Vector2>();
                if (leftButtonDown) {
                    direction = Vector2.up;
                } else if (rightButtonDown) {
                    direction = Vector2.down;
                } else {
                    direction = Vector2.zero;
                }
            } else {
                look = rightStickAction.ReadValue<Vector2>();
                direction = leftStickAction.ReadValue<Vector2>();
            }
            if (invertX) {
                look.x *= -1;
            }
            if (invertY) {
                look.y *= -1;
            }
            direction = Vector2.ClampMagnitude(direction, 1);
            if (axisInput) {
                axisInput.input = look;
            }
            if (wind) {
                wind.direction = direction;
                wind.strength = direction.magnitude;
            }
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