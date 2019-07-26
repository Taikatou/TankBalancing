using System.Collections.Generic;
using Assets.TankTutorial.Scripts.Tank;
using Complete;
using MLAgents;
using UnityEngine;

namespace Assets.TankTutorial.Scripts.MLAgentAI
{
    public class TankAgent : Agent
    {
        RayPerception rayPer;
        TankMovement tankMovement;
        TankShooting tankShooting;
        Rigidbody m_Rigidbody;

        public string Name;

        public float ShotLimit = 0.05f;

        public override void InitializeAgent()
        {
            base.InitializeAgent();
            rayPer = GetComponent<RayPerception>();
            tankMovement = GetComponent<TankMovement>();
            tankShooting = GetComponent<TankShooting>();
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        public override void CollectObservations()
        {
            float rayDistance = 100f;
            float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 110f, 70f, 225f, 270f, 315f};
            var detectableObjects = new[] { "tank", "wall", "bullet" };
            List<float> observations1 = rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 1f, 0f);
            AddVectorObs(m_Rigidbody.transform.rotation.y);
            AddVectorObs(tankShooting.AllowSpawn);
            AddVectorObs(observations1);
        }

        /// <summary>
        /// Called every step of the engine. Here the agent takes an action.
        /// </summary>
        ///
        private int GetDecision(float input)
        {
            int action = Mathf.FloorToInt(input);

            int output = 0;
            switch (input)
            {
                case 1:
                    output = 1;
                    break;
                case 2:
                    output = -1;
                    break;
            }
            return output;
        }

        public override void AgentAction(float[] vectorAction, string textAction)
        {
            float moveForward = GetDecision(vectorAction[0]);

            float turn = GetDecision(vectorAction[1]);

            // Penalty given each step to encourage agent to finish task quickly.

            tankMovement.UpdateAgent(moveForward, turn);

            bool shouldShoot = GetDecision(vectorAction[2]) == 1;

            if (shouldShoot)
            {
                tankShooting.Fire(30.0f);
            }
        }

        public override void AgentReset()
        {
            TankSpawn resetAgent = GetComponent<TankSpawn>();
            resetAgent.Reset();
        }
    }
}

