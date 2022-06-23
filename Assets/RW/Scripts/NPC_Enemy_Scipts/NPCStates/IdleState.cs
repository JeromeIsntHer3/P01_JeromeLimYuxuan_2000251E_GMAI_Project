using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class IdleState : GeneralNPCState
    {
        private bool canMoveAgain;
        private float timeBeforeNextMove = Random.Range(4f, 20f);
        private float seekRange = 10f;
        private FunctionTimer ft;

        public IdleState(NPC npc, NPCStateMachine stateMachine) : base(npc, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            npc.agent.ResetPath();
            canMoveAgain = false;
            ft = new FunctionTimer(CanMoveAgain,timeBeforeNextMove);
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            npc.SetNPCAnimation(0,0.1f);
            ft.Update();
            if (npc.PlayerNPCDist() <= seekRange)
            {
                stateMachine.ChangeState(npc.seek);
            }
            if (canMoveAgain)
            {
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

        public void CanMoveAgain()
        {
            canMoveAgain = true;
        }
    }
}