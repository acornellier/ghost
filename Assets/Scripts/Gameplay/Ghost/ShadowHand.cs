using System.Collections;
using UnityEngine;

public class ShadowHand : MonoBehaviour
{
    [SerializeField] Transform prepareTarget;
    [SerializeField] Transform chargeTarget;

    Vector3 _intialPosition;
    State _state = State.Inactive;

    void Awake()
    {
        _intialPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (_state != State.Charging) return;

        var playerHealth = col.GetComponent<PlayerHealth>();
        if (playerHealth)
            playerHealth.Health -= 1;
    }

    public void Prepare(float prepareSpeed)
    {
        _state = State.Preparing;
        StartCoroutine(CO_Prepare(prepareSpeed));
    }

    IEnumerator CO_Prepare(float prepareSpeed)
    {
        while (_state == State.Preparing &&
               Vector2.Distance(transform.position, prepareTarget.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                prepareTarget.position,
                Time.deltaTime * prepareSpeed
            );
            yield return null;
        }
    }

    public IEnumerator CO_Charge(float chargeSpeed)
    {
        _state = State.Charging;

        while (Vector2.Distance(transform.position, chargeTarget.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                chargeTarget.position,
                Time.deltaTime * chargeSpeed
            );
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        while (Vector2.Distance(transform.position, _intialPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _intialPosition,
                Time.deltaTime * 2 * chargeSpeed
            );
            yield return null;
        }
    }

    enum State
    {
        Inactive,
        Preparing,
        Charging,
    }
}