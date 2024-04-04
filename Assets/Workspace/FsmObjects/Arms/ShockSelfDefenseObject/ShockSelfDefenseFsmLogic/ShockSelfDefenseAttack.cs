using System;
using DG.Tweening;
using UnityEngine;
using Workspace.FiniteStateMachine;
using Workspace.FsmObjects.AnimationAction;
using Workspace.FsmObjects.Arms.KettleObject;

namespace Workspace.FsmObjects.Arms.ShockSelfDefenseObject.ShockSelfDefenseFsmLogic
{
    public class ShockSelfDefenseAttack : BaseState<ShockSelfDefenseState, IShockSelfDefense, ShockSelfDefenseAttack.AttackProperty>
    {
        [System.Serializable]
        public class AttackProperty
        {
            [SerializeField] private float speed;
            [SerializeField] private float rotationSpeed;
            [SerializeField] private float hitRange;

            public float HitRange => hitRange;

            public float Speed => speed;

            [Range(0, 360)] [SerializeField] private float angleOffset;
            [SerializeField] private float lockDuration;

            public float LockDuration => lockDuration;

            public float RotationSpeed => rotationSpeed;
            public float AngleOffset => angleOffset;
        }


        public ShockSelfDefenseAttack(IShockSelfDefense resources, AttackProperty privateRes) : base(resources, privateRes)
        {
        }

        public override ShockSelfDefenseState State => ShockSelfDefenseState.Attack;

        private float _targetAngle;
        private float _currentAngle;
        private float _timer;

        private bool _isHurt;

        public override void OnEnter()
        {
            _currentAngle = 0;
            _targetAngle = 0;
            _timer = 0;
        }

        public override void OnUnityUpdate()
        {
            if (_timer < PrivateRes.LockDuration)
            {
                Lock();
                _timer += Time.deltaTime;
                return;
            }

            var targetPosition = Resources.GetTargetOffsetPosition(Resources.Target.GetTag());

            Resources.Transform.position =
                Vector3.MoveTowards(Resources.Transform.position, targetPosition, PrivateRes.Speed * Time.deltaTime);

            if (_isHurt || !(Resources.DistanceVector2(Resources.Target.GetTag()).x <= PrivateRes.HitRange)) return;

            Resources.Target.GetTransform().GetComponent<IHurt>()?.Hurt(Resources.Transform.gameObject);
            Resources.KettleProperty.ChangeState(KettleState.MoveTo, Resources.Target.GetTransform().gameObject);
            _isHurt = true;
        }

        private void Lock()
        {
            // 计算当前对象与目标之间的向量  
            var direction = Resources.Target.GetTransform().position - Resources.CurrentPosition;


            // 使用Atan2函数计算目标旋转角度（以弧度为单位）  
            var angleInRadians = Mathf.Atan2(direction.y, direction.x);

            // 将弧度转换为角度  
            _targetAngle = angleInRadians * Mathf.Rad2Deg;
            _targetAngle -= -PrivateRes.AngleOffset;

            _currentAngle = Mathf.MoveTowardsAngle(_currentAngle, _targetAngle, PrivateRes.RotationSpeed * Time.deltaTime);

            // 应用旋转到当前对象。注意在2D中我们围绕Z轴旋转。  
            Resources.Transform.rotation = Quaternion.AngleAxis(_currentAngle, Vector3.forward);
        }

        public override void OnExit()
        {
            _isHurt = false;
            Resources.Transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
        }
    }
}