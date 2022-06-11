using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class DamageState : StandingState
    {
        private bool hit = false;
        private bool dead;

        private float stunTimer = 1f;
        private float currTime;

        public DamageState(Character character, StateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            currTime = stunTimer;
            speed = rotationSpeed = 0.0f;
            hit = true;
            dead = false;
            canAttack = false;
            if (hb.playerHealth <= 0)
            {
                dead = true;
            }
            if (!dead)
            {
                character.TriggerAnimation(character.hit);
            }
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            currTime -= 1 * Time.deltaTime;

            if (hit && currTime <= 0 && !dead)
            {
                stateMachine.ChangeState(stateMachine.PrevState);
            }
            else if (dead)
            {
                stateMachine.ChangeState(character.dead);
            }
        }
    }
}