using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //The BlockingState transition from SheathState when Right-Click is held
    public class BlockingState : GroundedState
    {
        private bool blockHeld;
        public BlockingState(Character character, PlayerStateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            DisplayOnUI(UIManager.Alignment.Left);
            character.DeactivateHitBox();
            character.SetAnimationBool(character.isBlocking, true);
        }

        public override void HandleInput()
        {
            base.HandleInput();
            blockHeld = Input.GetButton("Fire2");
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!blockHeld)
            {
                stateMachine.ChangeState(stateMachine.PrevState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            character.ActivateHitBox();
            character.SetAnimationBool(character.isBlocking, false);
        }
    }
}