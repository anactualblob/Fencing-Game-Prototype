using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAroundSubController : SubController
{
    Vector3 movement = Vector3.zero;
    [Header("Movement Parameters")]
    [SerializeField] float movementSpeed = 1.0f;

    [Header("Camera Parameters")]
    [Tooltip("Distance of the camera from the rig's pivot.")]
    [SerializeField] Vector3 cameraOffsetFromPivot = Vector3.zero;
    [Tooltip("Offset of the camera rig's pivot position from the position of the character on the y axis.")]
    [SerializeField] float rigHeight = 1.0f;
    [Space]
    [Range(0,90)]
    [SerializeField] float maxLookAngleUp = 0.0f;
    [Range(0,90)]
    [SerializeField] float maxLookAngleDown = 0.0f;

    float cameraPitch;
    float cameraYaw;

    Vector3 rigRot;
    Vector3 rigPos;

    #region Activation/Deactivation
    public override void OnSubControllerActivate()
    {
        
    }

    public override void OnSubControllerActivationFailed(ActivationFailedException e) { }

    public override void OnSubControllerDeactivate()
    {

    }

    public override void OnSubControllerDeactivationFailed(DeactivationFailedException e) { }
    #endregion


    public override void ActiveSubControllerUpdate()
    {
        transform.LookAt(transform.position + movement, Vector3.up);
        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
    }




    public void ReceiveMoveInput(Vector2 moveVector)
    {
        // uses the properties horizontalRight and horizontalForward of the cameraRig to make the 
        //   movement Vector relative to the camera but parallel to the horizontal xz plane.
        movement = moveVector.x * cameraRig.horizontalRight + moveVector.y * cameraRig.horizontalForward;
    }

    public void ReceiveCameraInput(Vector2 cameraVector)
    {
        cameraPitch += cameraVector.y;
        cameraYaw += cameraVector.x;

        cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngleUp, maxLookAngleDown);

        rigRot = new Vector3(cameraPitch, cameraYaw, 0);
        rigPos = transform.position + Vector3.up * rigHeight;


        cameraRig.SetCameraRigState(rigPos, rigRot, cameraOffsetFromPivot);
    }
}
