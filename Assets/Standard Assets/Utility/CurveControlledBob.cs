using System;
using UnityEngine;
using Random = UnityEngine.Random;


namespace UnityStandardAssets.Utility
{
    [Serializable]
    public class CurveControlledBob
    {
        public float HorizontalBobRange = 0.33f;
        public float VerticalBobRange = 0.33f;
        public AnimationCurve Bobcurve = new AnimationCurve(new Keyframe(0f, -1f), new Keyframe(1f, 1f),
                                                            new Keyframe(2f, -1f)); // sin curve for head bob
        public float VerticaltoHorizontalRatio = 1f;

        [Space]
        public AudioSource audioSource;
        public AudioClip[] footstepClips;

        private float m_CyclePositionX;
        private float m_CyclePositionY;
        private float m_BobBaseInterval;
        private Vector3 m_OriginalCameraPosition;
        private float m_Time;


        public void Setup(Camera camera, float bobBaseInterval)
        {
            m_BobBaseInterval = bobBaseInterval;
            m_OriginalCameraPosition = camera.transform.localPosition;

            // get the length of the curve in time
            m_Time = Bobcurve[Bobcurve.length - 1].time;
        }


        public Vector3 DoHeadBob(float speed, bool bobHorizontal)
        {
            float xPos = m_OriginalCameraPosition.x + (Bobcurve.Evaluate(m_CyclePositionX)*HorizontalBobRange);
            float yPos = m_OriginalCameraPosition.y + (Bobcurve.Evaluate(m_CyclePositionY)*VerticalBobRange);

            m_CyclePositionX += (speed*Time.deltaTime)/m_BobBaseInterval;
            m_CyclePositionY += ((speed*Time.deltaTime)/m_BobBaseInterval)*VerticaltoHorizontalRatio;

            if (m_CyclePositionX > m_Time)
            {
                m_CyclePositionX =- m_Time;
            }
            if (m_CyclePositionY > m_Time)
            {
                m_CyclePositionY =- m_Time;
                if (footstepClips.Length > 0)
                {
                    PlayFootstepSound();
                }
            }

            return new Vector3(bobHorizontal ? xPos : 0f, yPos, 0f);
        }

        private void PlayFootstepSound()
        {
            if (audioSource == null)
            {
                Debug.LogError("AudioSource not found.");
                return;
            }
            int randomClipIndex = Mathf.RoundToInt(Random.value * (footstepClips.Length - 1));
            audioSource.clip = footstepClips[randomClipIndex];
            audioSource.Play();
        }
    }
}
