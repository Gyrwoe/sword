using UnityEngine;

public class MeshDeformerInput : MonoBehaviour {
	
	public float force = 10f;
	public float forceOffset = 0.1f;
	
	/*void Update () {
		if (Input.GetMouseButton(0)) {
			HandleInput();
		}
	}*/

	void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            MeshDeformer deformer = contact.otherCollider.GetComponent<MeshDeformer>();
			print(contact.otherCollider);
			if (deformer) {
				Vector3 point = contact.point;
				point += contact.normal * forceOffset;
				deformer.AddDeformingForce(point, force);
			}
			print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
            // Visualize the contact point
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
    }
}