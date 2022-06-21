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
        }
    }
}