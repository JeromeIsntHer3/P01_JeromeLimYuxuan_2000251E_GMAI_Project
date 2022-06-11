using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class SheathState : StandingState
    {
        private bool drawMelee;
        private bool blocking;

        public SheathState(Character character, StateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            drawMelee = false;
            blocking = false;
            if (stateMachine.PrevState == character.drawn)
            {
                character.TriggerAnimation(character.sheathParam);
                character.Invoke("SheathWeapon", 0.25f);
            }
        }

        public override void HandleInput()
        {
            base.HandleInput();
            drawMelee = Input.GetKeyDown(KeyCode.Q);
            blocking = Input.GetKey(KeyCode.Mouse1);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (drawMelee)
            {
                stateMachine.ChangeState(character.drawn);
            }
            if (blocking)
            {
                character.SetAnimationBool(character.isBlocking, true); 
            }
            else
            {
                character.SetAnimationBool(character.isBlocking, false);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}