using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;

namespace Daze.P.Characters
{
    public class Character : MonoBehaviour, ICharacterController
    {
        [NonSerialized] public Player Player;

        public KinematicCharacterMotor Motor;

        [Header("Ground Movement")]
        public float MaxGroundMoveSpeed = 10f;
        public float GroundMovementSharpness = 15f;

        [Header("Gravity")]
        public Vector3 Gravity = new(0, -30f, 0);

        private Vector3 _moveDirection = Vector3.zero;

        public void OnAwake(Player player)
        {
            Player = player;
            Motor.CharacterController = this;
        }

        public void OnLateUpdate()
        {
            Vector3 right = Player.Camera.right;
            Vector3 forward = Vector3.Cross(right, -Gravity).normalized;

            Vector3 forwardRelativeVInput = forward * Player.Action.MoveComposite.y;
            Vector3 rightRelativeHInput = right * Player.Action.MoveComposite.x;

            _moveDirection = forwardRelativeVInput + rightRelativeHInput;
        }

        /// <summary>
        /// This is called when the motor wants to know what its velocity
        /// should be right now. This is the ONLY place where you can
        /// set the character's velocity.
        /// </summary>
        public void UpdateVelocity(ref Vector3 velocity, float deltaTime)
        {
            if (Motor.GroundingStatus.IsStableOnGround)
            {
                // Reorient source velocity on current ground slope. This is
                // because we don't want our smoothing to cause any velocity
                // losses in slope changes.
                velocity = Motor.GetDirectionTangentToSurface(velocity, Motor.GroundingStatus.GroundNormal) * velocity.magnitude;

                // Calculate target velocity
                Vector3 inputRight = Vector3.Cross(_moveDirection, Motor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(Motor.GroundingStatus.GroundNormal, inputRight).normalized * _moveDirection.magnitude;
                Vector3 targetMovementVelocity = reorientedInput * MaxGroundMoveSpeed;

                // // Smooth movement Velocity
                velocity = Vector3.Lerp(velocity, targetMovementVelocity, 1 - Mathf.Exp(-GroundMovementSharpness * deltaTime));
            }
            else
            {
                // Gravity
                velocity += Gravity * deltaTime;

                // Drag
                // currentVelocity *= (1f / (1f + (Drag * deltaTime)));
            }
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        { }

        public void BeforeCharacterUpdate(float deltaTime)
        { }

        public void AfterCharacterUpdate(float deltaTime)
        { }

        public void PostGroundingUpdate(float deltaTime)
        { }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        { }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        { }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        { }

        /// <summary>
        /// This is called after when the motor wants to know if the collider
        /// can be collided with (or if we just go through it).
        /// </summary>
        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        { }
    }
}
