using System.Collections.Generic;
using Assets.TankTutorial.Scripts.Tank;
using Complete;
using MLAgents;
using System.Linq;
using System.Collections;
using UnityEngine;
using Assets.TankTutorial.Scripts.AI;

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

        public int degrees = 360;

        public Transform spawnObjects;

        public List<GameObject> spawnTypes;

        public bool RewardTime = false;

        private List<GameObject> _tanks;

        public List<GameObject> Tanks => _tanks;

        // Depending on this value, the ai's spawned will have different AI
        private int _config;

        public List<Transform> WayPointList;

        public override void InitializeAgent()
        {
            base.InitializeAgent();
            _rayPer = GetComponent<RayPerception>();
            _tankMovement = GetComponent<TankMovement>();
            _tankShooting = GetComponent<TankShooting>();
            _mRigidbody = GetComponent<Rigidbody>();

            _tanks = new List<GameObject>();

            _config = Random.Range(0, 3);
        }

        public override void CollectObservations()
        {
            float rayDistance = 100f;

            float[] rayAngles = new float[rays];
            for (var i = 0; i < rays; i++)
            {
                rayAngles[i] = i * (degrees / rays);
            }
            var detectableObjects = new[] { "tank", "wall", "bullet" };
            

            AddVectorObs(_mRigidbody.transform.position);
            AddVectorObs(_mRigidbody.transform.rotation.y);
            AddVectorObs(_tankShooting.AllowSpawn);

            List<float> observations1 = _rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 1f, 0f);
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

            _tankMovement.UpdateAgent(moveForward, turn);

            bool shouldShoot = GetDecision(vectorAction[2]) == 1;

            if (shouldShoot)
            {
                _tankShooting.Fire(30.0f);
            }
            if (RewardTime)
            {
                float timePunishment = -1f / agentParameters.maxStep;
                // Penalty given each step to encourage agent to finish task quickly.
                AddReward(timePunishment);
            }
        }

        public override void AgentReset()
        {
            TankSpawn resetAgent = GetComponent<TankSpawn>();
            resetAgent?.Reset();

            TankShooting tankShooting = GetComponent<TankShooting>();
            tankShooting?.Reset();

            Spawn();
        }

        public GameObject GetSpawnTye()
        {
            int index = Random.Range(0, _config) % spawnTypes.Count;
            return spawnTypes[index];
        }

        public void Spawn()
        {
            foreach (GameObject tank in _tanks)
            {
                Destroy(tank);
            }
            foreach (Transform child in spawnObjects)
            {
                GameObject tank = Instantiate(GetSpawnTye(), child.position, child.rotation);
                StateController sController = tank.GetComponent<StateController>();
                if(sController)
                {
                    sController.WayPointList = WayPointList;
                }
                _tanks.Add(tank);
            }
        }
    }
}

