using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour 
{
	[SerializeField]
	Transform focus = default;

	[SerializeField, Range(1f, 100f)]
	public float distance = 5f;

	[SerializeField, Range(1f, 360f)]
	float rotationSpeed = 90f;

	Vector2 orbitAngles = new Vector2(45f, 0f);

	private Vector3 previousPosition;

	private Camera cam;

	private bool lockedOut;

	bool succesfulClick;

	void Start()
	{
		cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		lockedOut = false;
		succesfulClick = false;
	}

    void ManualRotation()
	{
		Vector2 input = new Vector2(
			Input.GetAxis("Vertical"),
			-Input.GetAxis("Horizontal")
		);
		const float e = 0.001f;
		if (input.x < -e || input.x > e || input.y < -e || input.y > e)
		{
			orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
		}
	}

	void LateUpdate()
	{
        if (control.isPaused() != control.pauseStates.menuPaused)
        {
            ManualRotation();
        }
        Vector3 focusPoint = focus.position;
        Quaternion lookRotation = Quaternion.Euler(orbitAngles);
        Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookPosition = focusPoint - lookDirection * distance;
        transform.SetPositionAndRotation(lookPosition, lookRotation);

		// FOR MOUSE DRAG MOVEMENT
		// https://github.com/EmmaPrats/Camera-Rotation-Tutorial

		EventSystem eventSys = GameObject.Find("EventSystem").GetComponent<EventSystem>();

		if (Input.GetMouseButtonDown(0) && ! (eventSys.IsPointerOverGameObject() || lockedOut))
		{
			previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
			succesfulClick = true;
			//orbitAngles = new Vector2(cam.transform.rotation.x, cam.transform.rotation.y);

		}
		else if (Input.GetMouseButton(0) && succesfulClick)
		{
			Vector3 newPosition = cam.ScreenToViewportPoint(Input.mousePosition);
			Vector3 direction = previousPosition - newPosition;

			float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
			float rotationAroundXAxis = direction.y * 180; // camera moves vertically

			cam.transform.position = focus.position;

			cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
			cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World); // <— This is what makes it work!

			cam.transform.Translate(new Vector3(0, 0, -distance));

			previousPosition = newPosition;

			Vector3 pos = cam.transform.rotation.eulerAngles;

			orbitAngles = new Vector2(pos.x, pos.y);
		}else if (Input.GetMouseButtonUp(0))
		{
			succesfulClick = false;
		}
	}

	public void LockOut() {
		GameObject.Find("Main Camera").GetComponent<OrbitCamera>().lockedOut = true;
	}


	public void UnlockOut()
	{
		GameObject.Find("Main Camera").GetComponent<OrbitCamera>().lockedOut = false;

	}
}