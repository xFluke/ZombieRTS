using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasing : IState
{
    private Zombie zombie;
    private HumanDetector humanDetector;

    public Chasing(Zombie zombie, HumanDetector humanDetector) {
        this.zombie = zombie;
        this.humanDetector = humanDetector;
    }

    public void OnEnter() {
        zombie.stateText.text = "Chasing";
        zombie.GetComponent<Pathfinding.AIDestinationSetter>().target = humanDetector.GetNearestHumanTransform();
        zombie.GetComponent<Pathfinding.AIPath>().maxSpeed = 8f;
    }

    public void OnExit() {
    }

    public void Tick() {
        //Debug.Log(Vector3.Distance(zombie.transform.position, humanDetector.GetNearestHumanTransform().position));

        if (Vector3.Distance(zombie.transform.position, humanDetector.GetNearestHumanTransform().position) <= 0.25f) {
            zombie.Infect(humanDetector.GetNearestHumanTransform().GetComponent<Human>());
        }
    }
}
