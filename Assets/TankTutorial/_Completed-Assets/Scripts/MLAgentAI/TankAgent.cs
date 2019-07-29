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
            var detectableObjects = new[] { "tank", "wall" };

            const float rayDistance = 35f;
            float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
            float[] rayAngles1 = { 25f, 95f, 165f, 50f, 140f, 75f, 115f };
            float[] rayAngles2 = { 15f, 85f, 155f, 40f, 130f, 65f, 105f };

            AddVectorObs(_tankShooting.AllowSpawn);

            AddVectorObs(transform.InverseTransformDirection(_mRigidbody.velocity));

            AddVectorObs(_rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
            AddVectorObs(_rayPer.Perceive(rayDistance, rayAngles1, detectableObjects, 0f, 5f));
            AddVectorObs(_rayPer.Perceive(rayDistance, rayAngles2, detectableObjects, 0f, 10f));
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
                _tankShooting.Fire(35.0f);
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

        private int SpawnIndex()
        {
            bool useRandom = false;
            if(useRandom)
            {
                int index = Random.Range(0, _config) % spawnTypes.Count;
                return index;
            }
            else
            {
                int index = 1;
                return index;
            }
        }

        public GameObject GetSpawnTye()
        {
            int index = SpawnIndex();
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
                StateController sController = tank.GetComponentInChildren<StateController>();
                if(sController)
                {
                    sController.WayPointList = WayPointList;
                }
                TankSpawn tSpawn = tank.GetComponentInChildren<TankSpawn>();
                if(tSpawn)
                {
                    tSpawn.StartPosition = child;
                }
                _tanks.Add(tank);
            }
        }
    }
}

