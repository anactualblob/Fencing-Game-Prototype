using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    PlayerInputActions playerInputActions;

    [Header("Player Controlled Objects")]
    [SerializeField] FencingController fencingController = null;
    [Header("Aiming")]
    [SerializeField] bool useCurve = false;
    [SerializeField] AnimationCurve aimingStickSensitivityCurve;



    public float attackAxis;
    Vector2 aimStick;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();

        if (fencingController == null)
            Debug.LogError("PlayerInput.cs : FencingController not found. Is the field assigned in the inspector?");
    }
    

    void Update()
    {
        // read input
        attackAxis = playerInputActions.Fencing.Attack.ReadValue<float>();
        aimStick = playerInputActions.Fencing.Aim.ReadValue<Vector2>();


        // process input if necessary
        if (useCurve)
        {
            aimStick.x = Mathf.Sign(aimStick.x) * aimingStickSensitivityCurve.Evaluate(Mathf.Abs(aimStick.x));
            aimStick.y = Mathf.Sign(aimStick.y) * aimingStickSensitivityCurve.Evaluate(Mathf.Abs(aimStick.y));
        }

        // Send the input
        SendInput();
    }


    void SendInput()
    {
        fencingController.ReceiveAttackInput(attackAxis);
        fencingController.ReceiveAimInput(aimStick);
    }


    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

}
