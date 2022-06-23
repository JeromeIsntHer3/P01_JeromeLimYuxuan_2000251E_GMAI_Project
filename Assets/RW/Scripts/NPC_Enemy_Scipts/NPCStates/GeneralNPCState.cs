using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class GeneralNPCState : NPCState
    {
        public GeneralNPCState(NPC npc, NPCStateMachine stateMachine) : base(npc, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (npc.currHealth<npc.prevHealth && npc.isHit)
            {
                Debug.Log("Transitioning to Damaged State");
                stateMachine.ChangeState(npc.damaged);
            }
        }
    }
}