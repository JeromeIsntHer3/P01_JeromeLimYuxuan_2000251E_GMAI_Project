using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //Damaged State is a state that gets transition into from any another state if the enemy npc is damaged which
    //will transition back to any previous state
    public class EnemyNPCDamagedState : NPCState
    {
        public EnemyNPCDamagedState(NPC npc, NPCStateMachine stateMachine) : base(npc, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            //Trigger the hit/hurt animation on the enemy npc
            npc.TriggerAnimation(npc.hit);
            //Check if the enemy npc's current health is or lesser than 0
            //and if it has been damaged by any other character type
            //play the dead/dying animation
            if (npc.currHealth <= 0 && npc.isHit)
            {
                npc.SetAnimationBool(npc.isDead, true);
            }
            //Set the npc is hit to false as it has just been Damaged so that
            //it can be damaged again once exiting out from this state
            npc.isHit = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //if npc remain in damaged state for more than 4 secs it will
            //be destroyed as it is presumed to be dead
            npc.DestroyNPC(4f);
            //Check to see if the health is greater than 0 and npc is no longer 
            //being hit which was set in Enter() if this state, then transition
            //to whichever state it was previously in
            if (npc.currHealth > 0 && !npc.isHit)
            {
                stateMachine.ChangeState(stateMachine.PrevState);
            }
        }
    }
}