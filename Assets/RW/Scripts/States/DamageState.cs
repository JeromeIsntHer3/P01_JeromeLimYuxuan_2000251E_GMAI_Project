using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //DamageState transition from GroundedState(s) (SheathState,DuckingState)
    //when the character loses health

    public class DamageState : GroundedState
    {
        private bool hit = false;
        private bool dead;

        private float stunTimer = 1f;
        private float currTime;

        public DamageState(Character character, StateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            //When Entering DamageState:
            //1. Set the timer timing
            currTime = stunTimer;
            //2. Set the character being hit,dead and |canAttack to false to prevent
            //the player from attacking 
            hit = true;
            dead = false;
            canAttack = false;
            //3. If the character health is lesser or equal to 0, set dead to true 
            if (hb.playerHealth <= 0)
            {
                dead = true;
            }
            //4. If dead is not true(character health not equal to 0)
            //Trigger The Hit Param to play the hit animation
            if (!dead)
            {
                character.TriggerAnimation(character.hit);
            }
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //In Update:
            //1. Start the countdown timer to stop the player from moving the character until
            //it is over 
            currTime -= 1 * Time.deltaTime;

            //2. Check DamageState entry accurately works with hit being set as true
            // and timer finishing and the character not being in DeadState, transition back
            //to the PrevState
            if (hit && currTime <= 0 && !dead)
            {
                stateMachine.ChangeState(stateMachine.PrevState);
            }
            //3. If dead is true (character health lesser or equal to 0) transition to DeadState
            else if (dead)
            {
                stateMachine.ChangeState(character.dead);
            }
        }
    }
}