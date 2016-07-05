using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {
  public float speed = 10;
  public AudioClip explodeClip;
  public AudioClip ughClip;
  private PlayerController player;
  private bool isDead;

  void Start() {
    player = GameObject.Find("/Player").GetComponent<PlayerController>();
    isDead = false;
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

  void OnTriggerStay2D(Collider2D collider) {
    if (!isDead && collider.CompareTag("Expression")) {
      if (ExpressionController.singleton.OnExpressionHit(collider.gameObject, transform.position.y, gameObject)) {
        isDead = true;
      }
    }
  }
}
