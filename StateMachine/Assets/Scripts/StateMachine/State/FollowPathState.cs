using UnityEngine;
using System.Collections;

public class FollowPathState : FSMState {

	public int _currentWayPoint;
	public Transform[] _waypoints;

	public FollowPathState(Transform[] vWayPoints){
		_currentWayPoint = 0;
		_waypoints = vWayPoints;
		SetStateID (StateID.FollowingPath);
	}

	bool _printOnce = true;
	public override void DoBeforeEntering() { 
		base.DoBeforeEntering ();
		Debug.Log ("FollowPathState Enter");
		_currentWayPoint = 0;

		_printOnce = true;
	}
	/// <summary>
	/// This method decides if the state should transition to another on its list
	/// NPC is a reference to the object that is controlled by this class
	/// </summary>
	public override void Reason(GameObject vPlayer, GameObject vNPC){
	
		//RaycastHit hit;
		int mask = LayerMask.GetMask ("Player");
		if (Physics.Raycast (vNPC.transform.position,
		                     vNPC.transform.forward,
		                     15.0f,
		                     mask)) {
			vNPC.SendMessage("OnHandlePerformTransition", Transition.SawPlayer, SendMessageOptions.RequireReceiver);
		}

	}
	
	/// <summary>
	/// This method controls the behavior of the NPC in the game World.
	/// Every action, movement or communication the NPC does should be placed here
	/// NPC is a reference to the object that is controlled by this class
	/// </summary>
	public override void Act(GameObject player, GameObject npc){
		Rigidbody npcRigidbody = npc.GetComponent<Rigidbody> ();

		Vector3 vec = npcRigidbody.velocity;
		Vector3 moveDir = _waypoints [_currentWayPoint].position - npc.transform.position;

		if (moveDir.magnitude < 1) {
			_currentWayPoint++;
			if(_currentWayPoint > _waypoints.Length - 1){
				_currentWayPoint = 0;
			}
		} else {
			vec = moveDir.normalized * 10;

			npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
			                                          Quaternion.LookRotation(moveDir),
			                                          5 * Time.deltaTime);

			npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);
		}

		npcRigidbody.velocity = vec;

		if (_printOnce == true) {
			Debug.Log ("<color=red>Follow</color>");
			_printOnce = false;
		}
	}
}
