using UnityEngine;
using UnityEngine.Networking;
using Holoville.HOTween;

public class PlayerController : NetworkBehaviour {
	Rigidbody player;

    public float turnSpeed = 5.0f;
    public float yRotation = 5.0F;

    void Start ()
	{
		player = gameObject.GetComponent<Rigidbody> ();
	}
	
	void Update ()
	{
        if (!isLocalPlayer)
        {
            // exit from update if this is not the local player
            return;
        }

        // current rotation
        Vector3 CR = transform.rotation.eulerAngles;
        // Camera transform
        Vector3 CT = Camera.main.transform.position;

		if (Input.GetKey (KeyCode.W)) {
            // player.AddForce (transform.forward * 300f);
            // transform.position += transform.position.normalized;

            //player.velocity = (transform.InverseTransformDirection(transform.forward) * 3f);
            player.AddRelativeForce(transform.forward * 300f);
        }

        if (Input.GetKey (KeyCode.D)) {
            if (CR.z > 344f || CR.z < 17f)
                HOTween.To(transform, Time.deltaTime, new TweenParms().Prop("eulerAngles", new Vector3(0, 0, -15f * Time.deltaTime), true));
		}

		if (Input.GetKey (KeyCode.A)) {
            if (CR.z < 16f || CR.z > 343f)
                HOTween.To(transform, Time.deltaTime, new TweenParms().Prop("eulerAngles", new Vector3(0, 0, 15f * Time.deltaTime), true));
        }

        // Tween back to 0 rotation
        if (CR.z > 300f)
        {
            HOTween.To(transform, 5, new TweenParms().Prop("eulerAngles", new Vector3(0, 0, 359.999f), false));
        }
        else
        {
            HOTween.To(transform, 5, new TweenParms().Prop("eulerAngles", new Vector3(0, 0, 0), false));
        }
    
        Camera.main.transform.position = new Vector3(CT.x, CT.y, transform.position.z - 5);

        yRotation += Input.GetAxis("Horizontal") * turnSpeed;
        transform.eulerAngles = new Vector3(0, yRotation, transform.eulerAngles.z);
    }
}
