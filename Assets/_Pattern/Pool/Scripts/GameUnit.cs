using UnityEngine;

namespace _Framework.Pool.Scripts
{
    public class GameUnit : MonoBehaviour
    {
        private Transform _tf;
        public Transform TF
        {
            get
            {
                if (_tf == null)
                {
                    _tf = transform;
                }
                return _tf;
            }
        }

        public PoolType poolType;
        
        // TODO: Add register and remove events to OnInit and OnDestroy methods
    }
}