using System;
using UnityEngine;

namespace Daze.Player.Avatar
{
    public class IkController : MonoBehaviour
    {
        public event Action OnAnimatorIk;

        public void OnAnimatorIK()
        {
            OnAnimatorIk?.Invoke();
        }
    }
}
