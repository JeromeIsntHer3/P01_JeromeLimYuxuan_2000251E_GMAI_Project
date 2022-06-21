using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class AttackState : NPCState
    {
        public AttackState(NPC npc, NPCStateMachine stateMachine) : base(npc, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            npc.TriggerAnimation(npc.isClose);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            npc.SetNPCAnimation(0f, 0.1f);
        }
    }
}