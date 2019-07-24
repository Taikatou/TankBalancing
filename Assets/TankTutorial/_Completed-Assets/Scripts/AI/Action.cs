using UnityEngine;

namespace Assets.TankTutorial.Scripts.AI
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Act(StateController controller);
    }
}