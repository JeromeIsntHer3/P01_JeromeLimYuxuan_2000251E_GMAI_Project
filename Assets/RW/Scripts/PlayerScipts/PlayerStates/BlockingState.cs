using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //The BlockingState transition from SheathState when Right-Click is held
    public class BlockingState : PlayerState
    {
        private bool blockHeld;
        public BlockingState(Character character, PlayerStateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            DisplayOnUI(UIManager.Alignment.Left);
            //When the BlockingState is entered turn off the hitbox so that the player
            //will not be damaged and also set the Blocking Parameter to true
            character.DeactivateHitBox();
            character.SetAnimationBool(character.isBlocking, true);
        }

        public override void HandleInput()
        {
            base.HandleInput();
            //Check if the player is holding Right-Click
            blockHeld = Input.GetButton("Fire2");
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //If they player let's go of Right-Click, have the BlockingState transition
            //to the previous State.
            if (!blockHeld)
            {
                stateMachine.ChangeState(stateMachine.PrevState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            //When BlockingState is transitioning back to the PrevState,
            //re-enable the hitbox and set the Blocking Param to false to
            //stop the animation
            character.ActivateHitBox();
            character.SetAnimationBool(character.isBlocking, false);
        }
    }
}