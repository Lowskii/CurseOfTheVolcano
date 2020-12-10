using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBehaviour : MonoBehaviour
{
    [SerializeField] private  float _riseSpeed;
    [SerializeField] private  float _MaxHeight;
    [SerializeField] private  float _riseSpeedMultiplier = 0.25f;
    private SpawnBehaviour SpawnBeh;

    float _ActiveRiseSpeed;

    private void Awake()
    {
        SpawnBeh = FindObjectOfType<SpawnBehaviour>();

        if (SpawnBeh) SpawnBeh.PlayerDiedEvent.AddListener(IncreaseSpeed);


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

            if (SpawnBeh)
            {
                SpawnBeh.RemovePlayer();

                //check if there is only one player left+
                if (SpawnBeh.PlayersLeft == 1)
                {
                    Application.Quit();
                    UnityEditor.EditorApplication.isPlaying = false;
                }
            }
        }
    }
}
