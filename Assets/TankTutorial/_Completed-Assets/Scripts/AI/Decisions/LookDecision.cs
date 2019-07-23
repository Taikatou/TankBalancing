﻿using UnityEngine;

namespace Assets.TankTutorial.Scripts.AI.Decisions
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
    public class LookDecision : StateDecision
    {

        public override bool Decide(StateController controller)
        {
            bool targetVisible = Look(controller);
            return targetVisible;
        }

        private bool Look(StateController controller)
        {
            RaycastHit hit;

            Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.enemyStats.lookRange, Color.green);

            if (Physics.SphereCast(controller.eyes.position, controller.enemyStats.lookSphereCastRadius, controller.eyes.forward, out hit, controller.enemyStats.lookRange)
                && hit.collider.CompareTag("Player"))
            {
                controller.chaseTarget = hit.transform;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}