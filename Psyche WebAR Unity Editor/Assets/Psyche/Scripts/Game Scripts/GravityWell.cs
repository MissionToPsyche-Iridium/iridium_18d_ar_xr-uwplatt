using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWell : MonoBehaviour
{
    [SerializeField] private float objectMass = 10f;

    private Rigidbody2D player_rb2d;
    private Player playerScript;
    private Transform Satellite;
    private float wellRadius;

    private void Start()
    {
        player_rb2d = Player.Instance.GetComponent<Rigidbody2D>();
        playerScript = Player.Instance.GetComponent<Player>();
        Satellite = Player.Instance.transform;
        wellRadius = GetComponent<CircleCollider2D>().radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform == Satellite)
        {
            playerScript.InGravityWell = true;
            Vector2 direction = (Vector2)(transform.position - col.transform.position);
            float distance = direction.magnitude;

            if (distance > 0f)
            {
                Vector2 force = direction.normalized * objectMass * wellRadius / (distance * distance); // inverse-square law
                player_rb2d.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform == Satellite)
        {
            playerScript.InGravityWell = false;
        }
    }

}