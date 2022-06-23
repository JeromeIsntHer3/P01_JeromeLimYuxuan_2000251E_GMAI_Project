using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class SeekState : GeneralNPCState
    {
        private float speed = 5f;
        public SeekState(NPC npc, NPCStateMachine stateMachine) : base(npc, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            npc.agent.speed = speed;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            npc.SetNPCAnimation(speed, 0.1f);
            npc.agent.SetDestination(npc.target.position);
            if (npc.PlayerNPCDist() <= 3f)
            {
                npc.agent.ResetPath();
                stateMachine.ChangeState(npc.attack);
            }
            if(npc.PlayerNPCDist() > 10f)
            {
                npc.agent.ResetPath();
                stateMachine.ChangeState(npc.idle);
            }
        }
    }
}