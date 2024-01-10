using System;
using UnityEngine;

namespace Daze.Characters
{
    [Serializable]
    public class Vfx
    {
        [NonSerialized] public Player Player;
        [NonSerialized] public AnimationEvent AnimationEvent;

        public ParticleSystem WalkL;
        public ParticleSystem WalkR;
        public ParticleSystem RunL;
        public ParticleSystem RunR;
        public ParticleSystem RunStopR;
        public ParticleSystem BlowSoft;
        public ParticleSystem BlowHard01;
        public ParticleSystem BlowHard02;

        public void OnAwake(Player player)
        {
            Player = player;
            AnimationEvent = Player.GetComponentInChildren<AnimationEvent>();

            AnimationEvent.OnWalkL += WalkL.Play;
            AnimationEvent.OnWalkR += WalkR.Play;
            AnimationEvent.OnRunL += RunL.Play;
            AnimationEvent.OnRunR += RunR.Play;
            AnimationEvent.OnRunStopR += RunStopR.Play;
            AnimationEvent.OnRunJumpRise += BlowSoft.Play;
            AnimationEvent.OnJumpLand += BlowSoft.Play;
            AnimationEvent.OnDiveLand += DiveLand;
        }

        private void DiveLand()
        {
            BlowHard01.Play();
            BlowHard02.Play();
        }
    }
}
