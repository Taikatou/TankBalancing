﻿using UnityEngine;

namespace Assets.TankTutorial.Scripts.AI
{
    [CreateAssetMenu(menuName = "PluggableAI/State")]
    public class State : ScriptableObject
    {
        public string StateName;
        public AIAction[] actions;
        public Transition[] transitions;
        public Color sceneGizmoColor = Color.grey;

        public void UpdateState(StateController controller)
        {
            DoActions(controller);
            CheckTransitions(controller);
        }

        private void DoActions(StateController controller)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                actions[i].Act(controller);
            }
        }

        private void CheckTransitions(StateController controller)
        {
            for (int i = 0; i < transitions.Length; i++)
            {
                bool decisionSucceeded = transitions[i].decision.Decide(controller);

                if (decisionSucceeded)
                {
                    Debug.Log("Decide transition change");
                    controller.TransitionToState(transitions[i].trueState);
                }
                else
                {
                    controller.TransitionToState(transitions[i].falseState);
                }
            }
        }


    }
}