using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Idle : IState
{
    private readonly Zombie zombie;
    private Text text;

    public Idle(Zombie zombie) {
        this.zombie = zombie;
    }

    public void OnEnter() {
        zombie.stateText.text = "Idle";
    }

    public void OnExit() {
        
    }

    public void Tick() {
        
    }
}
