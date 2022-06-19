using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //SheathState transitions from and to DrawnState,DuckingState and JumpingState
    public class SheathState : StandingState
    {
        private bool drawMelee;

        public SheathState(Character character, PlayerStateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            DisplayOnUI(UIManager.Alignment.Left);
            //Entering SheathState:
            drawMelee = false;
        }

        public override void HandleInput()
        {
            base.HandleInput();
            //Check if play wants to draw the melee out with KeyDown on Q
            drawMelee = Input.GetKeyDown(KeyCode.Q);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (drawMelee)
            {
                //if the player wants to draw the melee out transtion state
                //to DrawnState to draw out the weapon
                stateMachine.ChangeState(character.drawn);
            }
        }
    }
}