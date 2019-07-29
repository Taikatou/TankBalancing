using System.Collections.Generic;
using MLAgents;
using UnityEngine;

namespace Assets.TankTutorial.Scripts.MLAgentAI
{
    public class TankAcademy : Academy
    {
        public bool RewardShots = false;

        public bool Respawn = false;

        public bool Random = false;

        private int _spawnType = 0;

        public int SpawnType => _spawnType;

        public override void InitializeAcademy()
        {
            Monitor.SetActive(true);
            if (Random)
            {
                _spawnType = 1;
            }
        }
    }
}
