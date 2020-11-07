#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif


using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FeatherController : MonoBehaviour
{
    class CameraState
    {
        public float yaw;
        public float pitch;
        public float roll;
        public float x;
        public float y;
        public float z;

        public void SetFromTransform(Transform t)
        {
            pitch = t.eulerAngles.x;
            yaw = t.eulerAngles.y;
            roll = t.eulerAngles.z;
            x = t.position.x;
            y = t.position.y;
            z = t.position.z;
        }

        public void Translate(Vector3 translation)
        {
            Vector3 rotatedTranslation = Quaternion.Euler(pitch, yaw, roll) * translation;

            x += rotatedTranslation.x;
            y += rotatedTranslation.y;
            z += rotatedTranslation.z;
        }

        public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
        {
            yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
            pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
            roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);

            x = Mathf.Lerp(x, target.x, positionLerpPct);
            y = Mathf.Lerp(y, target.y, positionLerpPct);
            z = Mathf.Lerp(z, target.z, positionLerpPct);
        }

        public void UpdateTransform(Transform t)
        {
            t.eulerAngles = new Vector3(pitch, yaw, roll);
            t.position = new Vector3(x, y, z);
        }
    }

    private CameraState targetCameraState = new CameraState();
    private CameraState interpolatingCameraState = new CameraState();
    private bool blowed = false;
    public GameObject TargetObject;
    public float TargetDistance = 25;
    public double moveSpeed = 1.0f;

    [Header("Movement Settings")]
    [Tooltip("Exponential boost factor on translation, controllable by mouse wheel.")]
    public float boost = 3.5f;

    [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 1f)]
    public float positionLerpTime = 0.2f;

    [Header("Rotation Settings")]
    [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

    [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
    public float rotationLerpTime = 0.01f;

    [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
    public bool invertY = false;

#if ENABLE_INPUT_SYSTEM
        InputAction movementAction;
        InputAction verticalMovementAction;
        InputAction lookAction;
        InputAction boostFactorAction;
        bool        mouseRightButtonPressed;

        void Start()
        {
            var map = new InputActionMap("Simple Camera Controller");

            lookAction = map.AddAction("look", binding: "<Mouse>/delta");
            movementAction = map.AddAction("move", binding: "<Gamepad>/leftStick");
            verticalMovementAction = map.AddAction("Vertical Movement");
            boostFactorAction = map.AddAction("Boost Factor", binding: "<Mouse>/scroll");

            lookAction.AddBinding("<Gamepad>/rightStick").WithProcessor("scaleVector2(x=15, y=15)");
            movementAction.AddCompositeBinding("Dpad")
                .With("Up", "<Keyboard>/w")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/s")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/a")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/d")
                .With("Right", "<Keyboard>/rightArrow");
            verticalMovementAction.AddCompositeBinding("Dpad")
                .With("Up", "<Keyboard>/pageUp")
                .With("Down", "<Keyboard>/pageDown")
                .With("Up", "<Keyboard>/e")
                .With("Down", "<Keyboard>/q")
                .With("Up", "<Gamepad>/rightshoulder")
                .With("Down", "<Gamepad>/leftshoulder");
            boostFactorAction.AddBinding("<Gamepad>/Dpad").WithProcessor("scaleVector2(x=1, y=4)");

            movementAction.Enable();
            lookAction.Enable();
            verticalMovementAction.Enable();
            boostFactorAction.Enable();
        }
#endif

    void OnEnable()
    {
        targetCameraState.SetFromTransform(transform);
        interpolatingCameraState.SetFromTransform(transform);
    }

    // Update is called once per frame
    void Update()
    {

        UnityEngine.Debug.DrawLine(transform.position, transform.position + GetRotationVector(), new Color(0, 255, 0));
        UnityEngine.Debug.DrawLine(transform.position, transform.position + GetCamViewDirection(), new Color(255, 0, 0));
        UnityEngine.Debug.DrawLine(transform.position, transform.position + GetDistanceVectorToTargetObject(), new Color(0, 0, 255));

        // Hide and lock cursor when right mouse button pressed
        if (IsRightMouseButtonDown())
        {
            Cursor.lockState = CursorLockMode.Locked;
            var mouseMovement = GetInputLookRotation() * Time.deltaTime * 5;
            if (invertY)
                mouseMovement.y = -mouseMovement.y;

            var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

            targetCameraState.yaw += mouseMovement.x * mouseSensitivityFactor;
            targetCameraState.pitch += mouseMovement.y * mouseSensitivityFactor;

            transform.RotateAround(TargetObject.transform.position, Vector3.up, mouseMovement.x * mouseSensitivityFactor);
            transform.RotateAround(TargetObject.transform.position, Vector3.right, mouseMovement.y * mouseSensitivityFactor);

            //transform.RotateAround(TargetObject.transform.position, Vector3.up, 20 * Time.deltaTime);

        }

        if (Math.Abs(GetDistanceVectorToTargetObject().magnitude - TargetDistance) > Math.Abs(0.05f)) 
        {
            transform.Translate(GetDistanceVectorToTargetObject() * (GetDistanceVectorToTargetObject().magnitude - TargetDistance) * Time.deltaTime);
        }

        if (GetRotationVector().normalized != GetDistanceVectorToTargetObject().normalized)
        {
            transform.rotation = Quaternion.LookRotation(GetDistanceVectorToTargetObject());

        }

        if (IsRightMouseButtonUp())
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }

#if ENABLE_INPUT_SYSTEM
            if(Mouse.current.leftButton.isPressed && !blowed) 
            {
                blowed = true;
                TargetObject.transform.Translate(GetDistanceVectorToTargetObject() * Time.deltaTime * 3.0f);
            } else if(!Mouse.current.leftButton.isPressed)
            {
                blowed = false;
            }
#else
        if (Input.GetMouseButtonDown(0))
        {

            TargetObject.transform.Translate(GetCamViewDirection() * Time.deltaTime * 3.0f);
        }
        else if(!Mouse.current.leftButton.isPressed)
        {
            blowed = false;
        }
#endif

    }

    Vector3 GetRotationVector() 
    {
        //return this.GetDistanceVectorToTargetObject() - GetCamViewDirection();
        return GetCamViewDirection() - this.GetDistanceVectorToTargetObject();
    }

    Vector3 GetCamViewDirection() 
    {
        return transform.InverseTransformDirection(Vector3.forward);
    }

    Vector3 GetDistanceVectorToTargetObject()
    {
        return this.TargetObject.transform.position - this.transform.position;
    }

    Vector3 GetInputTranslationDirection()
    {
        Vector3 direction = new Vector3(
            this.TargetObject.transform.position.x - this.transform.position.x,
            this.TargetObject.transform.position.y - this.transform.position.y,
            this.TargetObject.transform.position.z - this.transform.position.z
        );
        if (Math.Abs(direction.magnitude - this.TargetDistance) > Math.Abs(1.0)) {
            return direction * (direction.magnitude - this.TargetDistance);
        }

        return Vector3.zero;
    }

    Vector2 GetInputLookRotation()
    {
#if ENABLE_INPUT_SYSTEM
            return lookAction.ReadValue<Vector2>();
#else
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * 10;
#endif
    }

    bool IsRightMouseButtonDown()
    {
#if ENABLE_INPUT_SYSTEM
            return Mouse.current != null ? Mouse.current.rightButton.isPressed : false;
#else
        return Input.GetMouseButtonDown(1);
#endif
    }

    bool IsRightMouseButtonUp()
    {
#if ENABLE_INPUT_SYSTEM
            return Mouse.current != null ? !Mouse.current.rightButton.isPressed : false;
#else
        return Input.GetMouseButtonUp(1);
#endif
    }
}
