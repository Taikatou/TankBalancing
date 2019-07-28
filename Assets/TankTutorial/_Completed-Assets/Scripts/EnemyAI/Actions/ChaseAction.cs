using UnityEngine;

namespace Assets.TankTutorial.Scripts.AI.Actions
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
    public class ChaseAction : AIAction
    {
        public override void Act(StateController controller)
        {
            Chase(controller);
        }

        private void Chase(StateController controller)
        {
            controller.navMeshAgent.destination = controller.chaseTarget.position;
            controller.navMeshAgent.stoppingDistance = 0;
            controller.navMeshAgent.isStopped = false;
        }
    }
}