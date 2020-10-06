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

        if (transform.localPosition.y < 0)
        {
            transform.localPosition = new Vector3(originalPosition.x, originalPosition.y, Random.Range(-4, 4));
            playerRigidBody.angularVelocity = Vector3.zero;
            playerRigidBody.velocity = Vector3.zero;
        }

        target.transform.localPosition = new Vector3(originalTargetPosition.x, originalPosition.y, Random.Range(-4, 4));


    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.transform.localPosition);
        sensor.AddObservation(playerRigidBody.velocity.x);
        sensor.AddObservation(playerRigidBody.velocity.y);

    }

    public override void OnActionReceived(float[] vectorAction)
    {
        var vectorForce = Vector3.zero;
        vectorForce.x = vectorAction[0];
        vectorForce.z = vectorAction[1];
        Debug.Log(vectorForce);
        playerRigidBody.AddForce(vectorForce * speed);

      

        var distanceFromTarget = Vector3.Distance(transform.localPosition, target.transform.localPosition);

        //we are doing good
        if(distanceFromTarget < 1.4)
        {
            SetReward(1f);
            EndEpisode();
            StartCoroutine(SwapGroundMaterial(successMaterial, 0.5f));
        }


        //we are doing not good
        if (transform.localPosition.y < 0)
        {
            EndEpisode();
            StartCoroutine(SwapGroundMaterial(failureMaterial, 0.5f));
        }
    }



    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Vertical");     // get value of z
        actionsOut[1] = Input.GetAxis("Horizontal");   // get value of x                                                     
      

}

    private IEnumerator SwapGroundMaterial(Material mat, float time)
    {
        groundMeshRenderer.material = mat;
        yield return new WaitForSeconds(time);
        groundMeshRenderer.material = defaultMaterial;
    }
}
