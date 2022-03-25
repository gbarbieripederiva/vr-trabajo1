using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookScript : MonoBehaviour {

 
    //Aca es a donde nos vamos a dirigir
    private Vector3 grapplePoint;
    //private Vector3 currentGrapplePosition;
    //A donde nos podemos enganchar
    public LayerMask enganchable;
    public Transform gunTip , camera , player;
    private float maxDistance = 10000f;
    //private float speed = 10;
    private SpringJoint joint;    
    private bool hooked;
    
    void Awake(){
       hooked = false;
    }

    void Update(){
        if(!hooked){
           if (Google.XR.Cardboard.Api.IsTriggerPressed || Input.GetMouseButtonDown(0))
            {
                StartGrapple();    
            }
        }else{
            if (Google.XR.Cardboard.Api.IsTriggerPressed || Input.GetMouseButtonDown(0)){
                StopGrapple();
            }
        }
    }

    
    void MoveTowards(){

    }
    void StartGrapple(){
        RaycastHit hit;
        if(Physics.Raycast(camera.position , camera.forward , out hit , maxDistance , enganchable)){
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;
        
            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);
    
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.01f;

            joint.spring = 100f;
            joint.damper = 7f;
            joint.massScale = 4.5f;
            hooked = true;
        }

    }
 
    void StopGrapple(){
         hooked = false;
        Destroy(joint);
    }

    public bool IsGrappling(){
        return joint != null;
    }

    public Vector3 GetGrapplePoint(){
        return grapplePoint;
    }

}
