using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
  public float speed;
  public Collider2D boundsCollider;
  public GameObject projectile;
  public Transform projectiles;
  public GameObject gameOverMessage;
  public bool isDead;
  public GameObject ammo;
  public GameObject avatar;

  private float radius;

  void Start() {
    // We need to know how wide the player is to keep her on screen.
    radius = boundsCollider.bounds.extents.x;

    // All projectiles will be parented to an organizing empty.
    projectiles = GameObject.Find("/Projectiles").transform;

    isDead = false;
  }
  
  void Update() {
    if (isDead) return;

    // Move player left or right based on user input.
    float h = Input.GetAxis("Horizontal");
    Vector3 position = transform.position;
    position.x += h * speed * Time.deltaTime;

    // Keep player on screen.
    float right = Camera.main.orthographicSize * Camera.main.aspect;
    position.x = Mathf.Clamp(position.x, -right + radius, right - radius);

    transform.position = position;

    // Shoot projectile.
    if (ammo.activeInHierarchy && Input.GetButtonDown("Fire1")) {
      GameObject projectileInstance = (GameObject) Instantiate(projectile, transform.position, Quaternion.identity);
      projectileInstance.transform.parent = projectiles;
      ammo.SetActive(false);
    }
  }

  void OnTriggerEnter2D(Collider2D collider) {
    if (collider.CompareTag("Expression")) {
      isDead = true;
      StartCoroutine(GameOver());
    }
  }

  IEnumerator GameOver() {
    GetComponent<AudioSource>().Play();

    gameOverMessage.SetActive(true);
    avatar.SetActive(false);

    yield return new WaitForSeconds(2.0f);

    Text text = gameOverMessage.GetComponent<Text>();
    float targetTime = 5.0f;
    float startTime = Time.time;
    float elapsedTime = Time.time - startTime;
    Color rgba = text.color;
    while (elapsedTime < targetTime) {
      float proportion = elapsedTime / targetTime;
      rgba.a = 1.0f - proportion;
      text.color = rgba;
      yield return null;
      elapsedTime = Time.time - startTime;
    }

    gameOverMessage.SetActive(false);
    rgba.a = 1.0f;
    text.color = rgba;
    ExpressionController.singleton.GetComponent<ExpressionController>().Regenerate();
    avatar.SetActive(true);
    isDead = false;
  }

  public void Reload() {
    ammo.SetActive(true);
  }
}
