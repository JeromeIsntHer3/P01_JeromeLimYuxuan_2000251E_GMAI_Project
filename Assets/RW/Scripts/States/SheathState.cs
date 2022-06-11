using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class SheathState : StandingState
    {
        private bool drawMelee;

        public SheathState(Character character, StateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            drawMelee = false;
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
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (drawMelee)
            {
                stateMachine.ChangeState(character.drawn);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}