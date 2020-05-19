using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SubController : MonoBehaviour
{

    public enum SubControllerState
    {
        none,
        active,
        inactive
    }

    SubControllerState state = SubControllerState.inactive;

    /// <summary>
    /// Each SubController is responsible for handling the camera.
    /// <para>cameraRig is set by PlayerMasterController when the SubController is set to active, otherwise it is null.</para>
    /// </summary>
    [HideInInspector] public CameraRig cameraRig;


    /// <summary>
    /// The state of this subcontroller.
    /// <para>Calls OnSubControllerActivate/Deactivate if set to active and inactive respectively.</para>
    /// </summary>
    public SubControllerState State
    {
        get { return state; }
        set
        {
            switch (value)
            {
                case SubControllerState.none:
                    Debug.LogWarning("SubController.cs : Setting this SubController's State property to 'SubControllerState.none'. This should not happen.");
                    break;
                case SubControllerState.active:
                    OnSubControllerActivate();
                    break;
                case SubControllerState.inactive:
                    OnSubControllerDeactivate();
                    break;
                default:
                    Debug.LogWarning("SubController.cs : Changing the State of this SubController to '" + value + "'. This case has not been implemented.");
                    break;
            }

            state = value;
        }
    }


    /// <summary>
    /// Called by the master controller every frame if this SubController is the active one
    /// </summary>
    public abstract void ActiveSubControllerUpdate();

    /// <summary>
    /// Called when this SubController's State is set to SubControllerState.active
    /// </summary>
    public abstract void OnSubControllerActivate();

    /// <summary>
    /// Called when this SubController's State is set to SubControllerState.inactive
    /// </summary>
    public abstract void OnSubControllerDeactivate();

    /// <summary>
    /// Called when OnSubControllerActivate throws an exception, meaning the conditions for this SubController to become active aren't met.
    /// </summary>
    public abstract void OnSubControllerActivationFailed(ActivationFailedException e);

    /// <summary>
    /// Called when OnSubControllerDeactivate throws an exception, meaning the conditions for this SubController to become inactive aren't met.
    /// </summary>
    public abstract void OnSubControllerDeactivationFailed(DeactivationFailedException e);


    public class ActivationFailedException : System.Exception
    {
        public ActivationFailedException() : base() { }
        public ActivationFailedException(string message) : base(message) { }
        public ActivationFailedException(string message, System.Exception inner) : base(message, inner) { }
    }

    public class DeactivationFailedException : System.Exception
    {
        public DeactivationFailedException() : base() { }
        public DeactivationFailedException(string message) : base(message) { }
        public DeactivationFailedException(string message, System.Exception inner) : base(message, inner) { }
    }
}
