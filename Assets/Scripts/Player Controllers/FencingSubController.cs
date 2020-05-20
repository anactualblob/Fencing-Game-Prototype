using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FencingSubController : SubController
{

    Animator animator;

    [SerializeField] bool canStopFencing = true;


    [Header("Attack")]

    [Range(0, 1)]
    [SerializeField] float attackStartedThreshold = 0;
    [Range(0, 1)]
    [SerializeField] float attackCompletedThreshold = 0;
    [Space]

    [SerializeField] float attackRecoveryDuration = 0;
    [Space]

    [SerializeField] AnimationCurve attackCurve = null;
    [SerializeField] bool useAttackCurve;

    float rawAttackValue;
    float processedAttackValue;
    bool attacking;
    bool attackRecovering;
    float attackCompletedTime;
    bool outOfRecovery = false;



 


    [Header("Camera")]

    [SerializeField] FencingTarget fencingTarget = null; // TODO : REPLACE WITH PROPER DETECTION OF FENCINGTARGET OBJECTS IN AN AREA
    [Space]

    [Range(-1, 1)] [Tooltip("Whether the position of the camera rig pivot is biased towards the enemy, or the player. -1 -> enemy position, 1 -> player position, 0 -> center")]
    [SerializeField] float cameraRigPositionBias = 0.0f;
    [Tooltip("Height from the ground of the camera rig pivot point.")]
    [SerializeField] float cameraRigVerticalOffset = 0.0f;
    [Tooltip("Rotation of the camera rig on its local x axis. ")]
    [SerializeField] float cameraRigPitch = 0.0f;
    [Tooltip("Angle of the camera rig on the y axis, from the line between the two fighters. " +
        "Negative values put the camera on the left, positive values on the right.")]
    [SerializeField] float cameraFixedAngle = 0.0f;
    [Space]
    [Tooltip("The minimum distance from the edge of the camera frustum the fighters should be.")]
    [SerializeField] float cameraMargin = 0.0f;
    [Space] 

    Vector3 rigRot = Vector3.zero;
    Vector3 cameraOffset = Vector3.zero;
    Vector3 rigPos = Vector3.zero;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    #region Activation/Deactivaation
    public override void OnSubControllerActivate()
    {
        // TODO : Find fencingTarget in scene instead of using a serialized field

        // Fail the activation if no target is found
        if (fencingTarget == null) throw new ActivationFailedException("fencingTarget is null.");

        // face the enemy
        transform.LookAt(fencingTarget.transform, Vector3.up);

        animator.SetBool("Fencing", true);
    }


    public override void OnSubControllerActivationFailed(ActivationFailedException e) 
    {
        Debug.LogWarning("Couldn't enter fencing mode. Error message : " + e.Message);
    }


    public override void OnSubControllerDeactivate()
    {
        if (!canStopFencing) throw new DeactivationFailedException("canStopFencing is false.");
        animator.SetBool("Fencing", false);
    }

    public override void OnSubControllerDeactivationFailed(DeactivationFailedException e)
    {
        Debug.LogWarning("Couldn't exit fencing mode. Error message : " + e.Message);
    }
    #endregion


    public override void ActiveSubControllerUpdate()
    {

        // face the enemy
        transform.LookAt(fencingTarget.transform, Vector3.up);

        #region Attacking Logic
        // we dedetermine if we're attacking based on the raw attack value, not the procesed attack value
        // (is this intentional? is this motivated? i don't remember ¯\_(ツ)_/¯ )
        if (rawAttackValue > attackStartedThreshold && !attacking)
            attacking = true;
        if (rawAttackValue < attackStartedThreshold && !attackRecovering)
            attacking = false;
        
        
        // process the attack value. the function does some mandatory processing but extra processing is determined by booleans and variables.
        processedAttackValue = ProcessAttackValue(rawAttackValue);

        // not recovering from an attack
        if (!attackRecovering && !outOfRecovery)
        {
            // if the value is passed the threshold for attacking, cache the time of attack and start a recovery "timer"
            if (processedAttackValue >= attackCompletedThreshold)
            {
                attackRecovering = true;
                attackCompletedTime = Time.time;

                animator.SetFloat("Attack", 1);
            }
            // otherwise just assign the value 
            else if (attacking)
            {
                animator.SetFloat("Attack", processedAttackValue);
            }
        }

        // ensure we're not in an attack pose when not attacking
        if (!attacking)
        {
            animator.SetFloat("Attack", 0);
        }


        // after recovery period, return to base pose
        // this should be animated ideally
        if (attackRecovering && attackCompletedTime + attackRecoveryDuration < Time.time)
        {
            animator.SetFloat("Attack", 0);
            attackRecovering = false;

            outOfRecovery = true;
        }

        // only after having released the attack trigger can we be out of recovery and attack again
        if (outOfRecovery && rawAttackValue <= attackStartedThreshold)
        {
            outOfRecovery = false;
            attacking = false;
        }
        #endregion
        
        UpdateCamera();

    }

    /// <summary>
    /// Computes what the state of the cameraRig should be and sends the values to cameraRig.SetCameraRigState()
    /// <para>
    /// The cameraRig is positioned at a point on the line between the two fighters, with a fixed angle 
    /// relative to that line.
    /// </para>
    /// <para>
    /// The distance of the camera from the rig's origin is calculated so that both fighters are always in frame.
    /// </para>
    /// </summary>
    public void UpdateCamera()
    {
        // remap cameraRigPositionBias from (-1,1) range to (0,1) range for lerp
        float u = (cameraRigPositionBias + 1) / 2;

        // set rig position
        rigPos = Vector3.Lerp(fencingTarget.transform.position, transform.position, u);
        rigPos.y += cameraRigVerticalOffset;

        // since cameraFixedAngle is relative to the line between the two fighters, we need to add the angle of that line to 
        //      get the angle in "world space"
        float adjustedAngle = Vector3.SignedAngle(Vector3.forward, fencingTarget.transform.position - transform.position, Vector3.up) + cameraFixedAngle;

        rigRot = new Vector3(cameraRigPitch, adjustedAngle, 0);

        // we need to first find the width we want the frustum to be to frame both the fighters.
        //
        // to find this we find the dot product of both their positions (relative to the camera rig position) with 
        //      the cameraRig.horizontalRight vector.
        //      
        // we take the biggest result of those two dot products, double it, add an optional margin and we have the 
        //      width we want the frustum to be.
        float dot1 = Mathf.Abs(Vector3.Dot(transform.position - cameraRig.transform.position, cameraRig.horizontalRight));
        float dot2 = Mathf.Abs(Vector3.Dot(fencingTarget.transform.position - cameraRig.transform.position, cameraRig.horizontalRight));
        float width = (Mathf.Max(dot1, dot2) + cameraMargin) * 2;


        // once we have the desired width, we apply this formula found on unity's docs to find the distance the camera must   
        //      be at for the frustum to be a given width : https://docs.unity3d.com/Manual/FrustumSizeAtDistance.html
        float distance = ((width / cameraRig.cameraAspect) * 0.5f) / Mathf.Tan(cameraRig.cameraFOV * 0.5f * Mathf.Deg2Rad);

        // to ensure that no fighter is behind the camera, we check the dot product of both their relative positions
        //      with the cameraRig's horizontal forward. if one of the to is negative, that means it's behind the 
        //      cameraRig center. in that case, we add the absolute value of the negative dot product to the distance.
        dot1 = Vector3.Dot(transform.position - cameraRig.transform.position, cameraRig.horizontalForward);
        dot2 = Vector3.Dot(fencingTarget.transform.position - cameraRig.transform.position, cameraRig.horizontalForward);

        if (dot1 <= 0 || dot2 <= 0) distance += Mathf.Abs(Mathf.Min(dot1, dot2)); 


        // the camera will be moved back by the distance we found
        cameraOffset = Vector3.back * distance;


        // pass the values to the cameraRig
        cameraRig.SetCameraRigState(rigPos, rigRot, cameraOffset);
    }


    /// <summary>
    /// Perform necessary and optional operations on the input attack value before sending it t the animator.
    /// </summary>
    /// <param name="value">The input attack value to be processed.</param>
    /// <returns>Returns the processed attack value.</returns>
    float ProcessAttackValue(float value)
    {
        if (!useAttackCurve)
        {
            return value;
        }
        else
        {
            return attackCurve.Evaluate(value);
        }
    }


    #region Input Receiving
    public void ReceiveAttackInput(float attackValue)
    {
        rawAttackValue = attackValue;
    }

    public void ReceiveAimInput(Vector2 aimValue)
    {
        if (!attacking)
        {
            animator.SetFloat("AimX", aimValue.x);
            animator.SetFloat("AimY", aimValue.y);
        }
    }

    
    #endregion
}
