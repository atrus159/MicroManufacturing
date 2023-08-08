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

	public bool lockedOut;

	void Start()
	{
		cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		lockedOut = false;
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
		if (lockedOut) {
			return;
		}
		// FOR MOUSE DRAG MOVEMENT
		// https://github.com/EmmaPrats/Camera-Rotation-Tutorial

		EventSystem eventSys = GameObject.Find("EventSystem").GetComponent<EventSystem>();

		if (Input.GetMouseButtonDown(0) || eventSys.IsPointerOverGameObject())
		{
			previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
			//orbitAngles = new Vector2(cam.transform.rotation.x, cam.transform.rotation.y);

		}
		else if (Input.GetMouseButton(0))
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
		}
		else {

			// FOR ARROW/WASD CONTROLS
			if (control.isPaused() != control.pauseStates.menuPaused)
			{
				ManualRotation();
			}
			Vector3 focusPoint = focus.position;
			Quaternion lookRotation = Quaternion.Euler(orbitAngles);
			Vector3 lookDirection = lookRotation * Vector3.forward;
			Vector3 lookPosition = focusPoint - lookDirection * distance;
			transform.SetPositionAndRotation(lookPosition, lookRotation);
		}
	}
}