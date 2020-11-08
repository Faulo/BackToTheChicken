using Cinemachine;
using Runtime.Effects;
using Runtime.Physics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Player {
    public class CameraController : MonoBehaviour {
        [Header("MonoBehaviour configuration")]
        [SerializeField]
        FeatherController targetObject = default;
        [SerializeField]
        CinemachineFreeLook cinemachine = default;

        [Header("Wind controls")]
        [SerializeField]
        bool invertX = false;
        [SerializeField]
        bool invertY = false;
        [SerializeField]
        CinemachineAxisInput axisInput = default;
        [SerializeField]
        WindSource wind = default;

        [Header("Feathers")]
        [SerializeField]
        EffectEvent onFeatherCollect = default;
        [SerializeField]
        int featherCount = 1;
        [SerializeField]
        int featherMaximum = 100;
        float featherRatio => (float)featherCount / featherMaximum;
        [SerializeField, Range(0, 10000)]
        float minimumForce = 100;
        [SerializeField, Range(0, 10000)]
        float maximumForce = 1000;
        [SerializeField, Range(0, 10)]
        float minimumRadius = 1;
        [SerializeField, Range(0, 10)]
        float maximumRadius = 10;
        [SerializeField]
        AnimationCurve windOverCount = new AnimationCurve();

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
            if (!cinemachine) {
                TryGetComponent(out cinemachine);
            }
            UpdateCamera();
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

            UpdateCamera();

            wind.onColliderEnter += (collider) => {
                if (collider.attachedRigidbody && collider.attachedRigidbody.TryGetComponent<FeatherController>(out var feather) && !feather.isMain) {
                    if (feather.SetTarget(targetObject)) {
                        featherCount++;
                        onFeatherCollect?.Invoke(gameObject);
                    }
                }
            };
        }

        void UpdateCamera() {
            if (cinemachine && targetObject) {
                cinemachine.Follow = targetObject.transform;
                cinemachine.LookAt = targetObject.transform;
            }
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
                wind.direction = new Vector3(direction.x, 0, direction.y);
                wind.strength = direction.magnitude;
                Debug.Log(windOverCount.Evaluate(featherCount));
                wind.force = minimumForce + (windOverCount.Evaluate(featherRatio) * (maximumForce - minimumForce));
                wind.radius = minimumRadius + (windOverCount.Evaluate(featherRatio) * (maximumRadius - minimumRadius));
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