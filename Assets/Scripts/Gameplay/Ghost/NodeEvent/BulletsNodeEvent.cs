using System.Collections;
using UnityEngine;

public class BulletsNodeEvent : NodeEvent
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] int waveCount = 1;
    [SerializeField] int bulletCount = 4;
    [SerializeField] float bulletSpeed = 15;
    [SerializeField] float startAngle = 0;
    [SerializeField] float endAngle = 360;
    [SerializeField] float spiralAngleChange = 10;
    [SerializeField] float startDelayTime = 0;
    [SerializeField] float timeBetweenWaves = 1;
    [SerializeField] float endDelayTime = 1;

    MonoPool<Bullet> _pool;

    void Awake()
    {
        _pool = new MonoPool<Bullet>(bulletPrefab);
    }

    protected override IEnumerator CO_Run()
    {
        yield return new WaitForSeconds(startDelayTime);
        var adjustedStartAngle = startAngle;
        for (var i = 0; i < waveCount; ++i)
        {
            Fire(adjustedStartAngle);
            adjustedStartAngle += spiralAngleChange;
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        yield return new WaitForSeconds(endDelayTime);

        _pool.Clear();
    }

    void Fire(float adjustedStartAngle)
    {
        var angleStep = (endAngle - adjustedStartAngle) / bulletCount;
        var angle = adjustedStartAngle;
        for (var i = 0; i < bulletCount; i++)
        {
            var directionX = transform.position.x + Mathf.Sin(angle / 180f * Mathf.PI);
            var directionY = transform.position.y + Mathf.Cos(angle / 180f * Mathf.PI);
            var moveVector = new Vector2(directionX, directionY);
            var direction = (moveVector - (Vector2)transform.position).normalized;

            var bullet = _pool.Get();
            bullet.Initialize(_pool, direction, bulletSpeed);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;

            angle += angleStep;
        }
    }
}