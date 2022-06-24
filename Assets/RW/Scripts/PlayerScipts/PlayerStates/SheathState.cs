using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class SheathState : StandingState
    {
        //Check if the melee is to be drawn
        private bool drawMelee;

        public SheathState(Character character, PlayerStateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            DisplayOnUI(UIManager.Alignment.Left);
            //Set as false as it should only be true
            //when the player presses the Q key
            drawMelee = false;
        }

        public override void HandleInput()
        {
            base.HandleInput();
            //drawMelee is only true when the player
            //presses Q
            drawMelee = Input.GetKeyDown(KeyCode.Q);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //if drawMelee is true, which means the player wants
            //to draw out the sword, the character will transition
            //into DrawnState
            if (drawMelee)
            {
                stateMachine.ChangeState(character.drawn);
            }
        }
    }
}