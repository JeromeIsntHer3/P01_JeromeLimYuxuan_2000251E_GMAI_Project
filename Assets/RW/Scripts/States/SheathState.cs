using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //SheathState transitions from and to DrawnState,DuckingState and JumpingState
    public class SheathState : StandingState
    {
        private bool drawMelee;

        public SheathState(Character character, StateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            //Entering SheathState:
            drawMelee = false;
            //1. Check if the the PrevState has the weapon be drawn
            //If so, Trigger the Sheath Param for the animation
            //and invoke the SheathWeapon function with a delay from 
            //character script
            if (stateMachine.PrevState == character.drawn)
            {
                character.TriggerAnimation(character.sheathParam);
                character.Invoke("SheathWeapon", 0.25f);
            }
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