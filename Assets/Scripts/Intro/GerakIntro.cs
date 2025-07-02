using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerakIntro : MonoBehaviour
{
    int[] posX = new int[] { -140, -120 };
    int idx = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (idx < posX.Length - 1)
            {
                idx++;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (idx > 0)
            {
                idx--;
            }
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector3(posX[idx], transform.position.y, transform.position.z),
            50 * Time.deltaTime // Speed disesuaikan agar terlihat transisinya
        );
    }
}
