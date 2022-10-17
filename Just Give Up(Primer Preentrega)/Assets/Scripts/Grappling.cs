using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("Ref")]
    private PlayerMovement pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask isGrappable;
    public LineRenderer lr;

    [Header("Grappling")]
    public float maxGrapplingDist;
    public float grappDelay;
    public float overshootYAxis;

    Vector3 grapplePoint;

    [Header("Cds")]
    public float grappCd;
    public float grappCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

    private bool grappling;


    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if(Input.GetKeyDown(grappleKey)) 
        {
            StartGrapp();
        }

        if(grappCdTimer > 0)
        {
            grappCdTimer -= Time.deltaTime;
        }

    }

    void LateUpdate()
    {
        if(grappling)
        {
            lr.SetPosition(0, gunTip.position);
        }
    }

    void StartGrapp()
    {
        if(grappCdTimer > 0) return;
        
        grappling = true;

        

        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrapplingDist, isGrappable))
        {
            grapplePoint = hit.point;

            Invoke(nameof(GetGrapp), grappDelay);
        }
            else
            {
                grapplePoint = cam.position + cam.forward * maxGrapplingDist;

                Invoke(nameof(NotGrapp), grappDelay);
            }

            lr.enabled = true;
            lr.SetPosition(1, grapplePoint);
        }

   void GetGrapp()
    {
      

        Vector3 lowerPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        
        float grapplePointRelativeYPos = grapplePoint.y - lowerPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if(grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;
        

        pm.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(NotGrapp), 1f);
    }

    void NotGrapp()
    {
        
        grappling = false;
        grappCdTimer = grappCd;
        lr.enabled = false;
    }

}



