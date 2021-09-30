using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class ZombieMovement : MonoBehaviour
{
    Vector3 targetPosition;
    NavMeshAgent navMeshAgent;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            navMeshAgent.SetDestination(targetPosition);
            Debug.Log(targetPosition);
            animator.SetBool("isMoving", true);
        }

        // Check if reached destination
        if (!navMeshAgent.pathPending) {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f) {
                    animator.SetBool("isMoving", false);
                }
            }
        }

        // Flip sprite depending on walking direction
        if (navMeshAgent.velocity.x < 0) {
            transform.eulerAngles = new Vector2(0, 180);
        }
        else {
            transform.eulerAngles = new Vector2(0, 0);
        }


        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5);
    }
}
