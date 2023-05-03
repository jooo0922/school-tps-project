using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MimicSpace
{
    /// <summary>
    /// This is a very basic movement script, if you want to replace it
    /// Just don't forget to update the Mimic's velocity vector with a Vector3(x, 0, z)
    /// </summary>
    public class Movement : MonoBehaviour
    {
        [Header("Stats")]
        public float speed = 2f;
        public float velocityLerpCoef = 4f;

        [Header("Nav Agent")]
        public Transform mimicAgentTransform;

        private Vector3 velocity = Vector3.zero;
        private Mimic myMimic;

        private void Start()
        {
            myMimic = GetComponent<Mimic>();
        }

        private void Update()
        {
            // Assigning velocity to the mimic to assure great leg placement
            velocity = new Vector3(mimicAgentTransform.position.x, 0f, mimicAgentTransform.position.z) - new Vector3(transform.position.x, 0f, transform.position.z);
            velocity *= speed;
            myMimic.velocity = velocity;

            transform.position = transform.position + velocity * Time.deltaTime;
            Vector3 destHeight = new Vector3(transform.position.x, mimicAgentTransform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, destHeight, velocityLerpCoef * Time.deltaTime);
        }
    }

}