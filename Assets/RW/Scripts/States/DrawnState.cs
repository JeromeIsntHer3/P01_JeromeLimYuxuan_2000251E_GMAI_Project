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

        public DrawnState(Character character, StateMachine stateMachine) : base(character, stateMachine){ }

        public override void Enter()
        {
            base.Enter();
            //Enter DrawnState:
            //1. Set the SheathMelee, attack,blocking to false and
            //canAttack to true to allow the player to attack
            sheathMelee = false;
            attack = false;
            canAttack = true;
            blocking = false;
            //2. Check if PrevState isn't BlockingState
            //and if it isn't use the character Unequip,Equip Functions and
            //Trigger Draw Param to show the animation
            if(stateMachine.PrevState != character.blocking)
            {
                character.Unequip();
                character.TriggerAnimation(character.drawParam);
                character.Equip(character.MeleeWeapon);
            }
        }

        public override void HandleInput()
        {
            base.HandleInput();
            //Check if the player wants to sheath the weapon using Q
            sheathMelee = Input.GetKeyDown(KeyCode.Q);
            //Check if the player want to attack with the weapon using Mouse 0
            attack = Input.GetButtonDown("Fire1");
            //Check if the player want to block with the weapon using Mouse 1
            blocking = Input.GetButtonDown("Fire2");
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //Transition to the SheathState if the player wants to Sheath the weapon
            if (sheathMelee)
            {
                stateMachine.ChangeState(character.sheath);
            }
            //Trigger the Swing Param to cause the swing animation
            else if (attack && canAttack)
            {
                character.TriggerAnimation(character.swingParam);
            }
            //Transition to the BlockingState if the player decides to block
            if (blocking)
            {
                stateMachine.ChangeState(character.blocking);
            }
        }
    }
}