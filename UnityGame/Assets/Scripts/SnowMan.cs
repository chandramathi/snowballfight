using UnityEngine;

public class SnowMan : MonoBehaviour
{
    private int headHits = 0;
    private int torsoHits = 0;

    [Header("Head Stages")]
    public GameObject head;
    public GameObject headHalf;
    public GameObject headQuarter;

    [Header("Torso Elements")]
    public GameObject buttons;
    public GameObject lefteye;
    public GameObject righteye;
    public GameObject nose;
    public GameObject rightarm;
    public GameObject leftarm;
    public GameObject torso;
    public GameObject torsoHalf;
    public GameObject torsoQuarter;
    public GameObject body;
    public GameObject bodyHalf;

    [Header("Shattering Effect")]
    public GameObject snowShatterEffect; // Assign in the Inspector


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Snowman frame");
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            if(headHits<4){
                RegisterHit("head");
            }else{
                RegisterHit("torso");
            }
        }
    }

    // Call this function when the snowman is hit
    public void RegisterHit(string hitArea)
    {
        if (hitArea == "head")
        {
            headHits++;
            UpdateHead();
        }
        else if (hitArea == "torso")
        {
            torsoHits++;
            UpdateTorso();
        }
    }

    private void UpdateHead()
    {

        if (headHits == 1)
        {
            CreateShatterEffect(head.transform.position); // Add effect
            if (head) head.SetActive(false);
            if (headHalf) headHalf.SetActive(true);
            if (righteye) righteye.SetActive(false); //DetachAndDrop(righteye);
        }
        else if (headHits == 2)
        {
            CreateShatterEffect(headHalf.transform.position); // Add effect
            if (headHalf) headHalf.SetActive(false);
            if (headQuarter) headQuarter.SetActive(true);
            if (lefteye) lefteye.SetActive(false);//DetachAndDrop(lefteye);
        } 
        else if (headHits == 3)
        {
            CreateShatterEffect(headQuarter.transform.position); // Add effect
            if (headQuarter) headQuarter.SetActive(false);
            if (nose) DetachAndDrop(nose);
        }
    }

    private void UpdateTorso()
    {
        if (headHits >=3)
        {
            if(torsoHits==1){
                CreateShatterEffect(torso.transform.position); // Add effect
                if (buttons) buttons.SetActive(false);
                if (torso) torso.SetActive(false);
                if (torsoHalf) torsoHalf.SetActive(true);
                if (rightarm) DetachAndDrop(rightarm);
            }
            if(torsoHits==2){
                CreateShatterEffect(torsoHalf.transform.position); // Add effect
                if (leftarm) DetachAndDrop(leftarm);
                if (torsoHalf) torsoHalf.SetActive(false);
                if (torsoQuarter) torsoQuarter.SetActive(true);
            }
            if(torsoHits==3){
                    CreateShatterEffect(torsoQuarter.transform.position); // Add effect
                if (torsoQuarter) torsoQuarter.SetActive(false);
            }
            if(torsoHits>3){
                CreateShatterEffect(body.transform.position); // Add effect
                if (body) body.SetActive(false);
                if (bodyHalf) bodyHalf.SetActive(true);
            }
        }
    }

    private void DetachAndDrop(GameObject arm)
    {
        // Remove the arm from the snowman hierarchy
        arm.transform.parent = null;

        // Add a Rigidbody if not already present
        Rigidbody rb = arm.GetComponent<Rigidbody>();
        if (!rb)
        {
            rb = arm.AddComponent<Rigidbody>();
        }

        // Enable gravity and remove constraints if any
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;

        // Optionally, apply a small force to make it fall realistically
        rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
    }

    private void CreateShatterEffect(Vector3 position)
    {
        if (snowShatterEffect)
        {
            GameObject effect = Instantiate(snowShatterEffect, position, Quaternion.identity);
            Destroy(effect, 4f); // Destroy after 2 seconds
        }
    }

}
