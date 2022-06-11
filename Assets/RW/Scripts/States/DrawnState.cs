using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class DrawnState : StandingState
    {
        private bool sheathMelee;
        private bool attack;

        public DrawnState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            sheathMelee = false;
            attack = false;
            canAttack = true;

            character.Unequip();
            character.TriggerAnimation(character.drawParam);
            character.Equip(character.MeleeWeapon);
        }

        public override void HandleInput()
        {
            base.HandleInput();
            sheathMelee = Input.GetKeyDown(KeyCode.Q);
            attack = Input.GetButtonDown("Fire1");
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (sheathMelee)
            {
                stateMachine.ChangeState(character.sheath);
            }
            else if (attack && canAttack)
            {
                character.TriggerAnimation(character.swingParam);
            }
        }
    }
}