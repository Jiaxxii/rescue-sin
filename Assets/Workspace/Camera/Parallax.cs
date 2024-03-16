using System;
using UnityEngine;

namespace Workspace.Camera
{
    public class Parallax : MonoBehaviour
    {
        [Range(-1f, 1f)] [SerializeField] private float parallaxEffect; // 视差效果强度，-1到1之间的值，负值表示背景移动方向与相机相反  
        private float _startPosition; // 背景层的初始位置  
        private Transform _cameraTransform; // 主相机的引用  

        private void Start()
        {
            _startPosition = transform.position.x; // 获取背景层的初始X位置  
            _cameraTransform = UnityEngine.Camera.main.transform;
        }

        private Vector3 _previousCamPos;

        private void Update()
        {
            // 计算相机与初始位置之间的水平差异  
            var dist = _cameraTransform.position.x * parallaxEffect - _startPosition;

            // 移动背景层  
            transform.position = new Vector3(_startPosition + dist, transform.position.y, transform.position.z);
        }
    }
}