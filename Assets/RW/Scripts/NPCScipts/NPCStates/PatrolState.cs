using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class PatrolState : NPCState
    {
        private float walkRadius = 20;
        private float seekRange = 10f;
        private float speed = 3f;
        int walkableArea = 1;

        public PatrolState(NPC npc, NPCStateMachine stateMachine) : base(npc, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            npc.agent.speed = speed;
            npc.agent.SetDestination(NextRandomNavMeshLocation());
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            npc.SetNPCAnimation(speed, 0.1f);
            if(npc.agent.remainingDistance == 0 && npc.agent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                stateMachine.ChangeState(npc.idle);
            }
            else if (npc.PlayerNPCDist() <= seekRange)
            {
                stateMachine.ChangeState(npc.seek);
            }
        }

        public Vector3 NextRandomNavMeshLocation()
        {
            Vector3 finalPos = Vector3.zero;
            Vector3 randomPos = Random.insideUnitSphere * walkRadius;
            randomPos += npc.transform.position;
            if(NavMesh.SamplePosition(randomPos, out NavMeshHit hit, walkRadius, walkableArea))
            {
                finalPos = hit.position;
            }
            return finalPos;
        }
    }
}