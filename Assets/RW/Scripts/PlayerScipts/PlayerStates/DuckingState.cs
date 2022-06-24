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
    //Ducking State transitions between Drawn and Sheath State
    //Transition from both states occurs when the the player decides
    //to input the crouching button
    public class DuckingState : GroundedState
    {
        private bool belowCeiling;
        private bool crouchHeld;

        public DuckingState(Character character, PlayerStateMachine stateMachine) : base(character, stateMachine){ }

        public override void Enter()
        {
            base.Enter();
            DisplayOnUI(UIManager.Alignment.Left);
            character.SetAnimationBool(character.crouchParam, true);
            speed = character.CrouchSpeed;
            rotationSpeed = character.CrouchRotationSpeed;
            character.ColliderSize = character.CrouchColliderHeight;
            belowCeiling = false;
        }

        public override void Exit()
        {
            base.Exit();
            character.SetAnimationBool(character.crouchParam, false);
            character.ColliderSize = character.NormalColliderHeight;
        }

        public override void HandleInput()
        {
            base.HandleInput();
            crouchHeld = Input.GetButton("Fire3");
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //if the player is not underneath a collidable ceiling or the player
            //has stopped holding the crouch button, the character will transition
            //back to the previous state
            if (!(crouchHeld || belowCeiling))
            {
                stateMachine.ChangeState(stateMachine.PrevState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            belowCeiling = character.CheckCollisionOverlap(character.transform.position + Vector3.up * character.NormalColliderHeight);
        }
    }
}