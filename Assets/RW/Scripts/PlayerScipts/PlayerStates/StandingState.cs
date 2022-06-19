/*
 * Copyright (c) 2019 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //StandingState acts as the parent class of SheathState and DrawnState
    //to know the speed the character should be at.
    public class StandingState : GroundedState
    {
        //Fields set as protected to allow States that derive from it
        //to set the new values for the variables
        protected bool jump;
        protected bool crouch;

        public StandingState(Character character, PlayerStateMachine stateMachine) : base(character, stateMachine){ }

        public override void Enter()
        {
            base.Enter();
            DisplayOnUI(UIManager.Alignment.Left);
            //When the StandingState is entered:
            //1. Set the character speed(rotation) to the player data variation
            speed = character.MovementSpeed;
            rotationSpeed = character.RotationSpeed;
            //2. Set crouch and jump to false as will take in the player
            //input in the HandleInput function
            crouch = false;
            jump = false;
        }

        public override void HandleInput()
        {
            base.HandleInput();
            //Check if the player wants to crouch with Shift/Middle-Mouse click
            crouch = Input.GetButtonDown("Fire3");
            //Check if the player wants to crouch with space bar click
            jump = Input.GetButtonDown("Jump");
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //If the player input they want to crouch transition to DuckingState
            if (crouch)
            {
                stateMachine.ChangeState(character.ducking);
            }
            //If the player input they want to jump transition to JumpState
            else if (jump)
            {
                stateMachine.ChangeState(character.jumping);
            }
        }
    }
}