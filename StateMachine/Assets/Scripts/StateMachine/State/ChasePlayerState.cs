using UnityEngine;
using System.Collections;

public class ChasePlayerState : FSMState {

	public ChasePlayerState(){
		SetStateID (StateID.ChasingPlayer);
	}


	bool _printOnce = true;
	public override void DoBeforeEntering() {
		base.DoBeforeEntering ();
		Debug.Log ("ChasePlayerState Enter");

		_printOnce = true;
	}

	/// <summary>
	/// This method decides if the state should transition to another on its list
	/// NPC is a reference to the object that is controlled by this class
	/// </summary>
	public override void Reason(GameObject player, GameObject npc){
		if(Vector3.Distance(player.transform.position,
		                    npc.transform.position) > 30){
			npc.SendMessage("OnHandlePerformTransition", Transition.LostPlayer, SendMessageOptions.RequireReceiver);
		}
	}
	
	/// <summary>
	/// This method controls the behavior of the NPC in the game World.
	/// Every action, movement or communication the NPC does should be placed here
	/// NPC is a reference to the object that is controlled by this class
	/// </summary>
	public override void Act(GameObject player, GameObject npc){
		Rigidbody npcRigidbody = npc.GetComponent<Rigidbody> ();

		Vector3 vel = npcRigidbody.velocity;
		Vector3 moveDir = player.transform.position - npc.transform.position;
		vel = moveDir.normalized * 10;

		npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
		                                          Quaternion.LookRotation(moveDir),
		                                          5 * Time.deltaTime);
		npc.transform.eulerAngles = new Vector3 (0, npc.transform.eulerAngles.y, 0);

		npcRigidbody.velocity = vel;

		if (_printOnce) {
			Debug.Log ("<color=red>Chase</color>");
			_printOnce = false;
		}
	}
}
