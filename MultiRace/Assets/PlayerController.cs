using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Holoville.HOTween;

public class PlayerController : NetworkBehaviour {
	Rigidbody player;

    public float turnSpeed = 5.0f;
    public float yRotation = 5.0F;
	public float force = 175f;
	public Text GameOver;

	[SerializeField]
	private float boostTime = 2f;
	private float appliedForce;
	private Vector3 PT;

	[SyncVar]
	public int counter = 0;

    void Start ()
	{
		player = gameObject.GetComponent<Rigidbody> ();
		PT = transform.position;
		GameOver = GameObject.FindObjectOfType<Text> ();
	}
	
	void Update ()
	{
        if (!isLocalPlayer)
            return;

		appliedForce = force;
		if (Input.GetKey (KeyCode.Space) && boostTime >= 0f) {
			boostTime -= Time.deltaTime;
			appliedForce *= 3;
		}
	
        // current rotation
        Vector3 CR = transform.rotation.eulerAngles;
		PT = transform.position;

		if (Input.GetKey (KeyCode.W))
            player.AddRelativeForce(transform.forward * appliedForce);

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
            HOTween.To(transform, 5, new TweenParms().Prop("eulerAngles", new Vector3(0, 0, 359.999f), false));
        else
            HOTween.To(transform, 5, new TweenParms().Prop("eulerAngles", new Vector3(0, 0, 0), false));

        yRotation += Input.GetAxis("Horizontal") * turnSpeed;
        transform.eulerAngles = new Vector3(0, yRotation, transform.eulerAngles.z);
    }

	public override void OnStartLocalPlayer () {
		Camera.main.GetComponent<CameraController>().setTarget(gameObject.transform);
		this.gameObject.name = counter.ToString ();
		counter++;
	}

	[Command]
	public void CmdSetWinner(string name)  {
		RpcDoMagic (name);
	}

	[ClientRpc]
	public void RpcDoMagic(string name)
	{
		GameOver.text = this.gameObject.name;
		Debug.Log ("gameObject name: " + this.gameObject.name);
		Debug.Log ("incomming name: " + name);
		if (name == this.gameObject.name) {
			GameOver.text = "You Won !!!";
		} else {
			GameOver.text = "You lost :(";
		}

		Time.timeScale = 0;
	}
}
