using MLAgents;
using UnityEngine;
using System.Collections;

namespace Assets.TankTutorial.Scripts.MLAgentAI
{
    public class TankAcademy : Academy
    {
        public bool RewardShots = false;

        public bool Respawn = false;

        public bool RandomAI = false;

        private int _spawnType = 0;

        public int SpawnType => _spawnType;

        public override void InitializeAcademy()
        {
            Monitor.SetActive(true);
            if (RandomAI)
            {
                _spawnType = Random.Range(0, 3);
            }
        }
    }
}
