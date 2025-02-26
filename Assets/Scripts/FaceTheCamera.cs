using UnityEngine;


namespace TheSentinel
{
    public class FaceTheCamera : MonoBehaviour
    {
        void Update()
        {
            transform.LookAt(Camera.main.transform.position);
            transform.Rotate(0, 180, 0);
        }
    }
}
