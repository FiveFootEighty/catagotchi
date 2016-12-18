using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlBehavior : MonoBehaviour {

    public CatBehavior catBehavior;

	void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            catBehavior.BowlOnTheGround(true, transform);
        }
    }
}
