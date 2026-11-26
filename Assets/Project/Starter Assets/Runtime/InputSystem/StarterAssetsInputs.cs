using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool Locked = false;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		[Header("Shooting Settings")]
		public float ProjectileSpeed;
		[SerializeField] private GameObject Projectile;
		[SerializeField] private Transform SpawnPoint;

#if ENABLE_INPUT_SYSTEM
		// On Inputs
		public void OnMove(InputValue value)
		{
			if (Locked)
				return;

			MoveInput(value.Get<Vector2>());
		}
		public void OnLook(InputValue value)
		{
			if (Locked)
				return;

			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}
		public void OnJump(InputValue value)
		{
			if (Locked)
				return;

			JumpInput(value.isPressed);
		}
		public void OnSprint(InputValue value)
		{
			if (Locked)
				return;
				
			SprintInput(value.isPressed);
		}
		public void OnShoot(InputValue value)
		{
			if (Locked)
				return;

			ShootInput();
		}
#endif
		// Input Results
		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 
		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}
		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}
		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		public void ShootInput()
		{
			GameObject ProjectileObject = Instantiate(Projectile, SpawnPoint.position, SpawnPoint.rotation);
			ProjectileObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,0, ProjectileSpeed), ForceMode.Impulse);
			Destroy(ProjectileObject, 5f);
			Debug.Log($"Player shot Projectile from X: {ProjectileObject.transform.position.x} Y: {ProjectileObject.transform.position.y} Z: {ProjectileObject.transform.position.z}");
		}

		// Other Methods
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}
		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}