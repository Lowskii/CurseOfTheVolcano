using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBehaviour : MonoBehaviour
{
    [SerializeField] private float _riseSpeed;
    [SerializeField] private float _MaxHeight;
    [SerializeField] private float _riseSpeedMultiplier = 0.25f;

    float _ActiveRiseSpeed;

    private void Awake()
    {
        CharacterControl.PlayerDiedEvent.AddListener(IncreaseSpeed);

        _ActiveRiseSpeed = _riseSpeed;
    }


    void IncreaseSpeed()
    {
        _ActiveRiseSpeed += _riseSpeed * _riseSpeedMultiplier;
    }
    void Update()
    {
        if (transform.position.y <= _MaxHeight)
        {
            transform.Translate(new Vector3(0, _ActiveRiseSpeed * Time.deltaTime, 0));
        }
    }
    private void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            Destroy(hit.gameObject);


            CharacterControl.PlayerDied();

            //check if there is only one player left+
            if (CharacterControl.PlayersAlive == 1)
            {
                Application.Quit();
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }
    }
}
