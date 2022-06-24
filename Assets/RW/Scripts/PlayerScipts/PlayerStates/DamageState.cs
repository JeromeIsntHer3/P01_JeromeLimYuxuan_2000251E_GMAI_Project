using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //DamageState transition from GroundedState(s) (SheathState,DuckingState)
    //when the character loses health

    public class DamageState : GroundedState
    {
        //Check if the player is Dead
        private bool dead;
        //Check if the player is Stunned
        private bool stunned;
        //Time before the character will be stunned
        private float stunTimer = 1f;
        //Hold new timer
        private FunctionTimer ft;

        public DamageState(Character character, PlayerStateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            DisplayOnUI(UIManager.Alignment.Left);
            //Timer that will set stunned as false using the IsStunned function
            //with the time being the stunTimer 
            ft = new FunctionTimer(IsStunned, stunTimer);
            //Set Dead as false so that the player does not die by default
            dead = false;
            //Set the character as unable to attack since they are stunned
            canAttack = false;
            //Set canBeDamaged to false so that the player cannot be
            //hit again until the timer to be able to get hit again is
            //reset
            character.canBeDamaged = false;
            //Set is hit to false so that the charcter can
            //be hit again
            character.isHit = false;
            //Set stunned to be true so the character won't
            //change to drawn state until the stun timer is
            //finished
            stunned = true;
            //Set Dead as true if the character's current health
            //has reached 0
            if (character.currHealth <= 0)
            {
                dead = true;
            }
            //if dead is not true then the animation of the
            //character being hit will be triggered
            else if (!dead)
            {
                character.TriggerAnimation(character.hit);
            }
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //Run the timer to countdown the character
            //stun tiemr
            ft.Update();
            //if the character is no longer being stunned and 
            //is not dead, transition back to the drawn state
            //so that the player when hit in sheath state will
            //not need to the sheath/draw button to pull out 
            //the sword to attack
            if (!stunned && !dead)
            {
                stateMachine.ChangeState(character.drawn);
            }
            //If the character's health is equal or lower than 0
            //dead is set to true, and the state will transitioned
            //to dead state
            else if (dead)
            {
                stateMachine.ChangeState(character.dead);
            }
        }

        //Method to set stunned as false and is taken
        //in by the function timer
        public void IsStunned()
        {
            stunned = false;
        }
    }
}