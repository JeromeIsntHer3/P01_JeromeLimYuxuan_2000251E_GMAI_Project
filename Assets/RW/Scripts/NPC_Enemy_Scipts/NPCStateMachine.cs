using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //The StateMachine is taken in by the character to go through the different states
    public class NPCStateMachine
    {
        //CurrentState derives from the State Class
        //that holds the state that is actively running
        public NPCState CurrentState { get; private set; }
        //PrevState derives from the State Class that
        //holds the state that was running previously
        public NPCState PrevState { get; private set; }
        //The Initialize Function takes the state the character
        //should be in when the game starts
        public void Initialize(NPCState startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
        }
        //The ChangeState function sets the CurrentState as the PrevState
        //and then Exits from that CurrentState. After which the State within the
        //param will be set as the CurrentState and then that "newState" will be entered
        public void ChangeState(NPCState newState)
        {
            PrevState = CurrentState;
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}