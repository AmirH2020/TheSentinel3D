using UnityEngine;


namespace TheSentinel
{
    public class FaceTheCamera : MonoBehaviour
    {
        void Update()
        {
            transform.rotation = (Camera.main.transform.rotation);
        }
    }
}
