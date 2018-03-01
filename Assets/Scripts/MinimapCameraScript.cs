using UnityEngine;

namespace Assets.Scripts
{
    public class MinimapCameraScript: MonoBehaviour
    {

        public Transform target;
        public Vector3 offset;
        public bool freezeY = true;

        void Start()
        {
            transform.position = target.position + offset;
        }

        void LateUpdate () {
            if (freezeY)
            {
                transform.position = new Vector3(target.position.x + offset.x, transform.position.y, target.position.z + offset.z);
            }
            else
            {
                transform.position = target.position + offset;
            }
        }
    }
}
