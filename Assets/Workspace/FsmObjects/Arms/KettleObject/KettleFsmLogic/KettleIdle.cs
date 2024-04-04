using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Workspace.EditorAttribute;
using Workspace.FiniteStateMachine;

namespace Workspace.FsmObjects.Arms.KettleObject.KettleFsmLogic
{
    public class KettleIdle : BaseState<KettleState, IKettle, KettleIdle.IdleProperty>
    {
        public KettleIdle(IKettle resources, IdleProperty privateRes) : base(resources, privateRes)
        {
        }

        [Serializable]
        public class IdleProperty
        {
            [SerializeField] private List<Config> configs;

            public Config GetConfig(string configName)
            {
                if (string.IsNullOrEmpty(configName))
                {
                    return configs[0] ?? throw new AggregateException($"\"{nameof(configName)}\"不能为空!并且\"{nameof(configs)}\"列表不能为空!");
                }

                return configs.Find(config => config.ConfigName == configName) ?? throw new AggregateException($"配置中没有\"{configName}\"项!");
            }

            [Serializable]
            public class Config
            {
                [SerializeField] private string configName;

                [SerializeField] [RewriteName("脱离距离", "如果水壶与目标之间的距离超过此值将切换状态")]
                private Vector2 idleOutRange;

                [SerializeField] [RewriteName("目标高度", "水壶将在定义的时间内经过此高度")]
                private float targetY;

                [SerializeField] [RewriteName("往返持续时间", "水壶一次往返的时间")]
                private float oneLoopDuration;

                [SerializeField] [RewriteName("运动模式")] private Ease moveEase;

                [SerializeField] [RewriteName("运动模式")] private AnimationCurve moveCurve;


                public string ConfigName => configName;
                public Vector2 IdleOutRange => idleOutRange;

                public float TargetY => targetY;

                public float OneLoopDuration => oneLoopDuration;

                public Ease MoveEase => moveEase;

                public AnimationCurve MoveCurve => moveCurve;
            }
        }


        public override KettleState State => KettleState.Idle;


        private IdleProperty.Config _currentConfig;

        public override void OnEnter()
        {
            // 更改配置
            _currentConfig = PrivateRes.GetConfig(Resources.Target.GetTag());

            // 设置悬浮动画
            var targetY = Resources.CurrentPosition.y + _currentConfig.TargetY;
            Resources.Transform.DOMoveY(targetY, _currentConfig.OneLoopDuration * .5F)
                .SetEase(_currentConfig.MoveCurve, _currentConfig.MoveEase)
                .SetLoops(-1, LoopType.Yoyo);
        }


        public override void OnUnityUpdate()
        {
            // 默认状态下于玩家分开一定的距离时推出状态
            var distance = Resources.GetTargetDistanceXY(Resources.Target.GetTag());
            if (distance.x >= _currentConfig.IdleOutRange.x || distance.y >= _currentConfig.IdleOutRange.y)
            {
                Resources.ChangeState(KettleState.MoveTo);
            }

            // if (!Input.GetMouseButtonDown(0)) return;
            //
            // // 表示水壶已经在敌人身上
            if (Resources.Target.CompareTag("Enemy"))
            {
                Resources.ChangeState(KettleState.Attack);
                return;
            }
            //
            // var worldPoint = Resources.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            //
            // var hit = Physics2D.Raycast(worldPoint, Vector2.zero, 100F);
            //
            // if (hit.collider is null || !hit.collider.CompareTag("Enemy")) return;
            //
            // // 更新悬浮目标
            // Resources.Target.UpDate(hit.collider);
            // Resources.ChangeState(KettleState.MoveTo);
        }


        public override void OnExit()
        {
            Resources.Transform.DOKill();
            _currentConfig = null;
        }
    }
}