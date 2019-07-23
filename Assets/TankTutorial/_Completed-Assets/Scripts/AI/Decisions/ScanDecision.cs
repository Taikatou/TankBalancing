using UnityEngine;

namespace Assets.TankTutorial.Scripts.AI.Decisions
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/Scan")]
    public class ScanDecision : StateDecision
    {
        public override bool Decide(StateController controller)
        {
            bool noEnemyInSight = Scan(controller);
            return noEnemyInSight;
        }

        private bool Scan(StateController controller)
        {
            controller.navMeshAgent.Stop();
            controller.transform.Rotate(0, controller.enemyStats.searchingTurnSpeed * Time.deltaTime, 0);
            return controller.CheckIfCountDownElapsed(controller.enemyStats.searchDuration);
        }
    }
}