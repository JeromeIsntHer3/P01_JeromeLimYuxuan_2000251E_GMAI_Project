using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //The general enemy npc state is used for the all the states except damaged state to derive from
    //to hold information that they all need to share at runtime
    public class EnemyNPCGeneralState : NPCState
    {
        public EnemyNPCGeneralState(NPC npc, NPCStateMachine stateMachine) : base(npc, stateMachine) { }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //From any state, if the enemy npc takes damages
            //and isHit, will transition into the damaged
            //state
            if (npc.currHealth<npc.prevHealth && npc.isHit)
            {
                Debug.Log("Transitioning to Damaged State");
                stateMachine.ChangeState(npc.damaged);
            }
        }
    }
}