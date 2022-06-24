using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //The Seek State transitions from Idle, Patrol and Damaged State.
    //Transitions from Idle state if the player is within the seek range of the enemy npc
    //Transition from Patrol State if the player is within the seek range of the enemy npc
    //Transitions from the Damaged State if it was the previous state and the enemy npc had been damaged
    public class EnemyNPCSeekState : EnemyNPCGeneralState
    {
        private float speed = 5f;
        public EnemyNPCSeekState(NPC npc, NPCStateMachine stateMachine) : base(npc, stateMachine) { }

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