using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSpeed : StateMachineBehaviour {
    
    public float fastWalkSpeed = 1.0f;
    public float runSpeed = 1.0f;
    public float PSpeed = 1.0f;

    public override void OnStateUpdate(Animator anim, AnimatorStateInfo stateInfo, int layer) {
        if (anim.GetBool("IsCrouching") || anim.GetBool("IsAttacking") && anim.GetBool("CanAttack")) {
            anim.speed = 1.0f;
            return;
        }
        switch (anim.GetInteger("Speed")) {
            case 0:
                //Idle
                anim.speed = 0.0f;
                int state = anim.GetInteger("PowerUpState");
                if (state == 1) anim.Play("Small_Walk", 1, 0f);
                else if (state == 2) anim.Play("Big_Walk", 2, 0f);
                else if (state == 3) anim.Play("Fire_Walk", 3, 0f);
                break;
            case 1:
                //SlowWalk
                anim.speed = 1.0f;
                break;
            case 2:
                //FastWalk
                anim.speed = fastWalkSpeed;
                break;
            case 3:
                //Run
                anim.speed = runSpeed;
                break;
            default:
                anim.speed = 1.0f;
                break;
        }
    }

    public override void OnStateExit(Animator anim, AnimatorStateInfo stateInfo, int layer) {
        anim.speed = 1.0f;
    }
}
