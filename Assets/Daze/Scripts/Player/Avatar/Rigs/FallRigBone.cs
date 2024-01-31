using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Daze.Player.Avatar.Rigs
{
    public class FallRigBone : MonoBehaviour
    {
        public OverrideTransform Rig;
        [NonSerialized] public Transform Target;

        [Header("Base Settings")]

        public float Weight;
        public Vector3 Offset;
        public float RotationSpeed;

        [Header("Rotation Settings")]

        public List<FallRigBoneSettings> X;
        public List<FallRigBoneSettings> Y;
        public List<FallRigBoneSettings> Z;

        private bool _useManualWeight = false;
        private float _manualWeight = 0f;

        protected void OnValidate()
        {
            UnityEditor.EditorApplication.delayCall += DoValidate;
        }

        protected void DoValidate()
        {
            UnityEditor.EditorApplication.delayCall -= DoValidate;
            if (this != null)
                SetWeight();
        }

        public void Awake()
        {
            Target = Rig.data.sourceObject.transform;
            SetWeight();
        }

        /// <summary>
        /// Handle the bone rotation. This method is called from the
        /// `FallRig` component, and the velocity is passed from there.
        /// This method runs inside `FixedUpdate` hook.
        /// </summary>
        public void Control(Vector3 velocity)
        {
            if (Rig.weight > 0)
                AddRotation(velocity);
        }

        protected void AddRotation(Vector3 velocity)
        {
            Quaternion rotation = Quaternion.Euler(
                Offset.x + GetRotationValueFor(velocity, X),
                Offset.y + GetRotationValueFor(velocity, Y),
                Offset.z + GetRotationValueFor(velocity, Z)
            );

            Target.localRotation = Quaternion.Slerp(
                Target.localRotation,
                rotation,
                RotationSpeed * Time.fixedDeltaTime
            );
        }

        protected float GetRotationValueFor(
            Vector3 velocity,
            List<FallRigBoneSettings> settings
        )
        {
            float value = 0f;

            foreach (FallRigBoneSettings s in settings)
            {
                value = GetRotationValue(
                    GetVelocityAxisFromSettings(velocity, s),
                    s.Multiplier,
                    s.CounterThreshold,
                    s.BendLimit
                );
            }

            return value;
        }

        /// <summary>
        /// Get the value based on the current velocity and the settings.
        /// </summary>
        protected float GetRotationValue(
            float velocity,
            Vector2 multiplier,
            Vector2 counterThreshold,
            Vector2 limit
        )
        {
            // If the velocity is 0, ust return 0.
            if (velocity == 0)
                return 0;

            // Check if the velocity is positive or negative since we have
            // settings that can affect differently based on the direction.
            bool isVelocityPositive = velocity > 0;
            float normalizedVelocity = isVelocityPositive ? 1 : -1;

            float m = isVelocityPositive ? multiplier.x : multiplier.y;
            float c = isVelocityPositive ? counterThreshold.x : counterThreshold.y;
            float l = isVelocityPositive ? limit.x : limit.y;

            // At first, we will multiply the velocity by the multiplier to get
            // exaggerated value because we want bones to rotate bigger when
            // the velocity is smaller so that bones don't stop rotating on
            // small velocity.
            //
            // We use absplute value here so that we can treat postive and
            // negative velocity in a same way.
            float v = Mathf.Abs(velocity) * m;

            // If the multiplied value is bigger than the counter threshold
            // `c`, supress the value as it gets close to the limit `l`. This
            // will try to increase value as velocity gets bigger but ensures
            // it will never reach the limit.
            //
            // Note (Kia): `15` is the maximum velocity. I'm using magic number
            // here because at the moment, I'm not sure how to handle max speed
            // change, which could happen when we add dive feature where the
            // character can increase the max speed while falling.
            if (v > c)
                v = Mathf.Lerp(c, l, Mathf.InverseLerp(c, 15 * m, v));

            // Finally, restore the postive/negative velocity direction.
            return v * normalizedVelocity;
        }

        protected float GetVelocityAxisFromSettings(
            Vector3 velocity,
            FallRigBoneSettings settings
        )
        {
            if (settings.VelocityAxis.x != 0)
                return velocity.x * settings.VelocityAxis.x;
            else if (settings.VelocityAxis.y != 0)
                return velocity.y * settings.VelocityAxis.y;
            else if (settings.VelocityAxis.z != 0)
                return velocity.z * settings.VelocityAxis.z;
            else
                return 0;
        }

        protected void SetWeight()
        {
            Rig.weight = _useManualWeight ? _manualWeight : Weight;
        }

        public void UseManualWeight(bool use, float weight)
        {
            _useManualWeight = use;
            _manualWeight = weight;
            SetWeight();
        }
    }

    [Serializable]
    public class FallRigBoneSettings
    {
        public Vector3 VelocityAxis;
        public Vector2 Multiplier;
        public Vector2 CounterThreshold;
        public Vector2 BendLimit;
    }
}
