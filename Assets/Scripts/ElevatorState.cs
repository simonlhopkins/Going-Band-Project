using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorState : State
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Debug.Log("Elevator State Entered");
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        Debug.Log("Elevator State Exited");
    }
}
