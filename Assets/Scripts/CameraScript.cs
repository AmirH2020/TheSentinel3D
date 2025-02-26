using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TheSentinel
{
    public class CameraScript : MonoBehaviour
    {

        [SerializeField] private Transform _target;
        [SerializeField] private float _smoothing = 5f;
        Vector3 _offset;

        void Start()
        {
            _offset = transform.position - _target.position;
        }

        // Update is called once per frame
        void Update()
        {
            var target = _target.position + _offset;
            transform.position = Vector3.Lerp(transform.position, target, _smoothing * Time.deltaTime);
        }
    }
}