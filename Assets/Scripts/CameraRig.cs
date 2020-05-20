using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    Camera rigCamera;

    /// <summary>
    /// Normalized Vector3 used for making movement relative to the camera. 
    /// <para>Points in the direction of CameraRig.transform.forward but is aligned with the horizontal xz plane.</para>
    /// </summary>
    public Vector3 horizontalForward
    {
        get
        {
            return new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        }
    }

    /// <summary>
    /// Normalized Vector3 used for making movement relative to the camera. 
    /// <para>Points in the direction of CameraRig.transform.right but is aligned with the horizontal xz plane.</para>
    /// </summary>
    public Vector3 horizontalRight
    {
        get
        {
            return new Vector3(transform.right.x, 0, transform.right.z).normalized;
        }
    }


    public float cameraAspect
    {
        get { return rigCamera.aspect; }
    }
    
    public float cameraFOV
    {
        get { return rigCamera.fieldOfView;  }
    }
    
    void Awake()
    {
        rigCamera = GetComponentInChildren<Camera>();
        if (rigCamera == null) Debug.LogError("CameraRig.cs : Camera component not found in children of this game object.");
        
    }

    void Update()
    {
        
    }


    /// <summary>
    /// Sets the state of the camera rig (position, rotation, camera offset from pivot).
    /// </summary>
    /// <param name="pivotPosition">Position of the pivot point of the rig (aka the parent game object).</param>
    /// <param name="pivotRotation">Rotation in euler angles of the pivot point of the rig (aka the parent game object).</param>
    /// <param name="cameraOffsetFromPivot">Offset of the camera from the pivot point of the rig (the local position of the camera relative to its parent).</param>
    public void SetCameraRigState(Vector3 pivotPosition, Vector3 pivotRotation, Vector3 cameraOffsetFromPivot)
    {
        transform.position = pivotPosition;
        transform.rotation = Quaternion.Euler(pivotRotation);

        // set camera local position
        rigCamera.transform.localPosition = cameraOffsetFromPivot;

        // add FOV state ?
        // physical camera state ?
    }


    public void SetCameraRigState(Vector3 pivotPosition, Vector3 pivotRotation)
    {
        transform.position = pivotPosition;
        transform.rotation = Quaternion.Euler(pivotRotation);

        // we don't set the camera's local position, keeping its scene values
        // WARNING : this might break if we moved the camera then switch to using this overload
        // watch out
    }
}
