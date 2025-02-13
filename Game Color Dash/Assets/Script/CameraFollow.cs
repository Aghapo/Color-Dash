using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform player; // Oyuncu karakteri


    // Update is called once per frame
    void Update() {
        if (player != null) {
            Vector3 newPos = new Vector3(player.position.x, transform.position.y, player.position.z - 5f);
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 5f);
        }
    }
}
