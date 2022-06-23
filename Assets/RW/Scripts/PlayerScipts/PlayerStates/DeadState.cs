using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //DeadState transitions from Damage State if the character health is 0
    public class DeadState : DamageState
    {
        private bool restart; 
        public DeadState(Character character,PlayerStateMachine stateMachine): base(character,stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            DisplayOnUI(UIManager.Alignment.Left);
            character.SetAnimationBool(character.isDead, true);
        }

        public override void HandleInput()
        {
            base.HandleInput();
            restart = Input.GetKeyDown(KeyCode.R);
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (restart)
            {
                SceneManager.LoadScene("Main");
            }
        }
    }
}