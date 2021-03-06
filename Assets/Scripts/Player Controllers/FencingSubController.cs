﻿using System;
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
    [SerializeField] bool useAttackCurve = true;

    float rawAttackValue;
    float processedAttackValue;
    bool attacking;
    bool attackRecovering;
    float attackCompletedTime;
    bool outOfRecovery = false;






    [Header("Camera")]

    [SerializeField] FencingTarget fencingTarget = null; // TODO : REPLACE WITH PROPER DETECTION OF FENCINGTARGET OBJECTS IN AN AREA
    [Space]

    [SerializeField] FencingCameraState fencingCameraState_near = FencingCameraState.empty;
    [SerializeField] FencingCameraState fencingCameraState_middle = FencingCameraState.empty;
    [SerializeField] FencingCameraState fencingCameraState_far = FencingCameraState.empty;
    [SerializeField] FencingCameraState fencingCameraState_attack = FencingCameraState.empty;
    [Space]
    [Tooltip("Whether or not to use fencingCameraState_middle as an intermediate state between _near and _far")]
    [SerializeField] bool useMiddleCameraState = false;
    [Tooltip("Distance from the enemy below which the fencingCameraState_near is used.")]
    [SerializeField] float nearCameraStateThreshold = 0.0f;
    [Tooltip("Distance from the enemy at which the fencingCameraState_middle is used (if useMiddleCameraState is true)")]
    [SerializeField] float middleCameraStateThreshold = 0.0f;
    [Tooltip("Distance from the enemy above which the fencingCameraState_far is used.")]
    [SerializeField] float farCameraStateThreshold = 0.0f;
    [Space]
    [Tooltip("The minimum distance from the edge of the camera frustum the fighters should be.")]
    [SerializeField] float cameraMargin = 0.0f;
    [Space]

    Vector3 rigRot = Vector3.zero;
    Vector3 cameraOffset = Vector3.zero;
    Vector3 rigPos = Vector3.zero;

    [Header("Movement")]
    [SerializeField] float movementSpeed = 1.0f;

    Vector3 movement;
    bool canMove = true;

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
        if (canMove)
        {
            transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);

            if (movement.magnitude > 0.0f)
            {
                animator.SetBool("Moving", true);
                animator.SetFloat("MoveSide", movement.x);
                animator.SetFloat("MoveForward", movement.z);
            }
            else
            {
                animator.SetBool("Moving", false);
            }
        }
        else
        {
            animator.SetBool("Moving", false);
        }

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

                canMove = false;

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

            canMove = true;

            outOfRecovery = true;
        }

        // only after having released the attack trigger can we be out of recovery and attack again
        if (outOfRecovery && rawAttackValue <= attackStartedThreshold)
        {
            outOfRecovery = false;
            attacking = false;
        }
        #endregion


        #region Camera Updating

        float fightersDistance = Vector3.Distance(transform.position, fencingTarget.transform.position);

        if (fightersDistance > farCameraStateThreshold)
            UpdateCamera(fencingCameraState_far);

        else if (fightersDistance < nearCameraStateThreshold)
            UpdateCamera(fencingCameraState_near);

        else
            if (useMiddleCameraState)
            {
                if (fightersDistance <= middleCameraStateThreshold)
                {
                    float u = (fightersDistance - nearCameraStateThreshold) / (middleCameraStateThreshold - nearCameraStateThreshold);
                    FencingCameraState lerpedState = FencingCameraState.Lerp(fencingCameraState_near, fencingCameraState_middle, u);
                    UpdateCamera(lerpedState);
                }
                if (fightersDistance > middleCameraStateThreshold)
                {
                    float u = (fightersDistance - middleCameraStateThreshold) / (farCameraStateThreshold - middleCameraStateThreshold);
                    FencingCameraState lerpedState = FencingCameraState.Lerp(fencingCameraState_middle, fencingCameraState_far, u);
                    UpdateCamera(lerpedState);
                }
            }
            else
            {
                float u = (fightersDistance - nearCameraStateThreshold) / (farCameraStateThreshold - nearCameraStateThreshold);
                FencingCameraState lerpedState = FencingCameraState.Lerp(fencingCameraState_near, fencingCameraState_far, u);
                UpdateCamera(lerpedState);
            }

        #endregion
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
    void UpdateCamera(FencingCameraState cameraState)
    {
        // remap positionBias from (-1,1) range to (0,1) range for lerp
        float u = (cameraState.positionBias + 1) / 2;

        // set rig position, between the player and the opponent, according to the positionBias that's been remapped above
        rigPos = Vector3.Lerp(fencingTarget.transform.position, transform.position, u);
        rigPos.y += cameraState.verticalOffset;

        // since cameraFixedAngle is relative to the line between the two fighters, we need to add the angle that line makes  
        //      with the world z axis to get the angle in "world space"
        float adjustedAngle = Vector3.SignedAngle(Vector3.forward, fencingTarget.transform.position - transform.position, Vector3.up) + cameraState.fixedAngle;

        // set te rig rotation
        rigRot = new Vector3(cameraState.pitch, adjustedAngle, 0);

        // we need to first find the width we want the camera frustum to be to frame both the fighters.
        //
        // to find this we get the dot product of both their positions (relative to the camera rig position) with 
        //      the cameraRig.horizontalRight vector. this gives us the distance of each fighter to the center of 
        //      the screen.
        //      
        // we take the biggest result of those two distances, double it, add an optional margin and we that gives 
        //      us the width we want the frustum to be.
        //
        float dot1 = Mathf.Abs(Vector3.Dot(transform.position - cameraRig.transform.position, cameraRig.horizontalRight));
        float dot2 = Mathf.Abs(Vector3.Dot(fencingTarget.transform.position - cameraRig.transform.position, cameraRig.horizontalRight));
        float width = (Mathf.Max(dot1, dot2) + cameraMargin) * 2;


        // once we have the desired width, we apply this formula, found on unity's docs, to find the distance the camera    
        //      must be at for the frustum to be a given width : https://docs.unity3d.com/Manual/FrustumSizeAtDistance.html
        float distance = ((width / cameraRig.cameraAspect) * 0.5f) / Mathf.Tan(cameraRig.cameraFOV * 0.5f * Mathf.Deg2Rad);

        // to ensure that no fighter is behind the camera, we check the dot product of both their relative positions
        //      with the cameraRig.horizontalForward . If one of the two is negative, that means it's behind the 
        //      cameraRig center. In that case, we add the absolute value of the negative dot product to the distance.
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

    public void ReceiveMoveInput(Vector2 moveVector)
    {
        movement = moveVector.x * transform.right + moveVector.y * transform.forward;
    }
    #endregion



    [System.Serializable]
    struct FencingCameraState
    {
        [Range(-1, 1)] 
        [Tooltip("Whether the position of the camera rig pivot is biased towards the enemy, or the player. -1 -> enemy position, 1 -> player position, 0 -> center")]
        public float positionBias;

        [Tooltip("Height from the ground of the camera rig pivot point.")]
        public float verticalOffset;

        [Tooltip("Rotation of the camera rig on its local x axis. ")]
        public float pitch;

        [Tooltip("Angle of the camera rig on the y axis, from the line between the two fighters. " +
        "Negative values put the camera on the left, positive values on the right.")]
        public float fixedAngle;


        static public FencingCameraState empty
        {
            get { return new FencingCameraState{}; }
        }

        /// <summary>
        /// Returns a linear interpolation of two FencingCameraStates. 
        /// <para>Performs Mathf.Lerp on each member of the given args and returns a new FencingCameraState with the results of those lerps as members.</para>
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lerpValue"></param>
        /// <returns></returns>
        static public FencingCameraState Lerp(FencingCameraState from, FencingCameraState to, float lerpValue)
        {
            return new FencingCameraState
            {
                fixedAngle = Mathf.Lerp(from.fixedAngle, to.fixedAngle, lerpValue),
                positionBias = Mathf.Lerp(from.positionBias, to.positionBias, lerpValue),
                pitch = Mathf.Lerp(from.pitch, to.pitch, lerpValue),
                verticalOffset = Mathf.Lerp(from.verticalOffset, to.verticalOffset, lerpValue)
            };
        }

    }
}
