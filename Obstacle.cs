using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private RaceTrack m_Track;
        [SerializeField] private float m_RollAngle;
        [SerializeField] private float m_Distance;

        [Range(-20.0f, 20.0f)]
        [SerializeField] private float m_RadiusModifier = 0;
        
        [SerializeField] private float m_RotationSpeed;


        private void OnValidate()
        {
            SetObstacleposition();
        }

        // считаем позицию
        private void SetObstacleposition()
        {
            Vector3 obstaclePos = m_Track.GetPosition(m_Distance);
            Vector3 obstacleDir = m_Track.GetDirection(m_Distance);

            Quaternion q = Quaternion.AngleAxis(m_RollAngle, Vector3.forward);
            Vector3 trackOffset = q * (Vector3.up * (m_RadiusModifier * m_Track.Radius));

            transform.position = obstaclePos - trackOffset;
            transform.rotation = Quaternion.LookRotation(obstacleDir, trackOffset);

        }
        private void Update()
        {
            //MoveBike();
            UpdateObstaclePhisics();
        }

        private void UpdateObstaclePhisics()
        {
            // вращение с заданной скоростью
            float dt = Time.deltaTime;

            Vector3 ObstaclePos = m_Track.GetPosition(m_Distance);
            Vector3 ObstacleDir = m_Track.GetDirection(m_Distance);

            float dAngle = m_RotationSpeed * dt;

            m_RollAngle += dAngle;

            // реализация направления вертикальной оси препятствия к оси трека
            Quaternion q = Quaternion.AngleAxis(m_RollAngle, Vector3.forward);
            Vector3 trackOffset = q * (Vector3.up * m_Track.Radius* m_RadiusModifier);

            transform.position = ObstaclePos - trackOffset;

            // вращение которое характеризуется направлением вперед и вектором ввверх
            transform.rotation = Quaternion.LookRotation(ObstacleDir, trackOffset);
        }

        private void OnDrawGizmos()
        {
           Gizmos.color = Color.red;

           Vector3 centerlinePos = m_Track.GetPosition(m_Distance);
           Gizmos.DrawSphere(centerlinePos, m_Track.Radius);

        }
    }
}
