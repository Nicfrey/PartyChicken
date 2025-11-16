using System;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

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
            try
            {
                currentState.Update(blackboard);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            var currentTransition = transitions.GetValueOrDefault(currentState);
            if (currentTransition != null)
            {
                foreach (var transition in currentTransition)
                {
                    if (transition.Key.Evaluate(blackboard))
                    {
                        Debug.Log($"Transitioning from {currentState} to {transition.Value} because {transition.Key} evaluated to true.");
                        ChangeState(transition.Value);
                    }
                }
            }
            
        }

        private void ChangeState(IState newState)
        {
            if(currentState != null)
                currentState.OnExit(blackboard);

            currentState = newState;
            currentState.OnEnter(blackboard);
        }
        
        public IState GetCurrentState()
        {
            return currentState;
        }
    }
}
