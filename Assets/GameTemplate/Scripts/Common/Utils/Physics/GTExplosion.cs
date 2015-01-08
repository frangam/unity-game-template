using UnityEngine;
using System.Collections;

public class GTExplosion : MonoBehaviour {
	[SerializeField]
	private GameObject pbExplosion;

	[SerializeField]
	private float expDelay = 3;

	[SerializeField]
	private float radius = 5f;

	[SerializeField]
	private float damage = 200f;

	public void explode(){
//		GameObject go = pbExplosion.Spawn(transform.position, transform.rotation);
		BaseSoundManager.Instance.play(ZSSoundIDs.BARREL_EXPLOSION_FX);

		AreaDamageEnemies(transform.position, radius, damage);

		Destroy(this.gameObject, expDelay);
	}

	void AreaDamageEnemies(Vector3 location, float radius, float damage) { 
		Collider[] objectsInRange = Physics.OverlapSphere(location, radius); 

		foreach (Collider col in objectsInRange) { 
			ZSZombie enemy = col.GetComponent<ZSZombie>(); 
			if (enemy != null) { 
				// linear falloff of effect 
				float proximity = (location - enemy.transform.position).magnitude; 
				float effect = 1 - (proximity / radius);
				int finalDamage = (int) (damage * effect);

				enemy.Status.ApplyDamage(finalDamage, Vector3.zero, false);
			}
		}
	}
}
