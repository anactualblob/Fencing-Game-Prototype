using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAroundSubController : SubController
{


    public override void OnSubControllerActivate()
    {
        //throw new System.NotImplementedException();
        Debug.Log("MovingAroundSubController.cs : SubController activated");
    }

    public override void OnSubControllerDeactivate()
    {
        //throw new System.NotImplementedException();
        Debug.Log("MovingAroundSubController.cs : SubController deactivated");
    }

    public override void ActiveSubControllerUpdate()
    {
        //throw new System.NotImplementedException();
        //Debug.Log("MovingAroundSubController.cs : SubController active update");
    }
}
