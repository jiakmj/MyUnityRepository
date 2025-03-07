using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class LerpSample : MonoBehaviour
{
    public Transform A;
    public Transform B;

    [Range(0f, 1f)]
    public float T = 0;

    float elapsedTime = 0;
    float sign = 1;

    public Color AColor;
    public Color BColor;

    public AnimationCurve curve;

    void Update()
    {
        elapsedTime =+ sign * Time.deltaTime;

        if (elapsedTime > 1 || elapsedTime < 0)
        {
            sign = sign * -1;
        }
        transform.position = Vector3.Lerp(A.position, B.position, curve.Evaluate(elapsedTime));
        transform.rotation = Quaternion.Lerp(A.rotation, B.rotation, curve.Evaluate(elapsedTime));

        GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.Lerp(AColor, BColor, curve.Evaluate(elapsedTime)));
        
    }
}
