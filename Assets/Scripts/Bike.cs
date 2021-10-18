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

        [Range(0.0f, 10.0f)]
        public float thrust;

        [Range(0.0f, 10.0f)]
        public float agility;
        public float maxSpeed;

        public bool afterburner;

        //visual models.
        public GameObject EngineModel; //ññûëêà íà ïðåôàá
        public GameObject HullModel;

    }

    /// <summary>
    /// Ñontroller. Entity
    /// </summary>
    public class Bike : MonoBehaviour
    {
        /// <summary>
        /// Data Model.
        /// </summary>
        [SerializeField] private BikeParameters m_BikeParametersInital;

        /// <summary>
        /// View.
        /// </summary>
        [SerializeField] private BikeViewController m_VisualController;

        private BikeParameters m_EffectiveParameters;
 
        #region Unity events

        private GameObject CreateNewPrefabInstance(GameObject soursePrefab)
        {
            return Instantiate(soursePrefab);
        }

        [SerializeField] private GameObject m_Prefab;

        private void Update()
        {
           if(Input.GetKeyDown(KeyCode.Space))
            {
                CreateNewPrefabInstance(m_Prefab);
            }

        }

        #endregion

    }
}