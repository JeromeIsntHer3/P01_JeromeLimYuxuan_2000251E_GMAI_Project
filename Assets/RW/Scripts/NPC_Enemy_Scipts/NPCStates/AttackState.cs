using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //Attack State derives from GeneralNPCState and transitions to/from DamagedState and SeekState
    public class AttackState : GeneralNPCState
    {
        //To Check if the NPC is able to attack
        private bool canAttackAgain;
        //Time before it can attack again
        private float timeBetweenAttack = 2f;
        //To hold a new function timer
        private FunctionTimer ft;

        public AttackState(NPC npc, NPCStateMachine stateMachine) : base(npc, stateMachine) { }

        public override void Enter()
        {
            base.Enter();                                         
            npc.TriggerAnimation(npc.isClose);                    //The Sword attack animation is played to attack the player
            ft = new FunctionTimer(Timer,timeBetweenAttack);      //Set the canAttackAgain to true after a few seconds to allow
            canAttackAgain = false;                               //the enemy npc to attack again
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();                                   
            npc.SetNPCAnimation(0f, 0.1f);                        //Set the movement animation to stop and blend to smooth out
            ft.Update();                                          //the stopping of the animation
            if (canAttackAgain)                                   //Check to see if the player canAttackAgain, then sent back
            {                                                     //to seek state which will return to attack state if the enemy
                stateMachine.ChangeState(npc.seek);               //is close enough to the player
            }                                                     
        }                                                         

        public void Timer()                                       //Timer method that the TimerFunction will take in
        {                                                         //and will run it after a few seconds according to
            canAttackAgain = true;                                //timeBetweenAttack
        }                                                         
    }                                                             
}