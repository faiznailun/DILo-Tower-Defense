using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour

{

    [SerializeField] private int _maxHealth = 1;

    [SerializeField] private float _moveSpeed = 1f;

    [SerializeField] private SpriteRenderer _healthBar;

    [SerializeField] private SpriteRenderer _healthFill;



    private int _currentHealth;


    public Vector3 TargetPosition { get; private set; }

    public int CurrentPathIndex { get; private set; }

    public Enemy(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }

    public Enemy(SpriteRenderer healthFill)
    {
        _healthFill = healthFill;
    }

    public SpriteRenderer HealthBar { get => _healthBar; set => _healthBar = value; }

    public override bool Equals(object obj)
    {
        var enemy = obj as Enemy;
        return enemy != null &&
               base.Equals(obj) &&
               EqualityComparer<SpriteRenderer>.Default.Equals(HealthBar, enemy.HealthBar);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }



    // Fungsi ini terpanggil sekali setiap kali menghidupkan game object yang memiliki script ini

    private void OnEnable()

    {

        _currentHealth = _maxHealth;

        _healthFill.size = HealthBar.size;

    }

    public void MoveToTarget()

    {

        transform.position = Vector3.MoveTowards(transform.position, TargetPosition, _moveSpeed * Time.deltaTime);

    }



    public void SetTargetPosition(Vector3 targetPosition)

    {

        TargetPosition = targetPosition;

        _healthBar.transform.parent = null;



        // Mengubah rotasi dari enemy

        Vector3 distance = TargetPosition - transform.position;

        if (Mathf.Abs(distance.y) > Mathf.Abs(distance.x))

        {

            // Menghadap atas

            if (distance.y > 0)

            {

                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));

            }

            // Menghadap bawah

            else

            {

                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -90f));

            }

        }

        else

        {

            // Menghadap kanan (default)

            if (distance.x > 0)

            {

                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

            }

            // Menghadap kiri

            else

            {

                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));

            }

        }


        public void ReduceEnemyHealth(int damage)

        {

            _currentHealth -= damage;

            AudioPlayer.Instance.PlaySFX("hit-enemy");

            if (_currentHealth <= 0)

            {

                gameObject.SetActive(false);

                AudioPlayer.Instance.PlaySFX("enemy-die");

            }

        }



        _healthBar.transform.parent = transform;

    }



    // Menandai indeks terakhir pada path

    public void SetCurrentPathIndex(int currentIndex)

    {

        CurrentPathIndex = currentIndex;

    }

}