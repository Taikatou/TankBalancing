﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.TankTutorial.Scripts.AI
{
    public class StateController : MonoBehaviour
    {

        public State currentState;
        public EnemyStats enemyStats;
        public Transform eyes;
        public State remainState;


        [HideInInspector] public NavMeshAgent navMeshAgent;
        [HideInInspector] public Complete.TankShooting tankShooting;
        [HideInInspector] public int nextWayPoint;
        [HideInInspector] public Transform chaseTarget;
        [HideInInspector] public float stateTimeElapsed;

        public bool aiActive;

        [HideInInspector] public List<Transform> WayPointList;

        void Awake()
        {
            tankShooting = GetComponent<Complete.TankShooting>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void SetupAI()
        {
            aiActive = true;
            if (aiActive)
            {
                navMeshAgent.enabled = true;
            }
            else
            {
                navMeshAgent.enabled = false;
            }
        }

        void Update()
        {
            if (!aiActive)
                SetupAI();
            currentState?.UpdateState(this);
        }

        void OnDrawGizmos()
        {
            if (currentState != null && eyes != null)
            {
                Gizmos.color = currentState.sceneGizmoColor;
                Gizmos.DrawWireSphere(eyes.position, enemyStats.lookSphereCastRadius);
            }
        }

        public void TransitionToState(State nextState)
        {
            if (nextState != remainState)
            {
                currentState = nextState;
                OnExitState();
            }
        }

        public bool CheckIfCountDownElapsed(float duration)
        {
            stateTimeElapsed += Time.deltaTime;
            return (stateTimeElapsed >= duration);
        }

        private void OnExitState()
        {
            stateTimeElapsed = 0;
        }
    }
}