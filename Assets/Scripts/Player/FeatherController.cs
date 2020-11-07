#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif


using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FeatherController : MonoBehaviour
{
    private bool blowed = false;
    private Vector3 nextBlowForce = Vector3.zero;
    public GameObject TargetObject;
    public Rigidbody BlowRigidbody;
    public float TargetDistance = 4;
    public float MoveSpeed = 1.0f;

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

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(TargetObject.transform.position, TargetDistance);
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
            Vector3 mouseMovement = GetInputLookRotation() * Time.deltaTime * 5;
            if (invertY)
            {
                mouseMovement.y = -mouseMovement.y;
            }

            float mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

            if (Math.Abs(mouseMovement.x) > 0.1f)
            {
                Vector3 rotVec = Vector3.up;
                rotVec = Quaternion.AngleAxis(transform.rotation.eulerAngles.x, Vector3.right) * rotVec;
                transform.RotateAround(TargetObject.transform.position, rotVec, mouseMovement.x * mouseSensitivityFactor);
            }

            if (Math.Abs(mouseMovement.y) > 0.1f)
            {
                Vector3 rotVec = Vector3.right;
                rotVec = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * rotVec;
                transform.RotateAround(TargetObject.transform.position, rotVec, mouseMovement.y * mouseSensitivityFactor);
            }

            transform.Rotate(0, 0, -transform.eulerAngles.z);
        }

        //UnityEngine.Debug.Log(GetDistanceVectorToTargetObject().magnitude);

        //transform.position = TargetObject.transform.position - (GetDistanceVectorToTargetObject().normalized * 4.0f);

        if (IsRightMouseButtonUp())
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void blowRoutine() 
    {
#if ENABLE_INPUT_SYSTEM
            if(Mouse.current.leftButton.isPressed && !blowed) 
            {
                // TODO add blow mechanic with wind force
            } else if(!Mouse.current.leftButton.isPressed)
            {
                // TODO add blow mechanic with wind force
            }
#else
        if (Input.GetMouseButtonDown(0))
        {
            // TODO add blow mechanic with wind force

        }
        else if (!Mouse.current.leftButton.isPressed)
        {
            // TODO add blow mechanic with wind force
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

