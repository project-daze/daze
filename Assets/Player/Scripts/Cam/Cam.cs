using System;
using UnityEngine;
using Cinemachine;

namespace Daze.Player
{
    public class Cam : MonoBehaviour
    {
        [NonSerialized] public Controller Controller;

        public Transform Main;
        public CinemachineFreeLook Look;
        public CinemachineVirtualCamera Free;

        public Transform FollowTarget;

        public float Speed = 200f;

        private enum _camType
        {
            Look,
            Free
        };

        private _camType _activeCam = _camType.Look;

        public void OnAwake(Controller controller)
        {
            Controller = controller;
        }

        public void LateUpdate()
        {
            FollowTarget.transform.position = Controller.Character.transform.position;

            if (Controller.IsFalling)
            {
                Look.gameObject.SetActive(false);
                _activeCam = _camType.Free;
            } else
            {
                Look.gameObject.SetActive(true);
                _activeCam = _camType.Look;
            }

            if (Controller.Input.LookComposite == Vector2.zero) return;

            float x = -Controller.Input.LookComposite.y * Speed * Time.deltaTime;
            float y = Controller.Input.LookComposite.x * Speed * Time.deltaTime;
            Free.transform.Rotate(x, y, 0f);
        }
    }
}
