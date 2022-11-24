using UnityEngine;
using System.Collections;

/// ------------------------------------------
/// <summary>
/// 
///     Makes an object move back-and-forth 
///     repeatedly.
///     
///     Attach this script to the gameobject
///     that needs to move.
///     
/// </summary>
/// ------------------------------------------
public class Oscillate : MonoBehaviour
{

	[Tooltip("How far to move from the start point")]
	public Vector3 distance = new Vector3(0, 1, 0);

    [Tooltip("Time period (in seconds) for movement.")]
    public float travelTime = 1;

    [Tooltip("Accelerate and decelerate?")]
    public bool smooth = true;

	private Vector3 origin;
	private float elapsedTime;
	private bool reverse;

	void Start()
	{
		origin = transform.localPosition;
		reverse = false;
	}

	void Update()
	{

		if (travelTime >= 0)
		{

			// Where is the target?
			Vector3 target = origin + distance;

			// Are we there yet?
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= travelTime)
			{
				elapsedTime = 0;
				reverse = !reverse;
			}

			// Move the object
			float proportion = elapsedTime / travelTime;
			if (smooth)
			{
				proportion = 0.5f - Mathf.Cos(proportion * 180.0f * Mathf.Deg2Rad) / 2;
			}
			if (reverse)
			{
				transform.localPosition = Vector3.Lerp(target, origin, proportion);
			}
			else
			{
				transform.localPosition = Vector3.Lerp(origin, target, proportion);
			}

		}

	}

}
