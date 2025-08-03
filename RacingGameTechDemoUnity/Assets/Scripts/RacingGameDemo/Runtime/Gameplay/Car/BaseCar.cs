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
        private Transform accelerationPoint = null;

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

        [Header("Car Settings")]

        [SerializeField]
        private float acceleration = 25f;

        [SerializeField]
        private float maxSpeed = 100f;

        [SerializeField]
        private float deceleration = 10f;

        [SerializeField]
        private float steerStrength = 15f;

        [SerializeField]
        private AnimationCurve turningCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [SerializeField]
        private float dragCoefficient = 1f; 

        [Header("Debugging")]

        [SerializeField]
        private bool enableDebugging = false;

        [SerializeField, Min(0)]
        private float accelerationPointLineSize = 1;

        private float springCompression = 0;
        private int[] wheelsIsGrounded = null;
        private bool isGrounded = false;
        private float moveInput = 0;
        private float steerInput = 0;
        private Vector3 currentCarLocalVelocity = Vector3.zero;
        private float carVelocityRatio = 0;

        #region Unity Methods

        private void Awake()
        {
            wheelsIsGrounded = new int[rayPoints.Length];
        }

        private void Update()
        {
            //Replace this so that it is controlled by GameBoxSdk's input system. 
            GetPlayerInput();
        }

        private void FixedUpdate()
        {
            Suspension();
            GroundCheck();
            CalculateCarVelocity();
            Movement();
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

                if(accelerationPoint != null)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(accelerationPoint.position, accelerationPoint.position + accelerationPoint.forward * accelerationPointLineSize);
                }
            }
        }

        #endregion

        public void Dispose()
        {

        }

        private void Suspension()
        {
            for(int i = 0; i < rayPoints.Length; i++) 
            {
                Transform rayPoint = rayPoints[i];
                RaycastHit hit = default(RaycastHit);
                float maxLength = restLegnth + springTravel;

                if (Physics.Raycast(rayPoint.position, -rayPoint.up, out hit, maxLength + wheelRadius, drivable))
                {
                    wheelsIsGrounded[i] = 1;
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
                    wheelsIsGrounded[i] = 0;
                    Debug.DrawLine(rayPoint.position, rayPoint.position + (maxLength + wheelRadius) * -rayPoint.up, Color.green);
                }
            }
        }

        private void GroundCheck()
        {
            int tempGroundedWheels = 0;

            foreach(int wheelIsGrounded in wheelsIsGrounded)
            {
                tempGroundedWheels += wheelIsGrounded;
            }

            isGrounded = tempGroundedWheels > 0;
        }

        private void GetPlayerInput()
        {
            moveInput = Input.GetAxis("Vertical");

            if(moveInput != 0)
            {
                Debug.Log("We are receiving horizontal input!");
            }
            steerInput = Input.GetAxis("Horizontal");
        }

        private void CalculateCarVelocity()
        {
            currentCarLocalVelocity = transform.InverseTransformDirection(carRB.linearVelocity);
            carVelocityRatio = currentCarLocalVelocity.z / maxSpeed;
        }

        private void Movement()
        {
            if(isGrounded)
            {
                Acceleration();
                Deceleration();
                Turn();
                SidewaysDrag();
            }
        }

        private void Acceleration()
        {
            carRB.AddForceAtPosition(acceleration * moveInput * transform.forward, accelerationPoint.position, ForceMode.Acceleration);
        }

        private void Deceleration()
        {
            carRB.AddForceAtPosition(deceleration * moveInput * -transform.forward, accelerationPoint.position, ForceMode.Acceleration);
        }

        private void Turn()
        {
            carRB.AddTorque(steerStrength * steerInput * turningCurve.Evaluate(carVelocityRatio) * Mathf.Sign(carVelocityRatio) * transform.up, ForceMode.Acceleration);
        }

        private void SidewaysDrag()
        {
            float currentSidewaysSpeed = currentCarLocalVelocity.x;
            float dragMagnitude = -currentSidewaysSpeed * dragCoefficient;
            Vector3 dragforce = transform.right * dragMagnitude;
            carRB.AddForceAtPosition(dragforce, carRB.worldCenterOfMass, ForceMode.Acceleration);
        }
    }
}

