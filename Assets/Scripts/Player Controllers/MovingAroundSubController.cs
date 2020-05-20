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
    [SerializeField] float cameraDistanceFromPivot = 1.0f;
    [Tooltip("Offset of the camera rig's pivot position from the position of the character.")]
    [SerializeField] Vector3 rigOffsetFromPosition = Vector3.zero;
    [Space]
    [Range(0,90)]
    [SerializeField] float maxLookAngleUp = 0.0f;
    [Range(0,90)]
    [SerializeField] float maxLookAngleDown = 0.0f;

    float cameraPitch;
    float cameraYaw;


    public override void OnSubControllerActivate()
    {
        
    }

    public override void OnSubControllerActivationFailed(ActivationFailedException e) { }

    public override void OnSubControllerDeactivate()
    {
    }

    public override void OnSubControllerDeactivationFailed(DeactivationFailedException e) { }



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

        Vector3 rigRot = new Vector3(cameraPitch, cameraYaw, 0);
        Vector3 rigPos = transform.position + rigOffsetFromPosition;

        cameraRig.SetCameraRigState(rigPos, rigRot, Vector3.back * cameraDistanceFromPivot);
    }
}
