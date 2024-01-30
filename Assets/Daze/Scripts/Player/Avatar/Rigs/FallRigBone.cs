using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Daze.Player.Avatar.Rigs
{
    [Serializable]
    public class FallRigBoneRotationSettings
    {
        public Vector3 VelocityAxis;
        public Vector2 Multiplier;
        public Vector2 CounterThreshold;
        public Vector2 BendLimit;
    }

    public class FallRigBone : MonoBehaviour
    {
        public OverrideTransform Rig;
        [NonSerialized] public Transform Target;

        [Header("Base Settings")]

        public float Weight;
        public Vector3 Offset;
        public float RotationSpeed;

        [Header("Rotation Settings")]

        public List<FallRigBoneRotationSettings> Z;

        [Header("Noise Settings")]

        public Vector2 NoiseSeed;
        public float NoiseScale;

        protected bool _brakeRequested = false;
        protected bool _isBraking = false;
        protected bool _isBrakeReached = false;
        protected Vector3 _currentBrake;
        protected Vector3 _brakeStart;

        public void Awake()
        {
            Target = Rig.data.sourceObject.transform;

            InitializeWeight();
            InitializeNoiseSeed();
        }

        public void Control(Vector3 velocity, Vector3 acceleration)
        {
            // Here we pass either velocity or the acceleration which have
            // higher magnitude. Ths is so that when decelerating, the bones
            // should get higher impact on its pose.

            // if (!_brakeRequested)
            // {
            //     // DoControl(velocity);
            // }
            // else
            {
                if (!_isBraking)
                {
                    Debug.Log("Magnitude when brake start:" + velocity.magnitude);
                    _isBraking = true;
                    _isBrakeReached = false;
                    _currentBrake = velocity;
                    _brakeStart = velocity * 3f;
                }

                if (!_isBrakeReached)
                {
                    _currentBrake = Vector3.Lerp(_currentBrake, _brakeStart, Time.fixedDeltaTime * 10f);
                    // DoControl(_currentBrake);

                    if (_currentBrake.magnitude > _brakeStart.magnitude * 0.99f)
                    {
                        _isBrakeReached = true;
                    }
                }
                else
                {
                    _currentBrake = Vector3.Lerp(_currentBrake, Vector3.zero, Time.fixedDeltaTime * 5f);
                    // DoControl(_currentBrake);

                    if (_currentBrake.magnitude < 1f)
                    {
                        _brakeRequested = false;
                        _isBraking = false;
                    }
                }
            }

            AddRotation(velocity);
            AddNoise(velocity);
        }

        private void AddRotation(Vector3 velocity)
        {
            Quaternion rotation = Quaternion.Euler(
                0f,
                0f,
                GetRotationValueFor(velocity, Z)
            );

            Target.localRotation = Quaternion.Slerp(
                Target.localRotation,
                rotation,
                RotationSpeed * Time.fixedDeltaTime
            );
        }

        private float GetRotationValueFor(
            Vector3 velocity,
            List<FallRigBoneRotationSettings> settings
        )
        {
            float value = 0f;

            foreach (FallRigBoneRotationSettings s in settings)
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
        /// Get the value based on the current velocity and the settings. This
        /// value will be used to rotate the bone via `Rotate` function later.
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
            {
                return 0;
            }

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
            {
                v = Mathf.Lerp(c, l, Mathf.InverseLerp(c, 45 * m, v));
            }

            // Finally, restore the postive/negative velocity direction.
            return v * normalizedVelocity;
        }

        private float GetVelocityAxisFromSettings(
            Vector3 velocity,
            FallRigBoneRotationSettings settings
        )
        {
            if (settings.VelocityAxis.x != 0)
            {
                return velocity.x * settings.VelocityAxis.x;
            }
            else if (settings.VelocityAxis.y != 0)
            {
                return velocity.y * settings.VelocityAxis.y;
            }
            else if (settings.VelocityAxis.z != 0)
            {
                return velocity.z * settings.VelocityAxis.z;
            }
            else
            {
                return 0;
            }
        }

        protected void InitializeWeight()
        {
            Rig.weight = Weight;
        }

        private void InitializeNoiseSeed()
        {
            // NoiseSeed = new Vector2(Random.Range(0f, 100f), Random.Range(0f, 100f));
        }

        private void AddNoise(Vector3 velocity)
        {
            // for (int i = 0; i < _rigs.Count; i++)
            {
                // float seed = i == 0 ? NoiseSeed.x : NoiseSeed.y;
                // float noise = CreateNoise(velocity, seed);

                // _rigs[i].weight = Mathf.Lerp(_rigs[i].weight, Weight + noise, 1f * Time.fixedDeltaTime);
            }
        }

        private float CreateNoise(Vector3 velocity, float seed)
        {
            float noise = Mathf.PerlinNoise(Time.time * NoiseScale, seed) - 0.5f;

            return Mathf.Lerp(0f, noise, Mathf.InverseLerp(5f, 15f, velocity.magnitude));
        }

        public void Break()
        {
            Debug.Log("Break Requested");
            _brakeRequested = true;
        }

        public void ShowMesh(bool show)
        {
            Target.GetComponent<MeshRenderer>().enabled = show;
        }
    }
}
