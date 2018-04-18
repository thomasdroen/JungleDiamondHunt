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
        public AudioSource[] feet;
        [HideInInspector]
        public bool inWater = false;
        public AudioClip[] footstepClips;
        public AudioClip[] footstepWaterClips;
        private int feetIndex = 0;

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
            float xPos = m_OriginalCameraPosition.x + (Bobcurve.Evaluate(m_CyclePositionX) * HorizontalBobRange);
            float yPos = m_OriginalCameraPosition.y + (Bobcurve.Evaluate(m_CyclePositionY) * VerticalBobRange);

            m_CyclePositionX += (Mathf.Sqrt(speed) * Time.deltaTime) / m_BobBaseInterval * 4;
            m_CyclePositionY += ((Mathf.Sqrt(speed) * Time.deltaTime) / m_BobBaseInterval) * VerticaltoHorizontalRatio * 4;

            if (m_CyclePositionX > m_Time)
            {
                m_CyclePositionX = -m_Time;
            }
            if (m_CyclePositionY > m_Time)
            {
                m_CyclePositionY = -m_Time;
                PlayFootstepSound(inWater ? footstepWaterClips : footstepClips);
            }

            return new Vector3(bobHorizontal ? xPos : 0f, yPos, 0f);
        }

        private void PlayFootstepSound(AudioClip[] clips)
        {
            if (feet.Length <= 0)
            {
                Debug.LogError("AudioSources not found.");
                return;
            }

            if (clips.Length <= 0)
            {
                Debug.LogError("clips are empty!");
                return;
            }


            int randomClipIndex = Mathf.RoundToInt(Random.value * (clips.Length - 1));
            AudioSource source = feet[feetIndex];
            source.clip = clips[randomClipIndex];
            source.pitch = 1.1f - Random.value * 0.2f;
            source.Play();

            feetIndex = feetIndex + 1 >= feet.Length ? 0 : feetIndex + 1;

        }
    }
}
