using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    PlayerInputActions playerInputActions;

    [Header("Player Controlled Objects")]
    [SerializeField] PlayerMasterController playerMasterController = null;
    [Header("Aiming")]
    [SerializeField] bool useCurve = false;
    [SerializeField] AnimationCurve aimingStickSensitivityCurve;


    //fencing input storage
    float attackAxis;
    Vector2 aimStick;
    bool stopFencing;

    // moving around input storage
    Vector2 moveStick;
    Vector2 cameraStick;
    bool startFencing;


    private void Awake()
    {
        playerInputActions = new PlayerInputActions();

        if (playerMasterController == null)
            Debug.LogError("PlayerInput.cs : PlayerMasterController not found. Is the field assigned in the inspector?");
    }
    

    void Update()
    {
        #region read input
        // fencing
        attackAxis = playerInputActions.Fencing.Attack.ReadValue<float>();
        aimStick = playerInputActions.Fencing.Aim.ReadValue<Vector2>();

        stopFencing = playerInputActions.Fencing.StopFencing.triggered;


        // moving around
        moveStick = playerInputActions.MovingAround.Move.ReadValue<Vector2>();
        cameraStick = playerInputActions.MovingAround.Camera.ReadValue<Vector2>();

        startFencing = playerInputActions.MovingAround.StartFencing.triggered;

        #endregion


        #region process input
        // apply the curve to the aim input
        if (useCurve)
        {
            aimStick.x = Mathf.Sign(aimStick.x) * aimingStickSensitivityCurve.Evaluate(Mathf.Abs(aimStick.x));
            aimStick.y = Mathf.Sign(aimStick.y) * aimingStickSensitivityCurve.Evaluate(Mathf.Abs(aimStick.y));
        }
        #endregion


        // Send the input
        SendInput();
    }


    void SendInput()
    {
        if (playerMasterController.PlayerState == PlayerMasterController.PlayerControllerState.movingAround )
        {
            playerMasterController.MovingAround_ReceiveCameraInput(cameraStick);
            playerMasterController.MovingAround_ReceiveMoveInput(moveStick);
            playerMasterController.MovingAround_StartFencing(startFencing);

        }
        else if (playerMasterController.PlayerState == PlayerMasterController.PlayerControllerState.fencing)
        {
            playerMasterController.Fencing_ReceiveAimInput(aimStick);
            playerMasterController.Fencing_ReceiveAttackInput(attackAxis);
            playerMasterController.Fencing_StopFencing(stopFencing);
        }
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
