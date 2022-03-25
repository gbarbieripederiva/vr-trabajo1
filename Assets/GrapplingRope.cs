using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    private Spring spring;
    private LineRenderer lineRenderer;
    public GrapplingHookScript grapplingGun;
    private Vector3 currentGrapplePosition;
    public int quality;
    public float damper;
    public float velocity;
    public float strength;
    public float waveCount;
    public float waveHeight;
    public AnimationCurve affectCurve; 

    void LateUpdate(){
        DrawRope();
    }

    void Awake(){
        spring = new Spring();
        spring.SetTarget(0);
        lineRenderer = GetComponent<LineRenderer>();
    }

    void DrawRope(){
        
        if(!grapplingGun.IsGrappling()){
            currentGrapplePosition = grapplingGun.gunTip.position;
            spring.Reset();
            if(lineRenderer.positionCount > 0 ){
                lineRenderer.positionCount = 0;
            }

            return;
        }   
        if(lineRenderer.positionCount == 0 ){
            spring.SetVelocity(velocity);
            lineRenderer.positionCount = quality + 1;
        } 
        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        Vector3 grapplePoint = grapplingGun.GetGrapplePoint();
        Vector3 gunTipPosition = grapplingGun.gunTip.position;

        Vector3 up = Quaternion.LookRotation((grapplePoint-gunTipPosition).normalized) * Vector3.up;
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition , grapplePoint , Time.deltaTime * 12f);
        //se van?
        //lineRenderer.SetPosition(0,grapplingGun.gunTip.position);
      //  lineRenderer.SetPosition(1,grapplePoint);
        for(var i = 0 ; i < quality + 1 ; i++ ){
            var delta = i/ (float) quality;

            var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value *  affectCurve.Evaluate(delta);
            lineRenderer.SetPosition(i , Vector3.Lerp(gunTipPosition , currentGrapplePosition , delta) + offset); 
        }
    }
}
