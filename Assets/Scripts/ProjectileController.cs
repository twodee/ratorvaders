using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {
  public float speed = 10;
  public AudioClip explodeClip;
  public AudioClip ughClip;
  public PlayerController player;

  void Start() {
    player = GameObject.Find("/Player").GetComponent<PlayerController>();
  }
  
  void Update() {
    if (transform.position.y > Camera.main.orthographicSize) {
      Destroy(gameObject);
    }

    transform.position += speed * Vector3.up * Time.deltaTime;
  }

  void OnDestroy() {
    player.Reload();
  }

  void OnTriggerEnter2D(Collider2D collider) {
    if (collider.CompareTag("Expression")) {
      Destroy(gameObject);
      /* Precedence precedence = collider.gameObject.GetComponent<Precedence>(); */
      /* if (precedence.isHighestPrecedence) { */
        /* collider.gameObject.transform.parent.GetComponent<ExpressionController>().Regenerate(); */
        /* AudioSource.PlayClipAtPoint(explodeClip, transform.position); */
      /* } else { */
        /* AudioSource.PlayClipAtPoint(ughClip, transform.position); */
      /* } */
    }
  }
}
