using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinContainer : MonoBehaviour
{
    [SerializeField] private List<GameObject> _coins = new List<GameObject>();
    [SerializeField] private Player _player;

    private void OnEnable()
    {
        _player.CoinTaken += DestroyCoin;
    }

    private void OnDisable()
    {
        _player.CoinTaken -= DestroyCoin;
    }

    private void DestroyCoin(Coin coin)
    {
        for (int i = 0; i < _coins.Count; i++)
        {
            if (_coins[i].transform == coin.transform)
            {
                Destroy(_coins[i]);
                _coins.RemoveAt(i);
            }
        }
    }
}
