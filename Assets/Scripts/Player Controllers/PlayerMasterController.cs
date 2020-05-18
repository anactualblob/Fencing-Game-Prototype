﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FencingSubController), typeof(MovingAroundSubController))]
public class PlayerMasterController : MonoBehaviour
{
    FencingSubController fencingController;
    MovingAroundSubController movingAroundController;

    SubController activeSubController;

    List<SubController> subControllers = new List<SubController>();

    [Tooltip("Main CameraRig, to be handled by the SubControllers.")]
    [SerializeField] CameraRig mainCameraRig;



    /// <summary>
    /// This enum describes the different controller states the player can be in. Each state corresponds to a SubController.
    /// <para>Adding another SubController requires adding the new state to this enum.</para>
    /// </summary>
    public enum PlayerControllerState
    {
        none,
        movingAround,
        fencing
    }

    PlayerControllerState playerState = PlayerControllerState.none;

    /// <summary>
    /// State the player is currently in. Determines which SubController to use.
    /// <para>Setting this property will change the activeSubController.</para>
    /// </summary>
    public PlayerControllerState PlayerState
    {
        get { return playerState; }
        set
        {
            // return if we're assigning the same value.
            if (playerState == value) return;

            // if activeSubController is assigned, set its state to inactive
            //   also, nullify the cameraRig variable on the activeSubController.
            if (activeSubController != null)
            {
                activeSubController.State = SubController.SubControllerState.inactive;
                activeSubController.cameraRig = null;
            }

            // assign the new activeSubController according to the value we're setting PlayerState to
            switch (value)
            {
                case PlayerControllerState.none:
                    break;

                case PlayerControllerState.movingAround:
                    activeSubController = movingAroundController;
                    break;

                case PlayerControllerState.fencing:
                    activeSubController = fencingController;
                    break;

                default:
                    Debug.LogWarning("PlayerMasterController.cs : setting PlayerState property to '" + value + "'. This case has not been accounted for.");
                    break;
            }

            // set the new activeSubController's state to active and pass the cameraRig to it
            activeSubController.State = SubController.SubControllerState.active;
            activeSubController.cameraRig = mainCameraRig;

            // finally assign value to playerState
            playerState = value;
        }
    }






    private void Awake()
    {
        fencingController = GetComponent<FencingSubController>();
        movingAroundController = GetComponent<MovingAroundSubController>();

        if (mainCameraRig == null) Debug.LogError("PlayerMasterController.cs : mainCameraRig is null. Has it been assigned in the inspector ?");
    }

    void Start()
    {
        PlayerState = PlayerControllerState.movingAround;
    }




    void Update()
    {
        activeSubController.ActiveSubControllerUpdate();

    }






    #region Input Receiving
    // FencingController input
    public void Fencing_ReceiveAttackInput(float attackValue)
    {
        fencingController.ReceiveAttackInput(attackValue);
    }

    public void Fencing_ReceiveAimInput(Vector2 aimVector)
    {
        fencingController.ReceiveAimInput(aimVector);
    }

    public void Fencing_StopFencing(bool shouldStopFencing)
    {
        if (shouldStopFencing) PlayerState = PlayerControllerState.movingAround;
    }


    // MovingAroundController input
    public void MovingAround_ReceiveMoveInput(Vector2 moveVector)
    {
        movingAroundController.ReceiveMoveInput(moveVector);
    }

    public void MovingAround_ReceiveCameraInput(Vector2 cameraVector)
    {
        movingAroundController.ReceiveCameraInput(cameraVector);
    }

    public void MovingAround_StartFencing(bool shouldStartFencing)
    {
        if (shouldStartFencing) PlayerState = PlayerControllerState.fencing;
    }
    #endregion
}
