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
    //The DuckingState transitions from and to the SheathState according
    //to if the Crouch Button (Shift/Middle-Mouse Press) is pressed
    public class DuckingState : GroundedState
    {
        private bool belowCeiling;
        private bool crouchHeld;

        public DuckingState(Character character, PlayerStateMachine stateMachine) : base(character, stateMachine){ }

        public override void Enter()
        {
            base.Enter();
            DisplayOnUI(UIManager.Alignment.Left);
            //When The DuckingState is entered:
            //1. Set the Crouching Anim Param to true
            character.SetAnimationBool(character.crouchParam, true);
            //2. Set the new speed(rotation) to the PlayerData_Crouch_Variation
            speed = character.CrouchSpeed;
            rotationSpeed = character.CrouchRotationSpeed;
            //3. Set the new collider size to the PlayerData_CrouchCollider_Height
            character.ColliderSize = character.CrouchColliderHeight;
            //4. Set belowCeiling to false until a ceiling is detected
            belowCeiling = false;
        }

        public override void Exit()
        {
            base.Exit();
            //When transitioning out from the DuckingState:
            //1. Set the Crouch Anim Param to false to stop the animation
            character.SetAnimationBool(character.crouchParam, false);
            //2. Set the collider size to the original collider size
            character.ColliderSize = character.NormalColliderHeight;
        }

        public override void HandleInput()
        {
            base.HandleInput();
            //Set crouchHeld to true if Shift/Middle-Mouse pressed
            crouchHeld = Input.GetButton("Fire3");
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //If Shift/Middle-Mouse is no longer being press and the character
            //is not a collidable ceiling, transition back to the PrevState
            //if it is not the jumping state
            if (!(crouchHeld || belowCeiling))
            {
                stateMachine.ChangeState(stateMachine.PrevState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            //Set the beloweCeiling bool to tree if the collider of the character meets/overlaps
            //another collider above the character
            belowCeiling = character.CheckCollisionOverlap(character.transform.position + Vector3.up * character.NormalColliderHeight);
        }
    }
}