using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    /// <summary>
    /// Класс линейного трека.
    /// </summary>
  public class RaceTrackLinear : RaceTrack
    {
        [Header("Linear track properties")]
        [SerializeField] private Transform m_Start;
        [SerializeField] private Transform m_End;
        [SerializeField] private float m_Speed;

        public override Vector3 GetDirection() // убрал float distance 
        {
            //distance = Mathf.Clamp(distance, 0, GetTrackLength()); // пока не нужно
            return (m_End.position - m_Start.position).normalized;
        }

        public override Vector3 GetPosition(float distance)
        {
            //distance = Mathf.Clamp(distance, 0, GetTrackLength());

            Vector3 direction = GetDirection();

            //direction *= distance; 
            // код для зацикливания линейного трека
            // хотел найти код метода Mathf.repeat но не получилось
            // пришлось придумывать самому, есть + по сравнению с Math.Repeat
            if (distance < 0)
            {
                direction *= GetTrackLength() + distance % GetTrackLength();
            }
            else if (distance > GetTrackLength())
                     {
                        direction *= distance % GetTrackLength();
                     }
                 else
                    {
                        direction *= distance;
                    } 

            return m_Start.position + direction;
        }

        public override Vector3 Move(GameObject MovingBody)
        {
            float speed = m_Speed * Time.fixedDeltaTime; // для перевода скорости в [ед.мира/сек]

            return GetPosition(MovingBody.transform.position.magnitude + speed);
        }

        public override float GetTrackLength()
        {
            Vector3 direction = m_End.position - m_Start.position;

            return direction.magnitude; 
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(m_Start.position, m_End.position);
        }

        [SerializeField] private float m_TestDistance;
        [SerializeField] private Transform m_TestObject;
        
        #region Test

        private void OnValidate()
        {
            m_TestObject.position = GetPosition(m_TestDistance);
            m_TestObject.forward = GetDirection(); // убрал m_TestDistance
        }

        #endregion

        private void FixedUpdate()
        {
            m_TestObject.position = Move(m_TestObject.gameObject);
            m_TestObject.forward = GetDirection();
        }

    }
}
