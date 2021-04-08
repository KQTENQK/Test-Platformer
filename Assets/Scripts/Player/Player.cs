using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public int CoinCount { get; private set; }

    public event UnityAction<Coin> CoinTaken;

    private void Start()
    {
        CoinCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Coin>(out Coin coin))
        {
            CoinTaken?.Invoke(coin);
            CoinCount++;
        }
    }
}
