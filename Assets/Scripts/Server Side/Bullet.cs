using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {

    [SerializeField]
    float speed;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().velocity = transform.forward.normalized * speed;
	}
	

}
