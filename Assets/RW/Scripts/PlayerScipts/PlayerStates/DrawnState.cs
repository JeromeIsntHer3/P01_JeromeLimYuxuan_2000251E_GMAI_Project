using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //DrawnState transitions from SheathState,DuckingState and JumpingState
    public class DrawnState : StandingState
    {
        private bool sheathMelee;
        private bool attack;
        private bool blocking;
        

        public DrawnState(Character character, PlayerStateMachine stateMachine) : base(character, stateMachine){ }

        public override void Enter()
        {
            DisplayOnUI(UIManager.Alignment.Left);
            base.Enter();
            sheathMelee = false;
            attack = false;
            canAttack = true;
            blocking = false;
            if(stateMachine.PrevState != character.blocking)
            {
                EquippingWeapon();
            }
        }

        public override void HandleInput()
        {
            base.HandleInput();
            sheathMelee = Input.GetKeyDown(KeyCode.Q);
            attack = Input.GetButtonDown("Fire1");
            blocking = Input.GetButtonDown("Fire2");
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
            if (blocking)
            {
                stateMachine.ChangeState(character.blocking);
            }
        }

        public override void Exit()
        {
            base.Exit();
            if (!blocking)
            {
                character.TriggerAnimation(character.sheathParam);
                character.Invoke("SheathWeapon", 0.25f);
            }
        }

        void EquippingWeapon()
        {
            character.Unequip();
            character.TriggerAnimation(character.drawParam);
            character.Equip(character.MeleeWeapon);
        }
    }
}