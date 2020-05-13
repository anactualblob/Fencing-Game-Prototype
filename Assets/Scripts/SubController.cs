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
                    break;
                case SubControllerState.active:
                    OnSubControllerActivate();
                    break;
                case SubControllerState.inactive:
                    OnSubControllerDeactivate();
                    break;
            }

            state = value;
        }
    }


    /// <summary>
    /// Called by the master controller every frame if this subcontroller is the active one
    /// </summary>
    public abstract void ActiveSubControllerUpdate();

    /// <summary>
    /// Called when this subcontroller's State is set to SubControllerState.active
    /// </summary>
    public abstract void OnSubControllerActivate();

    /// <summary>
    /// Called when this subcontroller's State is set to SubControllerState.inactive
    /// </summary>
    public abstract void OnSubControllerDeactivate();
}
