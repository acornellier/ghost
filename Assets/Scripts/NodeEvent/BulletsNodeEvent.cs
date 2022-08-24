using System.Collections;
using UnityEngine;
using Zenject;

public class BulletsNodeEvent : NodeEvent
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] int waveCount = 1;
    [SerializeField] int bulletCount = 4;
    [SerializeField] float bulletSpeed = 15;
    [SerializeField] float startAngle = 0;
    [SerializeField] float endAngle = 360;
    [SerializeField] float spiralAngleChange = 10;
    [SerializeField] float timeBetweenWaves = 1;

    [Inject] Ghost _ghost;

    MonoPool<Bullet> _pool;

    void Awake()
    {
        _pool = new MonoPool<Bullet>(bulletPrefab);
    }

    void FixedUpdate()
    {
        transform.position = _ghost.transform.position;
    }

    protected override IEnumerator CO_Run()
    {
        _ghost.StartAttacking();

        var adjustedStartAngle = startAngle;
        for (var i = 0; i < waveCount; ++i)
        {
            Fire(adjustedStartAngle);
            adjustedStartAngle += spiralAngleChange;
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        _pool.Clear();
    }

    void Fire(float adjustedStartAngle)
    {
        var angleStep = (endAngle - startAngle) / bulletCount;
        var angle = adjustedStartAngle;
        for (var i = 0; i < bulletCount; i++)
        {
            var directionX = transform.position.x + Mathf.Sin(angle / 180f * Mathf.PI);
            var directionY = transform.position.y + Mathf.Cos(angle / 180f * Mathf.PI);
            var moveVector = new Vector2(directionX, directionY);
            var direction = (moveVector - (Vector2)transform.position).normalized;
            d.log("bullet", angle, direction);

            var bullet = _pool.Get();
            bullet.Initialize(_pool, direction, bulletSpeed);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;

            angle += angleStep;
        }
    }
}