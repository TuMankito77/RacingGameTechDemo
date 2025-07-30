namespace RacingGameDemo.Runtime.Gameplay.Car
{
    using UnityEngine;
    
    using GameBoxSdk.Runtime.Input;

    public class BaseCar : MonoBehaviour, IInputControlableEntity
    {
        [SerializeField]
        private Rigidbody carRB = null;

        [SerializeField]
        private Transform[] rayPoints = new Transform[0];

        [SerializeField]
        private LayerMask drivable = default(LayerMask);

        [Header("Suspension Settings")]

        [SerializeField]
        private float springStiffness = 30000;

        [SerializeField]
        private float damperStiffness = 3000;

        [SerializeField]
        private float restLegnth = 1;

        [SerializeField]
        private float springTravel = 0.5f;

        [SerializeField]
        private float wheelRadius = 0.33f;

        [Header("Debugging")]

        [SerializeField]
        private bool enableDebugging = false;

        float springCompression = 0;

        #region Unity Methods

        private void FixedUpdate()
        {
            Suspension();
        }

        private void OnDrawGizmos()
        {
            if(enableDebugging)
            {
                foreach(Transform rayPoint in rayPoints)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(rayPoint.position, rayPoint.position + (restLegnth) * -rayPoint.up);
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(rayPoint.position + (restLegnth - springCompression) * -rayPoint.up, wheelRadius);
                }
            }
        }

        #endregion

        private void Suspension()
        {
            foreach(Transform rayPoint in rayPoints)
            {
                RaycastHit hit = default(RaycastHit);
                float maxLength = restLegnth + springTravel;

                if (Physics.Raycast(rayPoint.position, -rayPoint.up, out hit, maxLength + wheelRadius, drivable))
                {
                    float currentSpringLength = hit.distance - wheelRadius;
                    springCompression = restLegnth - currentSpringLength;
                    float springCompressionNormalized = (restLegnth - currentSpringLength) / springTravel;
                    float springVelocity = Vector3.Dot(carRB.GetPointVelocity(rayPoint.position), rayPoint.up);
                    float dampForce = damperStiffness * springVelocity;
                    float springForce = springStiffness * springCompressionNormalized;
                    float netForce = springForce - dampForce;
                    carRB.AddForceAtPosition(netForce * rayPoint.up, rayPoint.position);
                    Debug.DrawLine(rayPoint.position, hit.point, Color.red);
                }
                else
                {
                    Debug.DrawLine(rayPoint.position, rayPoint.position + (maxLength + wheelRadius) * -rayPoint.up, Color.green);
                }
            }
        }


        public void Dispose()
        {

        }
    }
}

