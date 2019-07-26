using Complete;
using UnityEngine;

namespace Assets.TankTutorial.Scripts.Tank
{
    public class TankSpawn : MonoBehaviour
    {
        public Transform startPosition;

        // Start is called before the first frame update
        public void Reset()
        {
            Rigidbody rBody = GetComponent<Rigidbody>();
            rBody.MovePosition(startPosition.position);
            TankHealth t = GetComponent<TankHealth>();
            t.ResetHealth();
        }
    }
}
