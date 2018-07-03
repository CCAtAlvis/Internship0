using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform playerTransform;
	public float depth = -5f;

	// Update is called once per frame
	void Update()
	{
		if(playerTransform != null)
		{
			transform.position = playerTransform.position + new Vector3(0, 2.6f, depth);
			transform.LookAt (playerTransform);
		}
	}

	public void setTarget(Transform target)
	{
		playerTransform = target;
	}
}