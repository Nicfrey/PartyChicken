namespace Utils.AI
{
    public interface IState
    {
        public void OnEnter(Blackboard blackboard);
        public void OnExit(Blackboard blackboard);
        public void Update(Blackboard blackboard);
    }
}
