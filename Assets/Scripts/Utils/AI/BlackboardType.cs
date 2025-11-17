namespace Utils.AI
{
    public interface IBlackboardType
    {
    
    }

    public class BlackboardType<T> : IBlackboardType
    {
        private T Value { get; set; }
        
        public BlackboardType()
        {
        
        }

        public BlackboardType(T value)
        {
            SetValue(value);
        }

        public void SetValue(T value)
        {
            Value = value;
        }

        public T GetValue()
        {
            return Value;
        }
    }
}