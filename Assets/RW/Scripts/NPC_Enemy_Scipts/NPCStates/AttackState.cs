using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class AttackState : GeneralNPCState
    {
        private bool canAttackAgain;
        private float timeBetweenAttack = 2f;
        private FunctionTimer ft;

        public AttackState(NPC npc, NPCStateMachine stateMachine) : base(npc, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            npc.TriggerAnimation(npc.isClose);
            ft = new FunctionTimer(Timer,timeBetweenAttack);
            canAttackAgain = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            npc.SetNPCAnimation(0f, 0.1f);
            ft.Update();
            if (canAttackAgain)
            {
                stateMachine.ChangeState(npc.seek);
            }
        }

        public void Timer()
        {
            canAttackAgain = true;
        }
    }
}