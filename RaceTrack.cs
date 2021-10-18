using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    /// <summary>
    /// Базовый класс который определяет нашу трубу для гонок.
    /// </summary>
    public abstract class RaceTrack : MonoBehaviour
    {
        /// <summary>
        /// Радиус трубы.
        /// </summary>
        [Header("Base track properties")]
        [SerializeField] private float m_Radius;
        public float Radius => m_Radius; //гетр значения

        /// <summary>
        /// Метод возвращает длину трека.
        /// </summary>
        /// <returns></returns>
        public abstract float GetTrackLength();

        /// <summary>
        /// Метод возвращает позицию в 3д кривой цетр-линии трубы
        /// </summary>
        /// <param name="distance">дистанция от начала трубы до ее GetTrackLength</param>  
        /// <returns></returns>
        public abstract Vector3 GetPosition(float distance);

        /// <summary>
        /// Метод возвращает направление в 3д кривой центр-линии трубы.
        /// Касательная к кривой в точке.
        /// </summary>
        /// <returns></returns>
        public abstract Vector3 GetDirection(); // убрал float distance 


        /// <summary>
        /// Метод возвращает новое положение тела.
        /// </summary>
        /// <returns></returns>
        public abstract Vector3 Move(GameObject MovingBody); 

    }
}
