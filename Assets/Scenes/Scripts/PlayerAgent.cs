using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PlayerAgent : Agent
{

    private Rigidbody playerRigidBody;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float speed = 10f;

    [Tooltip("How fast the agent turns")]
    public float turnSpeed = 180f;

    private Vector3 originalPosition;

    private Vector3 originalTargetPosition;

    [SerializeField]
    private Material failureMaterial;

    [SerializeField]
    private Material successMaterial;

    [SerializeField]
    private Material defaultMaterial;

    [SerializeField]
    private MeshRenderer groundMeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
      
        
    }

    public override void Initialize()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        originalPosition = transform.localPosition;
        originalTargetPosition = target.transform.localPosition;
    }

    public override void OnEpisodeBegin()
    {

      
            transform.localPosition = new Vector3(originalPosition.x, originalPosition.y, Random.Range(-4, 4));   
            target.transform.localPosition = new Vector3(originalTargetPosition.x, originalPosition.y, Random.Range(-4, 4));
            transform.localRotation = Quaternion.Euler(new Vector3(0,-90,0));
       

    }

    public override void CollectObservations(VectorSensor sensor)
    {

        //sensor.AddObservation(transform.localPosition);
        //sensor.AddObservation(target.transform.localPosition);
       // sensor.AddObservation(playerRigidBody.velocity.x);
        //sensor.AddObservation(playerRigidBody.velocity.y);

    }
    
    //https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Learning-Environment-Design-Agents.md
    float forwardAmount = 0;
    float turnAmount = 0;
    public override void OnActionReceived(float[] vectorAction)
    {
        /*
         MOVING
         */
        if (vectorAction[0] == 0)
        {            
            AddReward(-0.1f);
           // return;
        }

        if (vectorAction[0] == 1)
        {
              forwardAmount = 1f;
              playerRigidBody.MovePosition(transform.position + transform.forward * forwardAmount * speed * Time.fixedDeltaTime);      
                    
        }


        /*
         ROTATING
         */

        // Convert the second action to turning left or right

        if (vectorAction[1] == 1f) //move left
        {
            turnAmount = -1f;
        }
        else if (vectorAction[1] == 2f)//move right
        {
            turnAmount = 1f;
        }
        else if (vectorAction[1] == 0)
        {
            turnAmount = 0f;
        }
       
        transform.Rotate(transform.up * turnAmount * turnSpeed * Time.fixedDeltaTime);
        //Debug.Log(rotation);



        if (transform.localPosition.y < 0)
        {
            EndEpisode();
            StartCoroutine(SwapGroundMaterial(failureMaterial, 0.5f));
        }        

       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("target"))
        {
            AddReward(10);
            StartCoroutine(SwapGroundMaterial(successMaterial, 0.5f));
            EndEpisode();

        }
    }



    public override void Heuristic(float[] actionsOut)
    {
        //actionsOut[0] = Input.GetAxis("Vertical");     // get value of z
        //actionsOut[1] = Input.GetAxis("Horizontal");   // get value of x                                         
        actionsOut[0] = 0f;
        actionsOut[1] = 0f;

        //forward
        if (Input.GetKey(KeyCode.Z))
        {
            actionsOut[0] = 1f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            actionsOut[1] = 1f;
        }
        //rotate
        if (Input.GetKey(KeyCode.D))
        {
            actionsOut[1] = 2f;
        }
        if (Input.GetKey(KeyCode.X))//stop turning & moving
        {
            actionsOut[0] = 0;
            actionsOut[1] = 0;
        }


    }

    private IEnumerator SwapGroundMaterial(Material mat, float time)
    {
        groundMeshRenderer.material = mat;
        yield return new WaitForSeconds(time);
        groundMeshRenderer.material = defaultMaterial;
    }
}
