using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infecting : IState
{
    private Zombie zombie;
    private HumanDetector humanDetector;
    public Infecting(Zombie zombie, HumanDetector humanDetector) {
        this.zombie = zombie;
        this.humanDetector = humanDetector;
    }

    public void OnEnter() {
        zombie.stateText.text = "Infecting";
        Debug.Log("IN THE INFECT STATE");
       // zombie.Infect(humanDetector.GetNearestHumanTransform().GetComponent<Human>());
    }

    public void OnExit() {
    }

    public void Tick() {
    }
}
