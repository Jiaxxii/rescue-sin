using System;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using Workspace.AudioManagerSystem;
using Workspace.FiniteStateMachine;
using Object = UnityEngine.Object;


namespace Workspace.FsmObjects.Enemy.YuKFsmLogic
{
    public class YuKHurt : BaseState<YuKState, IYuK, YuKHurt.HurtProperty>
    {
        [Serializable]
        public class HurtProperty
        {
            [SerializeField] private List<Config> configs;

            public Config FindConfig([CanBeNull] string configName) => configs.FindConfig(v => v.ConfigName == configName);


            [Serializable]
            public class Config
            {
                [SerializeField] private string configName;

                public string ConfigName => configName;

                [SerializeField] private float duration;
                [SerializeField] private Vector3 strength;
                [SerializeField] private int vibrato = 10;
                [SerializeField] private bool snapping;
                [SerializeField] private bool fadeOut = true;
                [SerializeField] private ShakeRandomnessMode shakeRandomnessMode;

                [SerializeField] private float exitHurtRange;

                [SerializeField] private Vector2 galvanicOffset;

                public float Duration => duration;

                public Vector2 GalvanicOffset => galvanicOffset;

                public Vector3 Strength => strength;

                public int Vibrato => vibrato;

                public bool Snapping => snapping;

                public bool FadeOut => fadeOut;

                public ShakeRandomnessMode ShakeRandomnessMode => shakeRandomnessMode;

                public float ExitHurtRange => exitHurtRange;
            }
        }

        public YuKHurt(IYuK resources, HurtProperty privateRes) : base(resources, privateRes)
        {
            _galvanic =
                Object.Instantiate(UnityEngine.Resources.Load<ParticleSystem>("Electric Particle")).GetComponent<ParticleSystem>();

            var groupPair = AudioManager.Instance.Create("Sound");
            AudioManager.Instance.TrtAddAudioSource(() => groupPair, nameof(YuK), new Sound());

            _audio = AudioManager.Instance.GetSource(groupPair, nameof(YuK));
        }

        public override YuKState State => YuKState.Hurt;

        private HurtProperty.Config _config;

        private readonly AudioSource _audio;

        private ParticleSystem _galvanic;


        public override void OnEnter()
        {
            _config = PrivateRes.FindConfig(Resources.Target.GetTag());

            Resources.Transform
                .DOShakePosition(_config.Duration, _config.Strength, _config.Vibrato, 90F, _config.Snapping, _config.FadeOut, _config.ShakeRandomnessMode)
                .OnComplete(Check);

            Resources.YukAnimator.speed = 0;
            Hurt();
        }

        public override void OnUnityUpdate()
        {
        }

        public override void OnExit()
        {
            Resources.YukAnimator.speed = 1;
            _audio.Stop();

            _galvanic.Stop();
        }


        private void Hurt()
        {
            if (_audio.isPlaying) return;

            _galvanic.transform.position = Resources.CurrentPosition + _config.GalvanicOffset.V3();
            _galvanic.Play();

            // 受伤时播放尖叫
            _audio.clip = UnityEngine.Resources.Load<AudioClip>("yuk1679");
            _audio.loop = true;
            _audio.Play();
        }

        private void Check()
        {
            var current = Resources.CurrentPosition + Resources.GetOffset(Resources.Target.GetTag()).V3();
            var distance = Resources.DistanceVector2(Resources.Target.GetTransform().position, current);

            if (Resources.Target.CompareTag("ShockSelfDefense") && _config.ExitHurtRange >= distance.x)
            {
                Resources.ChangeState(YuKState.Hurt);
            }
            else Resources.ChangeState(YuKState.Idle);
        }
    }
}