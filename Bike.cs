using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    /// <summary>
    /// Data model.
    /// </summary>
    [System.Serializable]
    public class BikeParameters
    {
        [Range(0.0f, 10.0f)]
        public float mass;

        [Range(0.0f, 100.0f)]
        public float thrust;

        [Range(0.0f, 100.0f)]
        public float agility;

        public float maxSpeed;

        public float maxRollSpeed;

        [Range(0.0f, 1.0f)]
        public float linearDrag;

        [Range(0.0f, 1.0f)]
        public float RollDrag;

        [Range(0.0f, 1.0f)]
        public float collisionBounceFactor;

        public bool afterburner;

        //visual models.
        public GameObject EngineModel; //ссылка на префаб
        public GameObject HullModel;

    }

    /// <summary>
    /// Сontroller. Entity
    /// </summary>
    public class Bike : MonoBehaviour
    {
        /// <summary>
        /// Data Model.
        /// </summary>
        [SerializeField] private BikeParameters m_BikeParametersInitial;

        /// <summary>
        /// View.
        /// </summary>
        [SerializeField] private BikeViewController m_VisualController;

        /// <summary>
        /// Управлелние газом байка. Нормализованное. от -1 до +1.
        /// </summary>
        private float m_ForwardThrustAxis;

        /// <summary>
        /// Установление значения педали газа.
        /// </summary>
        /// <param name="val"></param>
        public void SetForwardThrustAxis(float val) => m_ForwardThrustAxis = val;

        /// <summary>
        /// Управление отклонением влево и вправо. Нормализованное. от -1 до +1.
        /// </summary>
        private float m_HorizontalThrustAxis;

        public void SetHorizontalThrustAxis(float val)
        {
            m_HorizontalThrustAxis = val;
        }

        [SerializeField] private RaceTrack m_Track;

        private float m_Distance;
        private float m_Velocity;
        private float m_RollAngle;
        private float m_RollVelocity;

        private void Update()
        {
            //MoveBike();
            UpdateBikePhisics();
        }

        private void UpdateBikePhisics()
        {
            // S=vt
            // F=ma
            // V=V0 + at
            float dt = Time.deltaTime;

         // Begin. Движение вдоль оси трека

            float dv = dt * m_ForwardThrustAxis * m_BikeParametersInitial.thrust;

            m_Velocity += dv;

            // ограничение максимальной скорости 
            m_Velocity = Mathf.Clamp(m_Velocity, - m_BikeParametersInitial.maxSpeed, m_BikeParametersInitial.maxSpeed);

            // влияние трения на скорость
            m_Velocity += -m_Velocity * m_BikeParametersInitial.linearDrag * dt;

            float dS = m_Velocity * dt;

            // collision от прямого удара
            if (Physics.Raycast(transform.position, transform.forward, dS) || Physics.Raycast(transform.position, -transform.forward, -dS))
            {
                m_Velocity = -m_Velocity * m_BikeParametersInitial.collisionBounceFactor;
                dS = m_Velocity * dt;
             }

            m_Distance += dS;
 
            if(m_Distance < 0)
                m_Distance = 0;

            Vector3 bikePos = m_Track.GetPosition(m_Distance);
            Vector3 bikeDir = m_Track.GetDirection(m_Distance);

         // End. движение вдоль оси трека

         // Bigin. движение вокруг оси трека

            float drv = dt * m_HorizontalThrustAxis * m_BikeParametersInitial.agility; 
            
            m_RollVelocity += drv;

            // ограничение максимальной скорости вращения 
            m_RollVelocity = Mathf.Clamp(m_RollVelocity, - m_BikeParametersInitial.maxRollSpeed, m_BikeParametersInitial.maxRollSpeed);

            // влияние трения на скорость вращения
            m_RollVelocity += -m_RollVelocity * m_BikeParametersInitial.RollDrag * dt;

            float dAngle = m_RollVelocity * dt;

            // collision от бокового удара в правый борт
            if (Physics.Raycast(transform.position, transform.right, dAngle) || Physics.Raycast(transform.position, -transform.right, -dAngle)) 
            {
                m_RollVelocity = -m_RollVelocity * m_BikeParametersInitial.collisionBounceFactor;
                dAngle = m_RollVelocity * dt;
            }

            m_RollAngle += dAngle;

            // реализация направления вертикальной оси байка к оси трека
            Quaternion q = Quaternion.AngleAxis(m_RollAngle, Vector3.forward);
            Vector3 trackOffset = q * (Vector3.up * m_Track.Radius);

            transform.position = bikePos - trackOffset;

            // вращение которое характеризуется направлением вперед и вектором ввверх
            transform.rotation = Quaternion.LookRotation(bikeDir, trackOffset);

         // End. движение вокруг оси трека
        }
               
    }
}