using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour 
{
	[SerializeField]
	Transform focus = default;

	[SerializeField, Range(1f, 100f)]
	float distance = 5f;

	[SerializeField, Range(1f, 360f)]
	float rotationSpeed = 90f;

	Vector2 orbitAngles = new Vector2(45f, 0f);

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
        if (control.isPaused() == control.pauseStates.unPaused)
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