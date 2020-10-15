using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    // Start is called before the first frame update

    private StateMachine machine;



    public virtual void Execute() {
        
    }

    public virtual void OnStateEnter() {

    }
    public virtual void OnStateExit()
    {

    }

}
