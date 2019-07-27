using System.Collections.Generic;
using Assets.TankTutorial.Scripts.Tank;
using Complete;
using MLAgents;
using UnityEngine;

namespace Assets.TankTutorial.Scripts.MLAgentAI
{
    public class TankAgent : Agent
    {
        private RayPerception _rayPer;
        private TankMovement _tankMovement;
        private TankShooting _tankShooting;
        private Rigidbody _mRigidbody;

        public string Name;

        public int rays = 8;

        public int degrees = 180;

        public float shotLimit = 0.05f;

        public Transform spawnObjects;

        public GameObject spawnType;

        private List<GameObject> _tanks;

        public List<GameObject> Tanks => _tanks;

        public override void InitializeAgent()
        {
            base.InitializeAgent();
            _rayPer = GetComponent<RayPerception>();
            _tankMovement = GetComponent<TankMovement>();
            _tankShooting = GetComponent<TankShooting>();
            _mRigidbody = GetComponent<Rigidbody>();

            _tanks = new List<GameObject>();
        }

        public override void CollectObservations()
        {
            float rayDistance = 100f;

            float[] rayAngles = new float[rays];
            for (var i = 0; i < rays; i++)
            {
                rayAngles[i] = i * (degrees / 8);
            }
            var detectableObjects = new[] { "tank", "wall", "bullet" };
            
            //List<float> observations1 = _rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 1f, 0f);
            //AddVectorObs(observations1);

            AddVectorObs(_mRigidbody.transform.position);
            AddVectorObs(_mRigidbody.transform.rotation.y);
            AddVectorObs(_tankShooting.AllowSpawn);
            foreach (var tank in Tanks)
            {
                AddVectorObs(tank.transform.position);
                TankHealth health = tank.GetComponentInChildren<TankHealth>();
                AddVectorObs(health.CurrentHealth);
            }
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

            _tankMovement.UpdateAgent(moveForward, turn);

            bool shouldShoot = GetDecision(vectorAction[2]) == 1;

            if (shouldShoot)
            {
                _tankShooting.Fire(30.0f);
            }
        }

        public override void AgentReset()
        {
            TankSpawn resetAgent = GetComponent<TankSpawn>();
            resetAgent.Reset();

            Spawn();
        }

        public void Spawn()
        {
            foreach (GameObject tank in _tanks)
            {
                Destroy(tank);
            }
            foreach (Transform child in spawnObjects)
            {
                GameObject tank = Instantiate(spawnType, child.position, child.rotation);
                _tanks.Add(tank);
            }
        }
    }
}

