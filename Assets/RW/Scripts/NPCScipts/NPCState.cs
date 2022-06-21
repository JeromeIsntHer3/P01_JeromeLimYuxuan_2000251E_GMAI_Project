namespace RayWenderlich.Unity.StatePatternInUnity
{
    //Abstract is used to make the class "virtual", not creating an instance of the
    //class, since we only need to refer to this class for variables that the
    //concrete states will use.
    public abstract class NPCState
    {
        //"Protected" is used so that children of this class can the variable

        //Take in the character class so that we can refer to the variables,
        //properties, methods and Start and Update function where the code
        //will actually run
        protected NPC npc;
        //Take in the StateMachine Class/Factory so that we can instantiate/set
        //which state we want the StateMachine to be in
        protected NPCStateMachine stateMachine;

        protected NPCState(NPC npc, NPCStateMachine stateMachine)
        {
            this.npc = npc;
            this.stateMachine = stateMachine;
        }

        protected void DisplayOnUI(UIManager.Alignment alignment)
        {
            UIManager.Instance.DisplayN(this, alignment);
        }

        public virtual void Enter() { DisplayOnUI(UIManager.Alignment.Right); }
        public virtual void LogicUpdate() { }
        public virtual void PhysicsUpdate() { }
        public virtual void Exit() { }
    }
}
