using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Animations;
using System;

public class Zombie : MonoBehaviour
{
    [SerializeField] GameObject selectedIndicator;

    [SerializeField] GameObject destinationIndicatorPrefab;
    GameObject destinationIndicator;

    public Text stateText;

    StateMachine stateMachine;
    
    public bool IsMoving { get; set; }
    public Vector3 TargetPosition { get; set; }

    private void Awake() {
        Animator animator = GetComponent<Animator>();
        HumanDetector humanDetector = gameObject.AddComponent<HumanDetector>();

        stateMachine = new StateMachine();

        var idle = new Idle(this);
        var moving = new Moving(this, animator);
        var chasing = new Chasing(this, humanDetector);

        At(idle, moving, ReceivedMovementInput());
        At(moving, idle, ReachedDestination());

        stateMachine.AddAnyTransition(chasing, () => humanDetector.HumanInRange);

        stateMachine.SetState(idle);

        void At(IState to, IState from, Func<bool> condition) => stateMachine.AddTransition(to, from, condition);
        Func<bool> ReceivedMovementInput() => () => IsMoving;
        Func<bool> ReachedDestination() => () => !IsMoving;
    }

    // Start is called before the first frame update
    void Start()
    {
        TargetPosition = transform.position;
        IsMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Tick();
    }

    public void EnableSelectedIndicator(bool enabled) {
        selectedIndicator.SetActive(enabled);
    }

    public void MoveTo(Vector3 destination) {
        if (destinationIndicator != null)
            Destroy(destinationIndicator);

        Debug.Log("Moving to " + destination);
        TargetPosition = destination;

        destinationIndicator = Instantiate(destinationIndicatorPrefab, destination, Quaternion.identity);

        if ((TargetPosition.x - transform.position.x) < 0) {
            transform.eulerAngles = new Vector2(0, 180);
        }
        else {
            transform.eulerAngles = new Vector2(0, 0);
        }

        GetComponent<Pathfinding.AIDestinationSetter>().target = destinationIndicator.transform;
        IsMoving = true; 

    }
}
