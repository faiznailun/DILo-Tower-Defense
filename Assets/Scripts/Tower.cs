using UnityEngine;

using System.Collections.Generic;



public class Tower : MonoBehaviour

{

    // Tower Component

    [SerializeField] private SpriteRenderer _towerPlace;

    [SerializeField] private SpriteRenderer _towerHead;



    // Tower Properties

    [SerializeField] private int _shootPower = 1;

    [SerializeField] private float _shootDistance = 1f;

    [SerializeField] private float _shootDelay = 5f;

    [SerializeField] private float _bulletSpeed = 1f;

    [SerializeField] private float _bulletSplashRadius = 0f;


    [SerializeField] private Bullet _bulletPrefab;



    private float _runningShootDelay;

    private Enemy _targetEnemy;

    private Quaternion _targetRotation;



    // Mengecek musuh terdekat

    public void CheckNearestEnemy(List<Enemy> enemies)

    {

        if (_targetEnemy != null)

        {

            if (!_targetEnemy.gameObject.activeSelf || Vector3.Distance(transform.position, _targetEnemy.transform.position) > _shootDistance)

            {

                _targetEnemy = null;

            }

            else

            {

                return;

            }

        }



        float nearestDistance = Mathf.Infinity;

        Enemy nearestEnemy = null;



        foreach (Enemy enemy in enemies)

        {

            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance > _shootDistance)

            {

                continue;

            }



            if (distance < nearestDistance)

            {

                nearestDistance = distance;

                nearestEnemy = enemy;

            }

        }



        _targetEnemy = nearestEnemy;

    }



    // Menembak musuh yang telah disimpan sebagai target

    public void ShootTarget()

    {

        if (_targetEnemy == null)

        {

            return;

        }



        _runningShootDelay -= Time.unscaledDeltaTime;

        if (_runningShootDelay <= 0f)

        {

            bool headHasAimed = Mathf.Abs(_towerHead.transform.rotation.eulerAngles.z - _targetRotation.eulerAngles.z) < 10f;

            if (!headHasAimed)

            {

                return;

            }



            Bullet bullet = LevelManager.Instance.GetBulletFromPool(_bulletPrefab);

            bullet.transform.position = transform.position;

            bullet.SetProperties(_shootPower, _bulletSpeed, _bulletSplashRadius);

            bullet.SetTargetEnemy(_targetEnemy);

            bullet.gameObject.SetActive(true);



            _runningShootDelay = _shootDelay;

        }

    }



    // Membuat tower selalu melihat ke arah musuh

    public void SeekTarget()

    {

        if (_targetEnemy == null)

        {

            return;

        }



        Vector3 direction = _targetEnemy.transform.position - transform.position;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _targetRotation = Quaternion.Euler(new Vector3(0f, 0f, targetAngle - 90f));



        _towerHead.transform.rotation = Quaternion.RotateTowards(_towerHead.transform.rotation, _targetRotation, Time.deltaTime * 180f);

    }

    public Tower(SpriteRenderer towerHead)
    {
        _towerHead = towerHead;
    }

    public Tower(float shootDistance)
    {
        _shootDistance = shootDistance;
    }

    public Tower(int shootPower)
    {
        _shootPower = shootPower;
    }


    // Digunakan untuk menyimpan posisi yang akan ditempati selama tower di drag

    public Vector2? PlacePosition { get; private set; }
    public SpriteRenderer TowerPlace { get => _towerPlace; set => _towerPlace = value; }
    public float ShootDelay { get => _shootDelay; set => _shootDelay = value; }

    public void SetPlacePosition(Vector2? newPosition)

    {

        PlacePosition = newPosition;

    }



    public void LockPlacement()

    {

        transform.position = (Vector2)PlacePosition;

    }



    // Mengubah order in layer pada tower yang sedang di drag

    public void ToggleOrderInLayer(bool toFront)

    {

        int orderInLayer = toFront ? 2 : 0;

        TowerPlace.sortingOrder = orderInLayer;

        _towerHead.sortingOrder = orderInLayer;

    }



    // Fungsi yang digunakan untuk mengambil sprite pada Tower Head

    public Sprite GetTowerHeadIcon()

    {

        return _towerHead.sprite;

    }

    public override bool Equals(object obj)
    {
        var tower = obj as Tower;
        return tower != null &&
               base.Equals(obj) &&
               _bulletSplashRadius == tower._bulletSplashRadius;
    }

    public override int GetHashCode()
    {
        var hashCode = -206510725;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + _bulletSpeed.GetHashCode();
        return hashCode;
    }
}


