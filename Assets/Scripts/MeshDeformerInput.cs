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
		Vector3 collisionPoint = transform.position;
		bool audioPlayed = false;

        foreach (ContactPoint contact in collision.contacts)
        {
            MeshDeformer deformer = contact.otherCollider.GetComponent<MeshDeformer>();
			AudioSource audio = contact.otherCollider.GetComponent<AudioSource>();
			ParticleSystem particle = contact.otherCollider.GetComponent<ParticleSystem>();
			if (!audioPlayed) {
				audio.Play();
				particle.Play();
				audioPlayed = true;
			}
			if (deformer) {
				Vector3 point = contact.point;
				collisionPoint = contact.point;
				point += contact.normal * forceOffset;
				deformer.AddDeformingForce(point, force);
			}
        }
		/*GameObject part = GameObject.Find("Paille");
		part.transform.position = collisionPoint;
		ParticleSystem particles = part.GetComponent<ParticleSystem>();
			if (particles != null) {
				if (!particles.isEmitting) {
					particles.Play();
				}
			}else {
				print("Prolème");
			}*/
    }

	/*private void OnTriggerEnter(Collider other) {
        ParticleSystem particles = GetComponent<ParticleSystem>();
		if (particles != null) {
			print("Moins de problèmes  : " + particles.isPlaying);
			particles.Play();
		}else {
			print("Prolème");
		}
    }*/
}