using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FencingController : MonoBehaviour
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



    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        if (rawAttackValue > attackStartedThreshold && !attacking)
            attacking = true;
        if (rawAttackValue < attackStartedThreshold && !attackRecovering)
            attacking = false;
            

       


        
        processedAttackValue = ProcessAttackValue(rawAttackValue);

        if(processedAttackValue >= attackCompletedThreshold && !attackRecovering && !outOfRecovery)
        {
            // if the value is passed the threshold for attacking, cache the time of attack and start a recovery "timer"
            attackRecovering = true;
            attackCompletedTime = Time.time;


            animator.SetFloat("Attack", 1);
        }
        

        if (attacking && !attackRecovering && !outOfRecovery)
        {
            // otherwise just assign the value 
            animator.SetFloat("Attack", processedAttackValue);
        }

        if ( outOfRecovery && rawAttackValue <= attackStartedThreshold )
        {
            outOfRecovery = false;
            attacking = false;
        }
        

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
