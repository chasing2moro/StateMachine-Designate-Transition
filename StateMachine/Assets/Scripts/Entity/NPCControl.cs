using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class NPCControl : MonoBehaviour {
	public GeneratePathArray m_PathArrayGenerator;

	public GameObject player;
	public Transform[] path;
	private FSMSystem fsm;

	public void SetTransition(Transition t){
		fsm.PerformTransition (t);
	}
	// Use this for initialization
	void Start () {
		path = m_PathArrayGenerator.m_PathArray;

		MakeFSM ();
	}

	void MakeFSM(){
		FollowPathState state1 = new FollowPathState (path);
		state1.AddTransition (Transition.SawPlayer, StateID.ChasingPlayer);

		ChasePlayerState state2 = new ChasePlayerState ();
		state2.AddTransition (Transition.LostPlayer, StateID.FollowingPath);

		fsm = new FSMSystem ();
		fsm.AddState (state1);
		fsm.AddState (state2);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		fsm.CurrentState.Reason (player, this.gameObject);
		fsm.CurrentState.Act (player, this.gameObject);
	}

	void OnHandlePerformTransition(Transition vTransition){
		SetTransition (vTransition);
		//throw new System.NotImplementedException();
	}

}
