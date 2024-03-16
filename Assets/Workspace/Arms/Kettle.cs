using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Workspace.FiniteStateMachine;
using Workspace.Player;

namespace Workspace.Arms
{
    public interface IKettle
    {
        Transform Transform { get; }
        Transform FloatPoint { get; }
        Vector3 PlayerOffsetPosition { get; }
        float GetPlayerDistance(Vector3 position);
        float GetPlayerDistance();

        float GetPlayerFloatDistance(Vector3 position);

        float GetPlayerFloatDistance();

        void ChangedState(KettleState kettleState);
    }

    public class Kettle : FsmBehaviour<KettleState, IKettle>, IKettle
    {
        public Transform Transform => transform;

        public Transform FloatPoint => floatPoint;

        public Vector3 PlayerOffsetPosition => new(player.position.x + playerOffset.x, player.position.y + playerOffset.y, player.position.z);


        [Header("玩家")] [SerializeField] private Transform player;
        [Tooltip("水壶的移动目标点")] [SerializeField] private Transform floatPoint;

        [Tooltip("玩家的坐标点进行偏移")] [SerializeField]
        private Vector3 playerOffset;


        [Header("默认(浮空)")] [SerializeField] private KettleIdle.IdleProperty idleProperty;
        [Header("移动向玩家")] [SerializeField] private KettleMoveToPlayer.MoveToPlayerProperty moveToPlayerProperty;


        private void Awake()
        {
            Add(KettleState.Idle, new KettleIdle(this, idleProperty));
            Add(KettleState.MoveToPlayer, new KettleMoveToPlayer(this, moveToPlayerProperty));
        }

        private void Start()
        {
            StateMachine = new StateMachine<KettleState, IKettle>(this);
            ChangedState(KettleState.Idle);
        }


        public float GetPlayerDistance(Vector3 position)
        {
            return Vector3.Distance(position, player.position + playerOffset);
        }

        public float GetPlayerDistance()
        {
            return Vector3.Distance(transform.position, player.position + playerOffset);
        }


        public float GetPlayerFloatDistance(Vector3 position)
        {
            return Vector3.Distance(position, floatPoint.position);
        }

        public float GetPlayerFloatDistance()
        {
            return GetPlayerFloatDistance(transform.position);
        }
    }
}