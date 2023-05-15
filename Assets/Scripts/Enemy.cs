using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform enemy;

    private void Update()
    {
        transform.localScale = enemy.localScale;
    }
}