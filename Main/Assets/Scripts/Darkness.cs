using UnityEngine;
using System.Collections;

public class Darkness : MonoBehaviour
{
    public GameObject[] menusToDisable;
	public Camera camera;
	public float offsetX;
	
	public float beginSpeed;
	public float offsetStartX;
	
	float lastX = 0;
	float currentSpeed = 0;
	
	bool keepGoing = false;
	
	enum State
	{
		WaitingForInput,
		Beginning,
		Middle,
		End
	}
	
	State state;
	
	void Start () 
	{
		state = State.WaitingForInput;
	}
	
	float GetCameraRightX()
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
		var rightPlanePos = -planes[1].normal * planes[1].distance;
		return rightPlanePos.x- offsetX;
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (state)
		{
			case State.WaitingForInput:
			{
				if (Input.GetKey(KeyCode.Return))
				{
				    foreach (var guy in GameObject.FindGameObjectsWithTag("big_guy"))
				    {
				        guy.GetComponent<BigBadGuy>().enabled = true;
				    }

                    foreach (var menuItem in menusToDisable)
                    {
                        menuItem.SetActiveRecursively(false);
                    }

					transform.position = new Vector3(GetCameraRightX() + offsetStartX, transform.position.y, transform.position.z);
					state = State.Beginning;
				}
			
				break;
			}
			case State.Beginning:
			{
				transform.position = transform.position + Vector3.right * beginSpeed * Time.deltaTime;
			
				if (transform.position.x < GetCameraRightX())
				{
					camera.GetComponent<CameraController>().enabled = true;
					state = State.Middle;
				}
			
				break;
			}
			case State.Middle:
			{
				transform.position = new Vector3(GetCameraRightX(), transform.position.y, transform.position.z);
				
				currentSpeed = transform.position.x - lastX;
				break;
			}
			case State.End:
			{
				transform.position = transform.position + Vector3.right * currentSpeed;
				break;
			}
		}
		
		lastX = transform.position.x;
		
	}
	
	public void ContinueGoing()
	{
		state = State.End;
	}
}
