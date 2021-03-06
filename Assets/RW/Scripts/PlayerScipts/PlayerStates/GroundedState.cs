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
    //Grounded State does not move around but it is used by other states to keep track
    //of movement input by the player and if the character in those states can be moved
    public class GroundedState : PlayerState
    {
        protected float speed;
        protected float rotationSpeed;
        //To check whther or not the player can attack
        protected bool canAttack;

        private float horizontalInput;
        private float verticalInput;
        //To hold a new timer
        private FunctionTimer ft;

        public GroundedState(Character character, PlayerStateMachine stateMachine) : base(character, stateMachine) {}

        public override void Enter()
        {
            base.Enter();
            DisplayOnUI(UIManager.Alignment.Left);
            horizontalInput = verticalInput = 0.0f;
            //timer is created with a delay to prevent the character
            //from being damaged again
            ft = new FunctionTimer(CanBeDamaged, 3f);
        }

        public override void Exit()
        {
            base.Exit();
            character.ResetMoveParams();
        }

        public override void HandleInput()
        {
            base.HandleInput();
            verticalInput = Input.GetAxis("Vertical");
            horizontalInput = Input.GetAxis("Horizontal");
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //If the character lost any health and it is hit
            //transition to the damage state
            if (character.currHealth < character.prevHealth && character.isHit)
            {
                stateMachine.ChangeState(character.damage);
            }
            //Run the function timer
            ft.Update();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            character.Move(verticalInput * speed, horizontalInput * rotationSpeed);
        }

        //Taken in by the function timer and set
        //the player to be able to be damaged after 
        //countdwon is over
        void CanBeDamaged()
        {
            character.canBeDamaged = true;
        }
    }
}