using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //The Idle State is the default state of the enemy npc and transitions
    //between the patrol state after a random amount of time has passed
    //and between damaged state when the enemy npc takes damage
    public class EnemyNPCIdleState : EnemyNPCGeneralState
    {
        //Check to see enemy npc can move
        private bool canMoveAgain;
        //Time before the enemy npc is allowed to move again
        private float timeBeforeNextMove = Random.Range(4f, 20f);
        //The distance at which the enemy npc will move towards the player
        private float seekRange = 10f;
        //Hold a new timer 
        private FunctionTimer ft;

        public EnemyNPCIdleState(NPC npc, NPCStateMachine stateMachine) : base(npc, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            //Reset the agent path in the navmesh to stop it from moving
            npc.agent.ResetPath();
            //Set can move again to false so that it does not transition
            //to other states until the timer is over
            canMoveAgain = false;
            //Allow the enemy npc to move again by setting the canMoveAgain bool
            //to true in a method call CanMoveAgain after the amount of time
            //in timeBeforeNextMove
            ft = new FunctionTimer(CanMoveAgain,timeBeforeNextMove);
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //Set the enemy npc movement animation to stop and blend it
            //so that it smooths out as it reaches 0
            npc.SetNPCAnimation(0,0.1f);
            //Run the update function the timer to start counting down
            //the time before the enemy npc can move again
            ft.Update();
            //PlayerNPCDist is a float function and calculates the distance
            //between the player and the enemy npc
            //If the the float returned is lesser than the seekRange of the
            //enemy npc the enemy npc will transition into the seek state
            if (npc.PlayerNPCDist() <= seekRange)
            {
                stateMachine.ChangeState(npc.seek);
            }
            if (canMoveAgain)
            {
                //If the enemy npc player can move, it will check for what type
                //of enemy npc it is and either transitions back to idle state to loop
                //through or transitions to patrol state
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

        //Function for the FunctionTimer to take in to set
        //the canMoveAgain to be true
        public void CanMoveAgain()
        {
            canMoveAgain = true;
        }
    }
}