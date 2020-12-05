using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBehaviour : MonoBehaviour
{
    [SerializeField] private float _riseSpeed;
    [SerializeField] private float _MaxHeight;
    [SerializeField] private GameObject _Camera;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= _MaxHeight)
        {
            transform.Translate(new Vector3(0, _riseSpeed * Time.deltaTime, 0));
            _Camera.transform.Translate(new Vector3(0, _riseSpeed * Time.deltaTime, 0));
        }
    }
    private void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            Destroy(hit.gameObject);
        }
    }
}
