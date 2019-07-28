using UnityEngine;

namespace Assets.TankTutorial.Scripts.AI.Actions
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
    public class PatrolAction : AIAction
    {
        public override void Act(StateController controller)
        {
            Patrol(controller);
        }

        private void Patrol(StateController controller)
        {
            controller.navMeshAgent.destination = controller.WayPointList[controller.nextWayPoint].position;
            controller.navMeshAgent.isStopped = false;

            if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)
            {
                controller.nextWayPoint = (controller.nextWayPoint + 1) % controller.WayPointList.Count;
            }
        }
    }
}