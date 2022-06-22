using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class AttackState : GeneralNPCState
    {
        private bool timerOver;
        private float timerTime = 2f;
        private float currTime;

        public AttackState(NPC npc, NPCStateMachine stateMachine) : base(npc, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            npc.TriggerAnimation(npc.isClose);
            timerOver = false;
            currTime = timerTime;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            npc.SetNPCAnimation(0f, 0.1f);
            Timer();
            if (timerOver)
            {
                stateMachine.ChangeState(stateMachine.PrevState);
            }
        }

        public void Timer()
        {
            if (currTime > 0)
            {
                currTime -= Time.deltaTime;
                if (currTime <= 0)
                {
                    timerOver = true;
                }
            }
        }
    }
}