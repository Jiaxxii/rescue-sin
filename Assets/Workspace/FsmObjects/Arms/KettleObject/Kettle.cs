using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Workspace.FiniteStateMachine;
using Workspace.FiniteStateMachine.ExpandInterface;
using Workspace.FsmObjects.Arms.KettleObject.KettleFsmLogic;

namespace Workspace.FsmObjects.Arms.KettleObject
{
    public interface IKettleProperty
    {
        void ChangeState(KettleState state, GameObject gameObject, [CanBeNull] string tag = null);
    }

    public interface IKettle : IPlayerPosition
    {
        Transform Transform { get; }
        Transform FloatPoint { get; }

        TargetCollider Target { get; }

        UnityEngine.Camera MainCamera { get; }
        void ChangeState(KettleState state);

        float GetPlayerFloatDistance();

        //  float GetTargetDistance([CanBeNull] string tagName = null);
        Vector2 GetTargetDistanceXY([CanBeNull] string tagName = null);


        Vector3 GetTargetOffsetPosition([CanBeNull] string tagName = null);

        Vector2 FindOffset([CanBeNull] string tagName);


        void SetTargetAsPlayer();
    }

    public class Kettle : FsmBehaviour<KettleState, IKettle>, IKettle, IKettleProperty
    {
        [Serializable]
        public class OffsetProperty
        {
            [SerializeField] private string name;
            [SerializeField] private Vector2 offset;

            public string Name => name;

            public Vector2 Offset => offset;
        }

        public Transform Transform => transform;

        public Transform FloatPoint => floatPoint;
        public TargetCollider Target { get; private set; }
        public UnityEngine.Camera MainCamera => mainCamera;

        [SerializeField] private UnityEngine.Camera mainCamera;

        [Tooltip("水壶的移动目标点")] [SerializeField] private Transform floatPoint;


        [Header("默认(浮空)")] [SerializeField] private KettleIdle.IdleProperty idleProperty;

        [Header("悬浮偏移")] [SerializeField] private List<OffsetProperty> offsetProperties;

        [Header("移动向玩家")] [SerializeField] private KettleMoveTo.MoveToProperty moveToProperty;

        [Header("攻击")] [SerializeField] private KettleAttack.AttackProperty attackProperty;
        // [SerializeField] private KettleMoveToEnemy.MoveToEnemyProperty moveToEnemyProperty;


        protected override void Awake()
        {
            base.Awake();
            StateMachine = new StateMachine<KettleState, IKettle>(this);

            StateMachine.Add(new KettleIdle(this, idleProperty));
            StateMachine.Add(new KettleMoveTo(this, moveToProperty));
            StateMachine.Add(new KettleAttack(this, attackProperty));

            Target = new TargetCollider(FloatPoint, Player.tag);
            // StateMachine.Add(new KettleMoveToEnemy(this, moveToEnemyProperty));
        }


        private void Start()
        {
            // TagName = Player.gameObject.tag;
            // Target = FloatPoint;

            StateMachine.ChangeState(KettleState.Idle);
        }

        public float GetPlayerFloatDistance() => Vector3.Distance(FloatPoint.position, CurrentPosition);

        public float GetTargetDistance(string tagName)
        {
            if (string.IsNullOrEmpty(tagName) || Player.CompareTag(tagName)) return GetPlayerFloatDistance();

            var target = GetTargetOffsetPosition(tagName);

            return Vector3.Distance(CurrentPosition, target);
        }

        public override Vector2 DistanceVector2()
        {
            var target = GetTargetOffsetPosition(Target.GetTag());
            return new Vector2(Mathf.Abs(CurrentPosition.x - target.x), Mathf.Abs(CurrentPosition.y - target.y));
        }

        public Vector2 GetTargetDistanceXY(string tagName = null)
        {
            var target = GetTargetOffsetPosition(tagName);
            return new Vector2(Mathf.Abs(CurrentPosition.x - target.x), Mathf.Abs(CurrentPosition.y - target.y));
        }


        public Vector3 GetTargetOffsetPosition(string tagName)
        {
            // if (Player.CompareTag(tagName))
            // {
            //     return Target.position;
            // }

            var offset = FindOffset(tagName);
            var targetPosition = Target.GetTransform().position;
            return new Vector3(targetPosition.x + offset.x, targetPosition.y + offset.y);
        }

        public Vector2 FindOffset(string tagName)
        {
            if (string.IsNullOrEmpty(tagName) || Player.CompareTag(tagName))
            {
                return playerOffset;
            }

            var result = offsetProperties.Find(item => item.Name == tagName);

            return result?.Offset ?? throw new NullReferenceException($"未定义的名称\"{tagName}\"!");
        }

        public void SetTargetAsPlayer()
        {
            // TagName = Player.gameObject.tag;
            // Target = FloatPoint;
            Target.UpDate(FloatPoint, Player.tag);
        }

        void IKettleProperty.ChangeState(KettleState state, GameObject go, string goTag)
        {
            Target.UpDate(go, goTag);
            ChangeState(state);
        }
    }
}