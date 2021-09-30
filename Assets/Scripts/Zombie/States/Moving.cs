using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : IState
{
    private Zombie zombie;
    private Animator animator;

    public Moving(Zombie zombie, Animator animator) {
        this.zombie = zombie;
        this.animator = animator;
    }
    public void OnEnter() {
        zombie.stateText.text = "Moving";
        zombie.GetComponent<Pathfinding.AIPath>().maxSpeed = 5f;
    }

    public void OnExit() {
       
    }

    public void Tick() {
        if (Vector3.Distance(zombie.transform.position, zombie.TargetPosition) < 0.2f) {
            zombie.IsMoving = false;
            animator.SetBool("isMoving", false);
        }
        else {
            animator.SetBool("isMoving", true);
        }
    }
}
