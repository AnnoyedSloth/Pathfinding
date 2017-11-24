using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coroutine : MonoBehaviour {
    Renderer renderer;

    public float steps;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space)) StartCoroutine(Fade());

	}

    IEnumerator Fade()
    {
        for (float i = 1.0f; i > 0; i -= steps)
        {
            print(i);
            Color c = renderer.material.color;
            c.a = i;
            renderer.material.color = c;
            //yield return new WaitForSeconds(0.01f);
            yield return null;
        }
        //yield break;
    }
}
