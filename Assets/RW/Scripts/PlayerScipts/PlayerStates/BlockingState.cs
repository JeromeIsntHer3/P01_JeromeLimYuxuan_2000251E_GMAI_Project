using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //The BlockingState transitions between Drawn State
    //Transitions from Drawn State when the blockHeld input is pressed and remains
    //in blocking state as long as the input is being held down

    //Derives from groundedstate no movement input are required during this state
    public class BlockingState : GroundedState
    {
        //Check is the player is inputting the block button
        private bool blockHeld;
        public BlockingState(Character character, PlayerStateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            DisplayOnUI(UIManager.Alignment.Left);
            //Deactive the hitbox on the player when blockign so that they don't take damage
            character.DeactivateHitBox();
            //Set the animation of the blocking to true so that it loops on itself and the 
            //the animation remains until the block button is no longer being held
            character.SetAnimationBool(character.isBlocking, true);
        }

        public override void HandleInput()
        {
            base.HandleInput();
            //blockheld will be set to true as longas the button is held down
            blockHeld = Input.GetButton("Fire2");
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //if the block input is no longer being held
            //change the state back to which ever previous state it was from
            if (!blockHeld)
            {
                stateMachine.ChangeState(stateMachine.PrevState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            //Reactivate the player's hitbox after it leaves the blocking state
            //so it can be damaged again
            character.ActivateHitBox();
            //Set the animation bool of the blocking animation to false
            //to return the animation back to the previous states'
            //animations
            character.SetAnimationBool(character.isBlocking, false);
        }
    }
}