﻿using UnityEngine;
using System.Collections;

public class ExampleCharacterController : MonoBehaviour {
  private static CardboardInput cardboard;

  private Vector3 moveDirection = Vector3.zero;
  private CharacterController controller;

	void Start () {
    controller = GetComponent<CharacterController>();

    /*
    Start by declaring an instance of CardboardInput.
    This is a good point to pass your methods to its delegates.
    
    Unity provides a good primer on delegates here:
    http://unity3d.com/learn/tutorials/modules/intermediate/scripting/delegates
    */
    cardboard = new CardboardInput();

    cardboard.OnMagnetDown += CardboardDown;  // When the magnet goes down
    cardboard.OnMagnetUp += CardboardUp;      // When the magnet comes back up

    // When the magnet goes down and up within the "click threshold" time
    // That limit is public as cardboard.clickSpeedThreshold
    cardboard.OnMagnetClicked += CardboardClick;

    // Not shown here is the OnMagnetMoved delegate.
    // This is triggered when the magnet goes up or down.
	}



  /*
  In this demo, we change sphere colours for each event triggered.
  The CardboardEvent will eventually pass useful data related to the event
  but it's currently just a placeholder.
  */
  public void CardboardDown(object sender, CardboardEvent cardboardEvent) {
    ChangeSphereColor("SphereDown");
  }

  public void CardboardUp(object sender, CardboardEvent cardboardEvent) {
    ChangeSphereColor("SphereUp");
  }

  public void CardboardClick(object sender, CardboardEvent cardboardEvent) {
    ChangeSphereColor("SphereClick");

    TextMesh textMesh = GameObject.Find("SphereClick/Counter").GetComponent<TextMesh>();
    int increment = int.Parse(textMesh.text) + 1;
    textMesh.text = increment.ToString();
  }

  public void ChangeSphereColor(string name) {
    GameObject sphere = GameObject.Find(name);
    sphere.renderer.material.color = new Color(Random.value, Random.value, Random.value);
  }



  /*
  During our game we can utilize data from CardboardInput.
  */
  void Update() {
    TextMesh textMesh = GameObject.Find("SphereDown/Counter").GetComponent<TextMesh>();

    // Be sure to update CardboardInput during your cycle
    cardboard.Update();

    // IsMagnetHeld is true when the magnet has gone down but not back up yet.    
    if (!cardboard.IsMagnetHeld() ) {
      textMesh.renderer.enabled = Time.time % 1 < 0.5;
    }
    else {
      textMesh.renderer.enabled = true;

      // SecondsMagnetHeld is the number of seconds we've held the magnet down.
      // It stops when when the magnet goes up and resets when the magnet goes down.
      textMesh.text = cardboard.SecondsMagnetHeld().ToString("#.##");
    }

    // Not shown here is the WasClicked method.
    // This tells you if magnet was clicked
    // since the last time the method was called.
  }
}
