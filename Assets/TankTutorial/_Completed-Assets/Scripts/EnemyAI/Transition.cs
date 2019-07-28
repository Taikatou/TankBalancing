namespace Assets.TankTutorial.Scripts.AI
{
    [System.Serializable]
    public class Transition
    {
        public StateDecision decision;
        public State trueState;
        public State falseState;
    }
}