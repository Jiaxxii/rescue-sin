using System;
using UnityEngine;

namespace Workspace.Camera
{
    public class MoveTo : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float horizontalSpeed;
        [SerializeField] private float verticalSpeed;
        [Space] [SerializeField] private Vector2 offset;
        [SerializeField] private Vector2 rangeX;
        [SerializeField] private Vector2 rangeY;

        private void LateUpdate()
        {
            var currPos = transform.position;
            var x =
                Mathf.Clamp(Mathf.MoveTowards(currPos.x, target.position.x - offset.x, horizontalSpeed * Time.deltaTime)
                    , rangeX.x, rangeX.y);
            var y =
                Mathf.Clamp(Mathf.MoveTowards(currPos.y, target.position.y - offset.y, verticalSpeed * Time.deltaTime)
                    , rangeY.x, rangeY.y);
            transform.position = new Vector3(x, y, currPos.z);
        }
    }
}