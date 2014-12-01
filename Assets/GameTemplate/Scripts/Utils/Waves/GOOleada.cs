using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GOOleada : MonoBehaviour {
	private OleadaBase datosOleada;

	public OleadaBase DatosOleada {
		get {
			return this.datosOleada;
		}
	}
	public override string ToString ()
	{
		return string.Format ("[GOOleada: datosOleada={0}]", datosOleada);
	}
	

	public void inicializar(OleadaBase pOleada){
		datosOleada = pOleada;
	}



	private bool aplicarDecVel = false;
	private GameObject obstacle;


	void OnTriggerEnter2D(Collider2D collider){
		if(datosOleada == null) return; // para objetos combinados (el padre es un GOOleada) pero no tiene datos de oleada solo sirve para emparentar varios hijos que si tienen datos

		TerrainPart terrain = collider.GetComponent<TerrainPart> ();

		if(terrain != null && datosOleada.whereCanSpawn != null && datosOleada.whereCanSpawn.Count > 0 && !datosOleada.whereCanSpawn.Contains(terrain.Type)){
			Destroy(gameObject);
		}
	}

	void FixedUpdate () {
		if(datosOleada != null && datosOleada.canMove && datosOleada.Vivo && !GameManager.gameOver && GameManager.gameStart && this.collider2D != null){
			if(datosOleada.dirMov == Vector3.left)
				transform.position = new Vector3 (transform.position.x- datosOleada.Velocidad*Time.deltaTime, transform.position.y , transform.position.z);
			else if(datosOleada.dirMov == Vector3.left)
				transform.position = new Vector3 (transform.position.x+ datosOleada.Velocidad*Time.deltaTime, transform.position.y , transform.position.z);
			else if(datosOleada.dirMov == Vector3.up)
				transform.position = new Vector3 (transform.position.x, transform.position.y + datosOleada.Velocidad*Time.deltaTime, transform.position.z);
			else if(datosOleada.dirMov == Vector3.down)
				transform.position = new Vector3 (transform.position.x, transform.position.y - datosOleada.Velocidad*Time.deltaTime, transform.position.z);

			Vector3 dir = datosOleada.dirMov;
			Vector3 origin = transform.position+dir*this.collider2D.bounds.size.x;
			RaycastHit2D hit = Physics2D.Raycast(origin, dir, 10, datosOleada.maskForCollision);


			if (hit.collider != null && hit.collider.gameObject != this.gameObject && (obstacle == null || obstacle == hit.collider.gameObject)){
				obstacle = hit.collider.gameObject;
				float distance = Mathf.Abs(hit.point.x - transform.position.x);
				Debug.DrawLine(transform.position, hit.point, Color.red);
//					Debug.DrawLine(transform.localPosition, new Vector3(hit.point.x, hit.point.y, transform.localPosition.z), Color.red);
//					Debug.Log("Dist: " + distance);
				aplicarDecVel = true;

				if(distance <= datosOleada.minDistanciaColision){
					datosOleada.Velocidad = 0;
				}
				else if(datosOleada.Velocidad > 0)
					datosOleada.Velocidad -= datosOleada.decVelocidad;

			}
			else{
				aplicarDecVel = false;
				datosOleada.Velocidad = datosOleada.VelInicial;
			}

		}
	}


}
