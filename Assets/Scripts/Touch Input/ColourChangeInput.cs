using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourChangeInput : MonoBehaviour
{
    private void OnTouchDown()
    {
        this.GetComponent<MeshRenderer>().material.color = Color.blue;
    }
    private void OnTouchStay()
    {
        this.GetComponent<MeshRenderer>().material.color = Color.green;
    }
    private void OnTouchUp()
    {
        this.GetComponent<MeshRenderer>().material.color = Color.white;
    }
    private void OnTouchExit()
    {
        this.GetComponent<MeshRenderer>().material.color = Color.yellow;
    }
   
}
