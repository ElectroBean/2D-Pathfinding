using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDirectionsAvailable : MonoBehaviour {

    public enum Directions
    {
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }

    public List<Directions> availableDirections;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public List<Directions> GetDirections()
    {
        List<Directions> toBeReturned = new List<Directions>();

        if (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y) + new Vector2(0, 1) * 4.5f, new Vector2(1, 1), 0))
            toBeReturned.Add(Directions.Up);
        if (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y) + new Vector2(0, -1) * 4.5f, new Vector2(1, 1), 0))
            toBeReturned.Add(Directions.Down);
        if(Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y) + new Vector2(1, 0) * 4.5f, new Vector2(1, 1), 0))
            toBeReturned.Add(Directions.Right);
        if (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y) + new Vector2(-1, 0) * 4.5f, new Vector2(1, 1), 0))
            toBeReturned.Add(Directions.Left);

        return toBeReturned;
    }
}
