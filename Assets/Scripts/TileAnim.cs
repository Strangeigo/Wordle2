using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileAnim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Shake()
    {
        transform.DOShakePosition(0.5f, 1.5f, 10, 70, false, true);
        print("shaking");
        transform.DOShakeRotation(

            duration: 0.5f,
             strength: new Vector3(0, 0, 30), // rotation shake strength on each axis (e.g. Z axis)
             vibrato: 10,                     // how much it vibrates
             randomness: 90,                  // randomness factor (angle noise)
             fadeOut: true                    // smoothly decrease shake
             );
    }

    public void IsWrong()
    {
        transform.DOPunchRotation(
            punch: new Vector3(0, 0, 20), // Rotate a bit around Z axis (left-right shake)
            duration: 0.3f,
            vibrato: 1,                   // Number of shakes (set to 1 for a single punch)
            elasticity: 0.5f              // How much it rebounds (0 = no bounce back, 1 = full bounce)
        );

        Debug.Log("ISWRONG");
    }

    public void ValidWord()
    {
        transform.DORotate(
            new Vector3(0, 0, 360),     // Rotation target
            .8f,                       // Duration in seconds
            RotateMode.FastBeyond360   // Allows full spin, not clamped to shortest path
        );
    }
    public void InvalidWord()
    {

    }
}
