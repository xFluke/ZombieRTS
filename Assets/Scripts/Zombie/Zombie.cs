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
    public bool IsInfecting { get; set; }
    public Vector3 TargetPosition { get; set; }

    private void Awake() {
        Animator animator = GetComponent<Animator>();
        HumanDetector humanDetector = gameObject.AddComponent<HumanDetector>();

        stateMachine = new StateMachine();

        var idle = new Idle(this);
        var moving = new Moving(this, animator);
        var chasing = new Chasing(this, humanDetector);
        var infecting = new Infecting(this, humanDetector);

        At(idle, moving, ReceivedMovementInput());
        At(moving, idle, ReachedDestination());
        At(infecting, idle, FinishedInfecting());

        stateMachine.AddAnyTransition(chasing, () => humanDetector.HumanInRange);
        At(chasing, infecting, ReachedInfectionRange());
        At(chasing, idle, HumanNoLongerInRange());

        stateMachine.SetState(idle);

        void At(IState to, IState from, Func<bool> condition) => stateMachine.AddTransition(to, from, condition);
        Func<bool> ReceivedMovementInput() => () => IsMoving;
        Func<bool> ReachedDestination() => () => !IsMoving;
        Func<bool> ReachedInfectionRange() => () => IsInfecting;
        Func<bool> FinishedInfecting() => () => !IsInfecting;
        Func<bool> HumanNoLongerInRange() => () => !humanDetector.HumanInRange;
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

        GetComponent<Pathfinding.AIDestinationSetter>().target = destinationIndicator.transform;
        IsMoving = true; 

    }

    public void Infect(Human human) {
        IsInfecting = true;

        StartCoroutine(StartInfection(human));
    }

    IEnumerator StartInfection(Human human) {
        Vector3 targetPosition = human.transform.position;

        Destroy(human.gameObject);
        GameObject ps = Instantiate(Resources.Load("InfectionParticles") as GameObject, targetPosition, Quaternion.identity);

        yield return new WaitForSeconds(2);

        Instantiate(Resources.Load("Zombie") as GameObject, targetPosition, Quaternion.identity);
        Destroy(ps);

        IsInfecting = false;
    }
}
