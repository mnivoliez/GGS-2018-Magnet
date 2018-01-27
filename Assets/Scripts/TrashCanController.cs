using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanController : MonoBehaviour {

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "MagnetCore")
		{
			collider.gameObject.transform.parent.GetComponent<MagnetController>().isOnTrashcan = true;
		}
	}

	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag == "MagnetCore")
		{
			collider.gameObject.transform.parent.GetComponent<MagnetController>().isOnTrashcan = false;
		}
	}
}
