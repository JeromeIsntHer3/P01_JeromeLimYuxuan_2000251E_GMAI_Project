using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //Transitions only from Damage State when the character's currhealth
    //drops to or below zero 
    public class DeadState : DamageState
    {
        //Check if the player wants to restart the game
        private bool restart; 
        public DeadState(Character character,PlayerStateMachine stateMachine): base(character,stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            DisplayOnUI(UIManager.Alignment.Left);
            //Set the anim bool of the dying animation
            //so that the character death will play when the dead
            //state is entered
            character.SetAnimationBool(character.isDead, true);
        }

        public override void HandleInput()
        {
            base.HandleInput();
            //To take in player input to check they want to restart the scene
            restart = Input.GetKeyDown(KeyCode.R);
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //if the player inputs restart to be true
            //the scene will reloaded, restarting the level
            if (restart)
            {
                SceneManager.LoadScene("Main");
            }
        }
    }
}