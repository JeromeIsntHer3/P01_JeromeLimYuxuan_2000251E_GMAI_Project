using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class DamagedState : NPCState
    {
        public DamagedState(NPC npc, NPCStateMachine stateMachine) : base(npc, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            npc.TriggerAnimation(npc.hit);
            if (npc.currHealth <= 0 && npc.isHit)
            {
                npc.SetAnimationBool(npc.isDead, true);
            }
            npc.isHit = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            npc.DestroyNPC(4f);
            if (npc.currHealth > 0 && !npc.isHit)
            {
                stateMachine.ChangeState(stateMachine.PrevState);
            }
        }
    }
}