using System.Collections.Generic;
using System.Transactions;

namespace Utils.AI
{
    using TransitionStatePair = KeyValuePair<ICondition, IState>;
    using Transitions = List<KeyValuePair<ICondition, IState>>;
    
    public class FiniteStateMachine
    {
        private IState currentState;
        private Blackboard blackboard;
        private Dictionary<IState, Transitions> transitions;
        
        public FiniteStateMachine(IState initialState, Blackboard blackboard)
        {
            this.currentState = initialState;
            this.blackboard = blackboard;
            this.transitions = new Dictionary<IState, Transitions>();
            ChangeState(initialState);
        }

        public void AddTransition(IState startState, IState endState, ICondition condition)
        {
            if (!transitions.ContainsKey(startState))
            {
                transitions[startState] = new Transitions();
            }
            transitions[startState].Add(new TransitionStatePair(condition, endState));
        }
        
        public Blackboard GetBlackboard()
        {
            return blackboard;
        }
    
        public void Update()
        {
            var currentTransition = transitions.GetValueOrDefault(currentState);
            if (currentTransition != null)
            {
                foreach (var transition in currentTransition)
                {
                    if (transition.Key.Evaluate(blackboard))
                    {
                        ChangeState(transition.Value);
                        break;
                    }
                }
            }
            
        }

        private void ChangeState(IState newState)
        {
            currentState?.OnExit(blackboard);

            currentState = newState;
            currentState.OnEnter(blackboard);
        }
    }
}
