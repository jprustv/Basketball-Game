/*
  Code adapted from: www.gdmag.com nov 1998 article by Jeff Lander
*/
using UnityEngine;
using System.Collections;

public class IKCCD2D : IKSolver {

	public bool Damping = false; //Allow some delay in the motion?
	public float Damping_Max = 0.5f; //Maximum radians per iteration if damping is on
	public Vector3 RotationAxis = new Vector3(0,0,-1); //the axis on the bones that we'll be using to rotate around
	
	private float IK_POS_THRESH = 0.125f; //How close we need to be to skip the calculations.
	private float ANGLE_THRESH = 0.999f; //a little trick to avoid the NaN return from the Dot(180)
	private int MAXLOOPSPERFRAME = 10; //max loops to try before we let it slip to the next frame
	private float STEPSIZE = 3.0f; //multiplier to use to set the max movement per frame (Speed of following)
		
	public override void Solve(Transform[] bones, Vector3 target) {

		Transform endEffector = bones[bones.Length-1];
		Vector3 curEnd = Vector3.zero;	
		Vector3 rootPos = Vector3.zero;
		Vector3 crossResult = Vector3.zero;	
		Vector3 targetDirection = Vector3.zero;	
		Vector3 currentDirection = Vector3.zero;	

		float theDot=0;
		float turnRadians=0;
		
		int link=0; //which bone are we moving right now.
		int tries=1; //number of tries to match it up this frame.
		
		//start from the end effector.
		curEnd = endEffector.position;
		
		//we only want the Y rotations for now
		curEnd.y = 0; //0 out the end_bodypart height
		target.y = 0; //0 out the target height
		
		while (tries < MAXLOOPSPERFRAME && (curEnd-target).sqrMagnitude > IK_POS_THRESH)
		{
			//If we're at the top of the array, start over from the bottom
			if (link < 0) 
				{ link = bones.Length-1; }
			
			//Get the current link to rotate from the array, and its position
			//This is the main changing value in the set
			rootPos=bones[link].position;
			
			//Get the current positions of the target and end effector every loop
			curEnd=endEffector.position;
			
			//Only dealing with Y rotations for now.. more to come!		
			curEnd.y = 0; //0 out the end_bodypart height
			//target.y = 0; //0 out the target height
			rootPos.y = 0; //0 out the current links height Because we're doing 2D
			
			//The direction we are pointing, is the direction FROM the current links loop
			//TO the End_Bodypart.. 
			//the Direction we WANT to point, is 
			currentDirection =  curEnd - rootPos;
			targetDirection =  target - rootPos;
			
			
			//Dot product needs normalized vectors! (Slowest part of the process)
			currentDirection.Normalize();
			targetDirection.Normalize();
			theDot = Vector3.Dot(currentDirection,targetDirection);
		
			//If the DotProduct is below our angle threshold, we're already very nearly pointing the
			//linkage straight at the target.. so skip the rotations and send us out for a new frame
			//It also gives us the distance we need to turn to match the currentDirection TO the
			//targetDirection..
			
			//If the angle is exactly 180 degrees, skip this iteration..
			if (theDot > -ANGLE_THRESH && theDot < ANGLE_THRESH)
			{
				turnRadians = Mathf.Acos(theDot);
				
				//The sign of the y value of the CrossProduct tells us which 
				//direction we need to turn the current linkage!
				crossResult = Vector3.Cross(currentDirection,targetDirection);

				if (crossResult.y > 0.0f)
				{	
					if (Damping)
					{
							if (turnRadians > Damping_Max)
								turnRadians = Damping_Max;
					}
					bones[link].Rotate(RotationAxis,-turnRadians*STEPSIZE);
				}
				if (crossResult.y < 0.0f)
				{
					if (Damping)
					{
							if (turnRadians > Damping_Max)
							turnRadians = Damping_Max;
					}
					bones[link].Rotate(RotationAxis,turnRadians*STEPSIZE);
				}
			}
			tries++;
			link--; //move to the next link up the chain
		}//end while not-done
	}
}
