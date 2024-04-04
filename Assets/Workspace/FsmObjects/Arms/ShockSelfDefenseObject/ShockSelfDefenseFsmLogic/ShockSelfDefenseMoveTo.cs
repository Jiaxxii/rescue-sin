using System;
using DG.Tweening;
using UnityEngine;
using Workspace.FiniteStateMachine;
using Workspace.FsmObjects.Arms.KettleObject;

namespace Workspace.FsmObjects.Arms.ShockSelfDefenseObject.ShockSelfDefenseFsmLogic
{
    public class ShockSelfDefenseMoveTo : BaseState<ShockSelfDefenseState, IShockSelfDefense, ShockSelfDefenseMoveTo.MoveToProperty>
    {
        [Serializable]
        public class MoveToProperty
        {
            [SerializeField] private float duration;
            [Range(0.0001F, 1)] [SerializeField] private float endToleranceDistance = 0.01F;
            [SerializeField] private Ease moveEase;


            public float Duration => duration;
            public float EndToleranceDistance => endToleranceDistance;
            public Ease MoveEase => moveEase;
        }


        public override ShockSelfDefenseState State => ShockSelfDefenseState.MoveTo;


        private float _initialDistance; // 初始时水壶与玩家之间的距离  

        private bool _isComplete;


        public override void OnEnter()
        {
            // 计算初始距离  
            _initialDistance = Resources.DistanceVector2(Resources.Target.GetTag()).x;

            // 开始移动补间，使用初始距离计算出的持续时间  
            DoMove(_initialDistance);
        }

        public override void OnUnityUpdate()
        {
            // 如果补间不存在或已完成，并且玩家移动了，则重新计算并启动新的补间  
            if (_isComplete
                && Resources.DistanceVector2(Resources.Target.GetTag()).x > PrivateRes.EndToleranceDistance)
            {
                CheckDistanceAndMoveTo();
            }
        }

        private void CheckDistanceAndMoveTo()
        {
            _isComplete = true;
            // 计算当前水壶与玩家的距离  
            var currentDistance = Resources.DistanceVector2(Resources.Target.GetTag()).x;

            // 如果玩家在容忍范围内，则改变状态为空闲；否则，继续向玩家移动  
            if (currentDistance <= PrivateRes.EndToleranceDistance)
            {
                // if (Resources.Target.CompareTag("Enemy"))
                // {
                //     Debug.Log("<color=red>命中</color>");
                // }
                // else
                Resources.ChangeState(ShockSelfDefenseState.Idle);

                return;
            }

            // 根据当前距离计算新的持续时间并启动新的补间  
            DoMove(currentDistance);
        }

        private void DoMove(float distance)
        {
            _isComplete = false;
            // 根据当前距离计算新的持续时间
            Resources.Transform.DOMove(Resources.GetTargetOffsetPosition(Resources.Target.GetTag()), GetDuration(distance))
                .SetEase(PrivateRes.MoveEase)
                .OnComplete(CheckDistanceAndMoveTo); // 补间完成时再次检查距离  
        }

        private float GetDuration(float distance)
        {
            if (distance == 0) return 0;
            // 根据距离计算持续时间，这里可以根据需要调整公式来得到合适的速度感  
            // 例如，如果你想要距离越远移动越快，你可以使用反比关系；如果你想要恒定的速度，就直接返回 PrivateRes.Duration  
            // 下面的公式假设距离与持续时间成正比，但你可以根据需求进行修改 
            return PrivateRes.Duration * (distance / _initialDistance);
        }

        public override void OnExit()
        {
            _isComplete = false;
        }

        public ShockSelfDefenseMoveTo(IShockSelfDefense resources, MoveToProperty privateRes) : base(resources, privateRes)
        {
        }
    }
}