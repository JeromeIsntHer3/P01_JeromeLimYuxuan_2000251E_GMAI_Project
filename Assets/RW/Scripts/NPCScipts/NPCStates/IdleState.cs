using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class IdleState : GeneralNPCState
    {
        private bool timerOver;
        private float timerTime;
        private float currTime;
        private float seekRange = 10f;

        public IdleState(NPC npc, NPCStateMachine stateMachine) : base(npc, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            timerOver = false;
            npc.agent.ResetPath();
            timerTime = Random.Range(4f, 20f);
            currTime = timerTime;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            npc.SetNPCAnimation(0,0.1f);
            Timer();
            if (npc.PlayerNPCDist() <= seekRange)
            {
                stateMachine.ChangeState(npc.seek);
            }
            if (timerOver)
            {
                switch (npc.nPCType)
                {
                    case (NPC.NPCType.Idle):
                        stateMachine.Initialize(npc.idle);
                        break;
                    case (NPC.NPCType.Patrol):
                        stateMachine.ChangeState(npc.patrol);
                        break;
                    default:
                        stateMachine.Initialize(npc.idle);
                        break;
                }
            }
        }

        public void Timer()
        {
            if(currTime > 0)
            {
                currTime -= Time.deltaTime;
                if(currTime <= 0)
                {
                    timerOver = true;
                }
            }
        }
    }
}