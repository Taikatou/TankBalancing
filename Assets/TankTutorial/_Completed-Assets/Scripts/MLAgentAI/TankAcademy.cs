using System.Collections.Generic;
using MLAgents;
using UnityEngine;

namespace Assets.TankTutorial.Scripts.MLAgentAI
{
    public class TankAcademy : Academy
    {
        public Transform spawnObjects;

        public GameObject spawnType;

        private List<GameObject> _tanks;

        public void Spawn()
        {
            foreach(Transform child in spawnObjects)
            {
                GameObject tank = Instantiate(spawnType, child.position, child.rotation);
                _tanks.Add(tank);
            }
        }

        public override void AcademyReset()
        {
            if (_tanks == null)
            {
                _tanks = new List<GameObject>();
            }
            else
            {
                foreach (GameObject tank in _tanks)
                {
                    Destroy(tank);
                }
            }
            TankAgent[] tanks = Resources.FindObjectsOfTypeAll<TankAgent>();
            foreach (var tank in tanks)
            {
                tank.Done();
            }
            Spawn();

        }
    }
}
