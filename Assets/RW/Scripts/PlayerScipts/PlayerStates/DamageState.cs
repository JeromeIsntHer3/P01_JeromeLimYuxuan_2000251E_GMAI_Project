using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //DamageState transition from GroundedState(s) (SheathState,DuckingState)
    //when the character loses health

    public class DamageState : GroundedState
    {
        private bool dead;
        private bool stunned;

        private float stunTimer = 1f;

        private FunctionTimer ft;

        public DamageState(Character character, PlayerStateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            DisplayOnUI(UIManager.Alignment.Left);
            ft = new FunctionTimer(IsStunned, stunTimer);
            dead = false;
            canAttack = false;
            character.canBeDamaged = false;
            character.isHit = false;
            stunned = true;
            if (character.currHealth <= 0)
            {
                dead = true;
            }
            if (!dead)
            {
                character.TriggerAnimation(character.hit);
            }
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            ft.Update();
            if (!stunned && !dead)
            {
                stateMachine.ChangeState(character.drawn);
            }
            else if (dead)
            {
                stateMachine.ChangeState(character.dead);
            }
        }

        public void IsStunned()
        {
            stunned = false;
        }
    }
}