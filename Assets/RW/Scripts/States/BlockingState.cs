using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class BlockingState : State
    {
        private bool blockHeld;
        public BlockingState(Character character, StateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
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
                if (stateMachine.PrevState == character.jumping)
                {
                    return;
                }
                stateMachine.ChangeState(stateMachine.PrevState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            character.SetAnimationBool(character.isBlocking, false);
        }
    }
}