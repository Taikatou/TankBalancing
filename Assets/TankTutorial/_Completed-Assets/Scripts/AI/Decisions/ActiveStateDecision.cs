using UnityEngine;

namespace Assets.TankTutorial.Scripts.AI.Decisions
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/ActiveState")]
    public class ActiveStateDecision : StateDecision
    {
        public override bool Decide(StateController controller)
        {
            bool chaseTargetIsActive = controller.chaseTarget.gameObject.activeSelf;
            return chaseTargetIsActive;
        }
    }
}