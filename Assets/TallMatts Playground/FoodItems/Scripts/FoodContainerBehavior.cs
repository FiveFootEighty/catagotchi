using UnityEngine;
using System.Collections;

public class FoodContainerBehavior : HoldableObject {

    public Transform foodPiece;
    public Transform exitPoint;

    public int foodCount;

    private float waitTime = 0.2f;
    private float nextSpawnTime = 0;
	
	public override void PostUpdate () {

        if (foodCount > 0)
        {
            if (nextSpawnTime < Time.time)
            {
                if ((transform.rotation.eulerAngles.x > 90 && transform.rotation.eulerAngles.x < 270) || (transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 270))
                {
                    Instantiate(foodPiece, exitPoint.position, exitPoint.rotation);
                    foodCount--;
                    nextSpawnTime = Time.time + waitTime;
                }
            }
        }
	}
}
