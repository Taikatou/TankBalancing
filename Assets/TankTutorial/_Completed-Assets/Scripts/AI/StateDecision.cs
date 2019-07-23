using UnityEngine;

namespace Assets.TankTutorial.Scripts.AI
{
    public abstract class StateDecision : ScriptableObject
    {
        public abstract bool Decide(StateController controller);
    }
}