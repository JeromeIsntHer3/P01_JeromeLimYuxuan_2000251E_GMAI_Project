using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //This class transitions from Idle, Seek and Damage States.
    //It transition from Idle when the timer within is over and it is of type patrol
    //It transition from Seek when the player character is outside of the seek Range
    //It transitons from Damage after the enemy npc has been damaged and it was the previous state
    public class EnemyNPCPatrolState : EnemyNPCGeneralState
    {
        //the max region where the enemy npc can randomly go to
        private float walkRadius = 20;
        //Distance which the enemy npc will spot the player character and approach them
        private float seekRange = 10f;
        //The speed of the enemy npc that is set to agent of it
        private float speed = 3f;
        //LayerMask of the navmesh
        int walkableArea = 1;

        public EnemyNPCPatrolState(NPC npc, NPCStateMachine stateMachine) : base(npc, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            //Set the agent speed to the speed float
            npc.agent.speed = speed;
            //Take in the RandomNavMeshlocation
            npc.agent.SetDestination(NextRandomNavMeshLocation());
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //Set the movement animation floats according to the speed
            //and add blending to it so that it doesn't animate rigidly
            npc.SetNPCAnimation(speed, 0.1f);
            //Remaining Distance of the path is 0 and the status of the path is also completed
            //return the player to the idle state if it distance between the player and the npc enemy
            //is greater than the seekRange. If it is within the seekRange it will transition to the 
            //seek state and move towards the player
            if(npc.agent.remainingDistance == 0 && npc.agent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                stateMachine.ChangeState(npc.idle);
            }
            else if (npc.PlayerNPCDist() <= seekRange)
            {
                stateMachine.ChangeState(npc.seek);
            }
        }

        //The method takes in the walkRadius distance and walkableArea LayerMask anad
        //get a random position around the enemy npc and return the position of the 
        //hit point which is now the new destination of the enemy npc
        public Vector3 NextRandomNavMeshLocation()
        {
            Vector3 finalPos = Vector3.zero;
            Vector3 thisPos = Random.insideUnitSphere * walkRadius;
            thisPos += npc.transform.position;
            if(NavMesh.SamplePosition(thisPos, out NavMeshHit hit, walkRadius, walkableArea))
            {
                finalPos = hit.position;
            }
            return finalPos;
        }
    }
}