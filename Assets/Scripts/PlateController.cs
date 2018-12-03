
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlateController : MonoBehaviour
{
    public List<Transform> doors;
    public List<float> yOffset;
    public float timeToShift = 0.3f;
    float velocity = 0.0f;

    private bool _doorOpening = false;
    private List<Transform> _trans;
    private List<float> _startY;
    private int _numberOfUnitsOnPlate = 0;


    // Use this for initialization
    void Start()
    {
        _trans = new List<Transform>(doors.Select(x => x.transform).ToList());
        _startY = new List<float>(doors.Select(x => x.transform.position.y).ToList());
    }

    // Update is called once per frame
    void Update()
    {
        openOrCloseDoor(_doorOpening);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ENTERED");
        _numberOfUnitsOnPlate++;
        Debug.Log("NUMBER OF " + _numberOfUnitsOnPlate);
        _doorOpening = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("EXIT PLATE");
        _numberOfUnitsOnPlate--;
        Debug.Log("NUMBER OF " + _numberOfUnitsOnPlate);
        if (_numberOfUnitsOnPlate == 0)
        {
            _doorOpening = false;
        }
    }

    void openOrCloseDoor(bool opening)
    {
        for (int i = 0; i < doors.Count(); i++)
        {
            float newPositionY = opening ? _startY[i] + yOffset[i] : _startY[i];

            _trans[i].position = new Vector3(
                _trans[i].position.x,
                Mathf.SmoothDamp(_trans[i].position.y, newPositionY, ref velocity, timeToShift),
                _trans[i].position.z
            );
        }
    }
}