using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GeneratePathArray : MonoBehaviour {

	public Transform[] m_PathArray;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[ContextMenu("AssignPathArray")]
	void AssignPathArray(){
		m_PathArray = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			m_PathArray[i] = transform.GetChild(i);
		}
	}
}
