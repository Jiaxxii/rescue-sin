using Unity.VisualScripting;
using UnityEngine;
using Workspace.FiniteStateMachine;

namespace Workspace.Enemy.YuKFsmLogic
{
    public class YuKIdle : BaseState<YuKState, IYuK, YuKIdle.IdleProperty>
    {
        [System.Serializable]
        public class IdleProperty
        {
            [SerializeField] private Animator animator;
            [SerializeField] private Vector2 switchIdleSecondRange;
            [SerializeField] private Vector2 exitIdleSecond;
            public Vector2 SwitchIdleSecondRange => switchIdleSecondRange;

            public Vector2 ExitIdleSecond => exitIdleSecond;

            public Animator Animator => animator;
        }

        public YuKIdle(IYuK resources, IdleProperty privateRes) : base(resources, privateRes)
        {
        }

        public override YuKState State => YuKState.Idle;

        private float _timer;
        private float _idleSwitchTimer;

        private float _exitIdleSecond;
        private float _switchIdleSecondRange;

        public override void OnEnter()
        {
            _timer = 0F;
            _switchIdleSecondRange = Random.Range(PrivateRes.SwitchIdleSecondRange.x, PrivateRes.SwitchIdleSecondRange.y);
            _exitIdleSecond = Random.Range(PrivateRes.ExitIdleSecond.x, PrivateRes.ExitIdleSecond.y);
        }

        public override void OnUnityUpdate()
        {
            // 切换到其他的 idle 动画
            if (_idleSwitchTimer >= _switchIdleSecondRange)
            {
                // 重新计算下一个切换动画的时间点
                _switchIdleSecondRange = Random.Range(PrivateRes.SwitchIdleSecondRange.x, PrivateRes.SwitchIdleSecondRange.y);
                PrivateRes.Animator.Play(Random.Range(0, 2) == 0 ? "idle" : "idle_2");
                // 初始化idle计时器 为 负的当前(切换)后的动画总的时长(second)
                // 这里防止一个idle动画未播放结束就切换而导致的不连贯
                _idleSwitchTimer = PrivateRes.Animator.GetCurrentAnimatorStateInfo(0).length;
            }
            else _idleSwitchTimer += Time.deltaTime;
            
            // 判断玩家是否在范围内
         //   if()

            //
            // if (_timer >= _exitIdleSecond && !IsPlaying())
            // {
            //     Debug.Log("退出动画");
            // }
            // else _timer += Time.deltaTime;
        }

        public override void OnExit()
        {
            throw new System.NotImplementedException();
        }
        
        
        private bool IsPlaying()
        {
            var animatorStateInfo = PrivateRes.Animator.GetCurrentAnimatorStateInfo(0);
            return animatorStateInfo.normalizedTime is > 0 and < 1;
        }
    }
}