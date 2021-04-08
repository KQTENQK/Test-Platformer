using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _offsetY;

    private void Update()
    {
        transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y + _offsetY, transform.position.z);
    }
}
