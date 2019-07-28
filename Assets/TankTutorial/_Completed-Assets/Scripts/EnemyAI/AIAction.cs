using UnityEngine;

namespace Assets.TankTutorial.Scripts.AI
{
    public abstract class AIAction : ScriptableObject
    {
        public abstract void Act(StateController controller);
    }
}