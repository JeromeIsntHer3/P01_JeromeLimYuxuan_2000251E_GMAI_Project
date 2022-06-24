using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //DrawnState transitions from SheathState,DuckingState and JumpingState
    //Transiton from SheathState occurs when the player presses Q to toggle into the DrawnState
    //Transition from Ducking State occurs when the previous state is DrawnState and the player
    //lets go of the crouch button
    //Transitions from Jumping state if the previous state from Jumping State was DrawnState
    public class DrawnState : StandingState
    {
        //Check the sheathMelee input
        private bool sheathMelee;
        //Check if character wants to attack
        private bool attack;
        //Check if character wants to block
        private bool blocking;
        

        public DrawnState(Character character, PlayerStateMachine stateMachine) : base(character, stateMachine){ }

        public override void Enter()
        {
            DisplayOnUI(UIManager.Alignment.Left);
            base.Enter();
            //Set canAttack to true as they are no longer in
            //the blocking state they can now attack again
            canAttack = true;
            //Set these values to false as they should only
            //be true if there is player input
            sheathMelee = false;
            attack = false;
            blocking = false;
            //Re-equip the weapon from any other state except for
            //blocking as the only way to go into blocking state
            //is from Drawn State
            if(stateMachine.PrevState != character.blocking)
            {
                EquippingWeapon();
            }
        }

        public override void HandleInput()
        {
            base.HandleInput();
            //Take in player inputs 
            //To toggle sheath/draw using Q key
            sheathMelee = Input.GetKeyDown(KeyCode.Q);
            //Set attack as true when mouse0 is clicked
            attack = Input.GetButtonDown("Fire1");
            //Set blocking to be true when mouse1 is clicked
            blocking = Input.GetButtonDown("Fire2");
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //If the player inputs Q Key the character will transition
            //into SheathState and put away the sword
            if (sheathMelee)
            {
                stateMachine.ChangeState(character.sheath);
            }
            //If the player input Mouse 0 and the character can
            //attack again, trigger and play the swing animation
            //which will toggle the damage box in front of the character
            //on and off to damage the character type in front of it
            else if (attack && canAttack)
            {
                character.TriggerAnimation(character.swingParam);
            }
            //If the player inputs the Mouse 1 button they will
            //then transition into the Blocking State
            if (blocking)
            {
                stateMachine.ChangeState(character.blocking);
            }
        }

        public override void Exit()
        {
            base.Exit();
            //When Exiting the DrawnState, trigger and play the
            //sheath animation and invoke the sheathweapon function within
            //the character script so that the player will not be able to attack
            //whilst crouching, jumping, blocking or being dead
            if (!blocking)
            {
                character.TriggerAnimation(character.sheathParam);
                character.Invoke("SheathWeapon", 0.25f);
            }
        }

        //Function that removes the sword from the back of the character
        //using Unequip, triggering the animation to drawn the sword
        //and then instantiating the weapon onto the characters hand
        //to "hold" the sword
        void EquippingWeapon()
        {
            character.Unequip();
            character.TriggerAnimation(character.drawParam);
            character.Equip(character.MeleeWeapon);
        }
    }
}