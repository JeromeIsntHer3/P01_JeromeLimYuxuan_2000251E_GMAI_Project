using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class SheathState : StandingState
    {
        private bool drawMelee;

        public SheathState(Character character, PlayerStateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            DisplayOnUI(UIManager.Alignment.Left);
            drawMelee = false;
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
    }
}