using UnityEngine;
using System.Collections;

public class CameraVelocity : MonoBehaviour
{
	public ConfgPlayer[] configs;
	public UILabel lbVelocidad;

//    // script to move camera
//    public float speed = 7;
//	public float ritmo = 0.2f;
//	public float inc = 0.001f;
//	public int maxIncsARealizar = 500;
//	public bool atrezo = false;
	
	private bool iniciado = false;
	private int numIncRealizados = 0;
	private bool interrumpido = false;
	private Vector3 posInterrupido = Vector3.zero;

    // script to move camera
	public float speed = 7;
	public float ritmo = 0f;
	public float incVelocidad = 0;
	private int maxIncsARealizar = 500;
	public bool atrezo = false;

	[SerializeField]
	private float unidadesXPorMetro = 5f;

	private float velocidadInicial;
	private int metroActual = 0;
	private float velocidadFinal;

	public float VelocidadFinal {
		get {
			return this.velocidadFinal;
		}
	}

	public float Speed {
		get {
			return this.speed;
		}
	}

	public float VelocidadInicial {
		get {
			return this.velocidadInicial;
		}
	}

	void Awake(){
		selectConfig ();
		StartCoroutine (iniciar ());
	}

    void Update()
    {



		if(atrezo || !GameManager.isGameOver && GameManager.gameStart){


        	rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);

//			Debug.Log(transform.position.x);

			int metro = Mathf.RoundToInt(transform.position.x);

			if(transform.position.x != 0 && metro != metroActual && metro % unidadesXPorMetro == 0){
//				GameManager.Instance.Metros++;
				metroActual = metro;
			}
		}
    }

	private void selectConfig(){
		if(configs != null && configs.Length > 0){
			ConfgPlayer config = null;

			foreach(ConfgPlayer c in configs){
				if(c.difficulty == GameManager.Instance.Difficulty){
					config = c;
					break;
				}
			}

			if(config != null){
				speed = config.speed;
				ritmo = config.rate;
				velocidadFinal = config.finalSpeed;
				atrezo = config.isProps;
				incVelocidad = config.incSpeed;
			}
		}

		velocidadInicial = speed;
		maxIncsARealizar = (int) ((velocidadFinal - speed) / incVelocidad);
	}

	public void stop(){
		speed = 0;
		rigidbody2D.velocity = Vector3.zero;
	}

	private IEnumerator iniciar(){
		if(!atrezo && !GameManager.isGameOver && GameManager.gameStart)
			StartCoroutine(incrementaVel());
		else{
			yield return new WaitForSeconds(0.5f);
			StartCoroutine(iniciar());
		}
	}
	
	private IEnumerator incrementaVel(){
		if(!interrumpido && !GameManager.isGameOver){
			yield return new WaitForSeconds (ritmo);
			
			if(!interrumpido){
				speed += incVelocidad;
				numIncRealizados++;
				
				if(numIncRealizados < maxIncsARealizar)
					StartCoroutine(incrementaVel());
			}
			
			
		}
	}
}

