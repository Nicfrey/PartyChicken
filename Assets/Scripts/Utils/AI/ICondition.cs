namespace Utils.AI
{
    public interface ICondition
    {
        public bool Evaluate(Blackboard blackboard);
    }
}
