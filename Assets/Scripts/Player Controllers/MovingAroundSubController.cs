using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAroundSubController : SubController
{
    Vector3 movement = Vector3.zero;
    [SerializeField] float movementSpeed = 1.0f;

    public override void OnSubControllerActivate()
    {
    }

    public override void OnSubControllerDeactivate()
    {
    }



    public override void ActiveSubControllerUpdate()
    {
        transform.LookAt(transform.position + movement, Vector3.up);
        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
    }




    public void ReceiveMoveInput(Vector2 moveVector)
    {
        // TODO : make relative to camera
        movement.x = moveVector.x;
        movement.z = moveVector.y;
    }

    public void ReceiveCameraInput(Vector2 cameraVector)
    {
        //throw new NotImplementedException();
    }
}
