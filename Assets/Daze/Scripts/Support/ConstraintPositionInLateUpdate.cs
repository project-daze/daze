using UnityEngine;

namespace Daze.Support
{
    public class ConstraintPositionInLateUpdate : MonoBehaviour
    {
        public Transform Target;

        public void LateUpdate()
        {
            transform.position = Target.position;
        }
    }
}
