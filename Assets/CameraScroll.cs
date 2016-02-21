#pragma strict

using UnityEngine;
using System.Collections;

public class CameraScroll : MonoBehaviour {
	
	public MainScript mainScript;
	
	private Color highlightColor = new Color(1f, 0.3f, 0.3f);
	
	private int screenWidth;
	private int screenHeight;
	private static int scrollMargin = 50;
	private static int scrollSpeed = 6;
	
	// Use this for initialization
	void Start () {
		screenWidth = Screen.width;
		screenHeight = Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector2 mousePosition = Input.mousePosition;
		
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out hit)) {
			//Debug.Log("We hit something!");
			//hit.transform.GetChild(0).renderer.material.color = highlightColor;
			
			if (Input.GetMouseButtonUp(0) && hit.transform.CompareTag("Terrain")) {
				// user clicked on a terrain tile
				mainScript.placeBlock(hit);
			}
		}
		
		if (mousePosition.x < scrollMargin) {
			transform.Translate(Vector3.left * Time.deltaTime * scrollSpeed, Space.World);
		} else if (mousePosition.x > screenWidth - scrollMargin) {
			transform.Translate(Vector3.right * Time.deltaTime * scrollSpeed, Space.World);
		}
		
		if (mousePosition.y < scrollMargin) {
			transform.Translate(Vector3.back * Time.deltaTime * scrollSpeed, Space.World);
		} else if (mousePosition.y > screenHeight - scrollMargin) {
			transform.Translate(Vector3.forward * Time.deltaTime * scrollSpeed, Space.World);
		}
		
	}
}
