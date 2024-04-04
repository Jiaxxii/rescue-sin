using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Workspace.FiniteStateMachine;
using Workspace.FiniteStateMachine.ExpandInterface;
using Workspace.FsmObjects.Arms.KettleObject;
using Workspace.FsmObjects.Arms.KettleObject.KettleFsmLogic;
using Workspace.FsmObjects.Arms.ShockSelfDefenseObject.ShockSelfDefenseFsmLogic;

namespace Workspace.FsmObjects.Arms.ShockSelfDefenseObject
{
    public interface IShockSelfDefense : IPlayerPosition, IIdle
    {
        Transform Transform { get; }
        Transform FloatPoint { get; }


        IKettleProperty KettleProperty { get; }


        TargetCollider Target { get; set; }


        UnityEngine.Camera MainCamera { get; }

        float GetPlayerFloatDistance();

        //  float GetTargetDistance([CanBeNull] string tagName = null);
        Vector2 DistanceVector2([CanBeNull] string tagName = null);


        Vector3 GetTargetOffsetPosition([CanBeNull] string tagName);

        Vector2 FindOffset([CanBeNull] string tagName);
    }

    public interface IIdle
    {
        void SetTargetAsPlayer();
        void ChangeState(ShockSelfDefenseState state);
    }

    public class ShockSelfDefense : FsmBehaviour<ShockSelfDefenseState, IShockSelfDefense>, IShockSelfDefense
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
        public IKettleProperty KettleProperty { get; private set; }

        public UnityEngine.Camera MainCamera => mainCamera;

        [SerializeField] private UnityEngine.Camera mainCamera;

        [Tooltip("水壶的移动目标点")] [SerializeField] private Transform floatPoint;


        [Header("悬浮偏移")] [SerializeField] private List<OffsetProperty> offsetProperties;


        [Space] [SerializeField] private ShockSelfDefenseIdle.IdleProperty idleProperty;
        [SerializeField] private ShockSelfDefenseMoveTo.MoveToProperty moveToProperty;
        [SerializeField] private ShockSelfDefenseAttack.AttackProperty attackProperty;

        public TargetCollider Target { get; set; }


        protected override void Awake()
        {
            base.Awake();
            StateMachine = new StateMachine<ShockSelfDefenseState, IShockSelfDefense>(this);

            StateMachine.Add(new ShockSelfDefenseIdle(this, idleProperty));
            StateMachine.Add(new ShockSelfDefenseMoveTo(this, moveToProperty));
            StateMachine.Add(new ShockSelfDefenseAttack(this, attackProperty));

            KettleProperty = FindObjectOfType<Kettle>();
        }


        private void Start()
        {
            Target = new TargetCollider(FloatPoint);

            StateMachine.ChangeState(ShockSelfDefenseState.Idle);
        }

        public float GetPlayerFloatDistance() => Vector3.Distance(FloatPoint.position, CurrentPosition);


        public override Vector2 DistanceVector2()
        {
            return DistanceVector2(Target.Tag);
        }

        public Vector2 DistanceVector2(string tagName)
        {
            var target = GetTargetOffsetPosition(tagName);
            return new Vector2(Mathf.Abs(CurrentPosition.x - target.x), Mathf.Abs(CurrentPosition.y - target.y));
        }


        public Vector3 GetTargetOffsetPosition(string tagName)
        {
            var offset = FindOffset(tagName);
            var targetTransform = Target.GetTransform().position;
            return new Vector3(targetTransform.x + offset.x, targetTransform.y + offset.y);
        }

        public Vector2 FindOffset(string tagName)
        {
            if (string.IsNullOrEmpty(tagName) || Player.CompareTag(tagName)) return playerOffset;

            var result = offsetProperties.Find(item => item.Name == tagName);

            return result?.Offset ?? throw new NullReferenceException($"未定义的名称\"{tagName}\"!");
        }


        public void SetTargetAsPlayer() => Target.UpDate(FloatPoint, Player.tag);
    }
}