using System.Collections;
using UnityEngine;

public class BulletsCombatEvent : CombatEvent
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] int waveCount = 1;
    [SerializeField] int bulletCount = 4;
    [SerializeField] float bulletSpeed = 15;
    [SerializeField] float startAngle = 0;
    [SerializeField] float endAngle = 360;
    [SerializeField] float spiralAngleChange = 10;
    [SerializeField] float startDelayTime;
    [SerializeField] float timeBetweenWaves;

    public override void Run()
    {
        StartCoroutine(CO_Fire());
    }

    IEnumerator CO_Fire()
    {
        yield return new WaitForSeconds(startDelayTime);
        var adjustedStartAngle = startAngle;
        for (var i = 0; i < waveCount; ++i)
        {
            Fire(adjustedStartAngle);
            adjustedStartAngle += spiralAngleChange;
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        IsDone = true;
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

            var bullet = Instantiate(bulletPrefab);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.gameObject.SetActive(true);
            bullet.Initialize(direction, bulletSpeed);

            angle += angleStep;
        }
    }
}