using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // Start is called before the first frame update

    protected State currentState;
    protected TimingEvent currentTimingEvent;


    private State stateAtEndOfLoop = null;
    public Dictionary<StateType, State> stateDict = new Dictionary<StateType, State>();

    public enum StateType {
        Minigame,
        Elevator,
        Tutorial
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stateAtEndOfLoop) {
            if (stateAtEndOfLoop != currentState) {
                stateAtEndOfLoop.OnStateExit();
                currentState.OnStateEnter();
            }
        }
        
        currentState.Execute();

        stateAtEndOfLoop = currentState;
    }



    public void AddState(State s, StateType st) {
        stateDict.Add(st, s);
    }
    public void setInitialState(StateType st) {
        currentState = stateDict[st];
        currentState.OnStateEnter();
    }

    public void ChangeState(StateType newState) {
        currentState = stateDict[newState];
    }
}
