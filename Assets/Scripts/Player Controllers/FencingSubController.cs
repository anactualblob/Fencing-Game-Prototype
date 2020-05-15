using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FencingSubController : SubController
{

    Animator animator;



    [Header("Attack")]
    [Range(0,1)]
    [SerializeField] float attackStartedThreshold;
    [Range(0, 1)]
    [SerializeField] float attackCompletedThreshold;
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

   
    public override void OnSubControllerActivate()
    {
        animator.SetBool("Fencing", true);
    }

    public override void OnSubControllerDeactivate()
    {
        animator.SetBool("Fencing", false);
    }



    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public override void ActiveSubControllerUpdate()
    {
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
