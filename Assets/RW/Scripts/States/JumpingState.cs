﻿/*
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
    //JumpingState transtions from and to SheathState and DrawnState
    //according to if the player wants to chat
    
    public class JumpingState : State
    {
        private bool grounded;
        private int jumpParam = Animator.StringToHash("Jump");
        private int landParam = Animator.StringToHash("Land");

        public JumpingState(Character character, StateMachine stateMachine) : base(character, stateMachine){ }

        public override void Enter()
        {
            base.Enter();
            //Take in an Instance of the SoundManager 
            SoundManager.Instance.PlaySound(SoundManager.Instance.jumpSounds);
            grounded = false;
            //Activate the Jump function
            Jump();
            //If the PrevState is drawn, Invoke the SheathWeapon from the character
            //with delay to get the weapon to transform in the similar timing
            if (stateMachine.PrevState == character.drawn)
            {
                character.TriggerAnimation(character.sheathParam);
                character.Invoke("SheathWeapon", 0.25f);
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //If player is grounded:
            //1. Trigger the Land Param for the landing animation
            //2. Use the SoundManager Instance to play the sound
            //3. Transition to the PrevState
            if (grounded)
            {
                character.TriggerAnimation(landParam);
                SoundManager.Instance.PlaySound(SoundManager.Instance.landing);
                stateMachine.ChangeState(stateMachine.PrevState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            //Check if the character's collision overlaps the ground collision
            grounded = character.CheckCollisionOverlap(character.transform.position);
        }

        private void Jump()
        {
            //Send the character upwards
            character.transform.Translate(Vector3.up *(character.CollisionOverlapRadius + 0.1f));
            //Add the upwards force according to the player data jump force
            character.ApplyImpulse(Vector3.up * character.JumpForce);
            //Trigger the Jump Param to play the animation
            character.TriggerAnimation(jumpParam);
        }
    }
}