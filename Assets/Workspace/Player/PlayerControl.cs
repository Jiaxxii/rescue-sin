using System;
using DG.Tweening;
using UnityEngine;

namespace Workspace.Player
{
    public class PlayerControl : MonoBehaviour, PlayerControl.IPlayerMove
    {
        [SerializeField] private Animator animator;
        public float Horizontal { get; private set; }

        public Transform Transform => transform;

        [SerializeField] private float speed = 10;
        [SerializeField] private float rotateTowardsSpeed = 15;


        private readonly int _animatorHashSpeed = Animator.StringToHash("speed");

        private void Awake() => Application.targetFrameRate = 90;

        private void Update()
        {
            Horizontal = Input.GetAxis("Horizontal");

            animator.SetFloat(_animatorHashSpeed, Mathf.Abs(Horizontal));

            RotateTowardsTarget2D();
        }

        private void FixedUpdate()
        {
            if (Horizontal != 0)
            {
                transform.position += speed * Time.fixedDeltaTime * (Horizontal > 0 ? Vector3.right : Vector3.left);
            }
        }

        private void RotateTowardsTarget2D()
        {
            if (Horizontal == 0) return;

            var x = Horizontal > 0 ? 1 : -1;

            transform.localScale = new Vector3(Mathf.MoveTowards(transform.localScale.x, x, Time.deltaTime * rotateTowardsSpeed), 1, 1);
        }


        public interface IPlayerMove
        {
            public float Horizontal { get; }
            public Transform Transform { get; }
        }
    }
}