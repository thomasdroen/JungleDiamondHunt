using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pcTester : MonoBehaviour
{
    private RectTransform rT;

    void Start()
    {
        rT = GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		Debug.Log(transform.localPosition + "transform vs rectTransform:" + rT.transform.localPosition);
	}
}
