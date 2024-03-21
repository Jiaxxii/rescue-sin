using UnityEngine;
using Workspace.FiniteStateMachine;

namespace Workspace.Enemy.YuKFsmLogic
{
    public class YuKIdle : BaseState<YuKState, IYuK, YuKIdle.IdleProperty>
    {
        [System.Serializable]
        public class IdleProperty
        {
            [Header("检测角色进入的范围")] [SerializeField] [Tooltip("应该设置为角色面向的坐标")]
            private Transform frontPoint;

            [SerializeField] [Tooltip("应该设置为角色背向的坐标")]
            private Transform rearPoint;

            [Header("Idle动画")] [SerializeField] private AnimationClip[] idleAnimationClips;

            [SerializeField] [Tooltip("设置切换下一个动画的时间(time = [x~y])")]
            private Vector2 switchIdleSecondRange;

            public Transform FrontPoint => frontPoint;

            public Transform RearPoint => rearPoint;

            public Vector2 SwitchIdleSecondRange => switchIdleSecondRange;

            private int[] _animatorNameHashCode;

            private int[] AnimatorNameHashCode
            {
                get
                {
                    if (_animatorNameHashCode != null)
                    {
                        return _animatorNameHashCode;
                    }

                    _animatorNameHashCode = new int[idleAnimationClips.Length];
                    for (var i = 0; i < idleAnimationClips.Length; i++)
                    {
                        _animatorNameHashCode[i] = Animator.StringToHash(idleAnimationClips[i].name);
                    }

                    idleAnimationClips = null;
                    return _animatorNameHashCode;
                }
            }

            public int GetRandomAnimation()
            {
                return AnimatorNameHashCode[Random.Range(0, _animatorNameHashCode.Length)];
            }
        }

        public YuKIdle(IYuK resources, IdleProperty privateRes) : base(resources, privateRes)
        {
        }

        public override YuKState State => YuKState.Idle;

        private float _idleSwitchTimer;

        private float _switchIdleSecondRange;

        public override void OnEnter()
        {
            _switchIdleSecondRange = Random.Range(PrivateRes.SwitchIdleSecondRange.x, PrivateRes.SwitchIdleSecondRange.y);
            Resources.YukAnimator.Play("idle");
        }

        public override void OnUnityUpdate()
        {
            // 切换到其他的 idle 动画
            if (_idleSwitchTimer >= _switchIdleSecondRange)
            {
                // 重新计算下一个切换动画的时间点
                _switchIdleSecondRange = Random.Range(PrivateRes.SwitchIdleSecondRange.x, PrivateRes.SwitchIdleSecondRange.y);
                Resources.YukAnimator.Play(PrivateRes.GetRandomAnimation());
                // 初始化idle计时器 为 负的当前(切换)后的动画总的时长(second)
                // 这里防止一个idle动画未播放结束就切换而导致的不连贯
                _idleSwitchTimer = Resources.YukAnimator.GetCurrentAnimatorStateInfo(0).length;
            }
            else _idleSwitchTimer += Time.deltaTime;

            
            // 判断玩家是否在范围内
            if (Resources.InRangeAs(
                    new Range(PrivateRes.FrontPoint.position.x, PrivateRes.RearPoint.position.x), null))
            {
                Resources.ChangedState(YuKState.PlayerInRange);
            }
        }

        public override void OnExit()
        {
            _idleSwitchTimer = 0F;
        }
    }
}