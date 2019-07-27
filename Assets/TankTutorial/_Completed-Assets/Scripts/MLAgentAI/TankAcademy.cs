using System.Collections.Generic;
using MLAgents;
using UnityEngine;

namespace Assets.TankTutorial.Scripts.MLAgentAI
{
    public class TankAcademy : Academy
    {
        public bool RewardShots = false;

        public override void InitializeAcademy()
        {
            Monitor.SetActive(true);
        }
    }
}
