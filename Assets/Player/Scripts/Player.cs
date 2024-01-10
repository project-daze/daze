using UnityEngine;
using Daze.P.Actions;
using Daze.P.Characters;

namespace Daze.P
{
    public class Player : MonoBehaviour
    {
        public Character Character;
        public Transform Camera;

        public Action Action = new();

        private void Awake()
        {
            Character.OnAwake(this);
        }

        private void OnEnable()
        {
            Action.OnEnable();
        }

        private void OnDisable()
        {
            Action.OnDisable();
        }

        private void LateUpdate()
        {
            Character.OnLateUpdate();
        }
    }
}
