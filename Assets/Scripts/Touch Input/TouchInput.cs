using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
   public enum ConnectionType
    {
        Mobile,
        Windows,
        Both
    }

    public ConnectionType connectionType = ConnectionType.Mobile;
    //The layer we can interact with
    public LayerMask touchLayer;
    //Current touches
    private List<GameObject> _touchList = new List<GameObject>();
    //Old Touches
    private GameObject[] _oldTouches;
    //Touch Info
    private RaycastHit _hit;






    private void Update()
    {
        if (connectionType == ConnectionType.Mobile)
        {
            Mobile();
        }
        else if (connectionType == ConnectionType.Windows)
        {
            Windows();
        }
        else
        {
            Mobile();
            Windows();
        }

    }

    private void Mobile()
    {
        if (Input.touchCount > 0)
        {
            //Set the old touches array to the size of our current touch list.
            _oldTouches = new GameObject[_touchList.Count];
            //Send the data from our current touch list to the array container for old touches
            _touchList.CopyTo(_oldTouches);
            //Clear the current touch list
            _touchList.Clear();

            foreach (Touch touch in Input.touches)
            {
                //Create a ray from the mouse position according to where it is on the screen
                Ray ray = GetComponent<Camera>().ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out _hit, touchLayer))
                {
                    //hold onto the touchable object
                    GameObject recipient = _hit.transform.gameObject;
                    //Add it to the list of things that got touched.
                    _touchList.Add(recipient);

                    //Equiv to 'MouseButtonDown' for mobile devices
                    if (touch.phase == TouchPhase.Began)
                    {
                        recipient.SendMessage("OnTouchDown", _hit.point, SendMessageOptions.DontRequireReceiver);
                        Debug.Log("OnTouchDown");
                    }
                    //Equiv to 'MouseButton' for mobile devices
                    if (touch.phase == TouchPhase.Stationary)
                    {
                        recipient.SendMessage("OnTouchStay", _hit.point, SendMessageOptions.DontRequireReceiver);
                        Debug.Log("OnTouchStay");
                    }
                    //Equiv to 'MouseButtonUp' for mobile devices
                    if (touch.phase == TouchPhase.Ended)
                    {
                        recipient.SendMessage("OnTouchUp", _hit.point, SendMessageOptions.DontRequireReceiver);
                        Debug.Log("OnTouchUp");
                    }
                    //Equiv to 'OnTouchStay' for mobile devices
                    if (touch.phase == TouchPhase.Canceled)
                    {
                        recipient.SendMessage("OnTouchExit", _hit.point, SendMessageOptions.DontRequireReceiver);
                        Debug.Log("OnTouchExit");
                    }
                }
            }
            foreach (GameObject item in _oldTouches)
            {
                if (!_touchList.Contains(item))
                {
                    item.SendMessage("OnTouchExit", _hit.point, SendMessageOptions.DontRequireReceiver);
                    Debug.Log("OnTouchExit");
                }
            }
        }
    }

    private void Windows()
    {
        //When we press down somewhere new
        if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0) || Input.GetMouseButtonDown(0)) // In theory you should just be able to get 'mouse button' but SOMETIMES it can be funky, so this checks for all input.
        {
            //Set the old touches array to the size of our current touch list.
            _oldTouches = new GameObject[_touchList.Count];
            //Send the data from our current touch list to the array container for old touches
            _touchList.CopyTo( _oldTouches);
            //Clear the current touch list
            _touchList.Clear();

            //Create a ray from the mouse position according to where it is on the screen
            Ray ray = GetComponent<Camera>().ScreenPointToRay( Input.mousePosition );

            //If this ray hits something on the touch layer
            if(Physics.Raycast( ray, out _hit, touchLayer ))
            {
                //hold onto the touchable object
                GameObject recipient = _hit.transform.gameObject;
                //Add it to the list of things that got touched.
                _touchList.Add( recipient );


                
                if (Input.GetMouseButtonDown(0))
                {
                    //This message that we're sending out doesn't require receiver means that if the script of the object we hit doesn't have this on it,
                    //we're not going to get an error back because we don't require it.
                    //Whereas if you needed that one object to have the script and it does require that information then you need RequireReceiver
                    //So doing it this way, if you've accidentally set something in your layer to have Touchable stuff, but it's got like OnTouchUp and it doesn't have OnTouchDown.
                    //It's not going to Chuck his event, You're not gonna get a null reference.Not gonna get any errors because we're we've got does not require receiver.
                    recipient.SendMessage("OnTouchDown", _hit.point,SendMessageOptions.DontRequireReceiver);
                    Debug.Log("OnTouchDown");
                }
                if (Input.GetMouseButton(0))
                {
                   
                    recipient.SendMessage("OnTouchStay", _hit.point, SendMessageOptions.DontRequireReceiver);
                    Debug.Log("OnTouchStay");
                }
                if (Input.GetMouseButtonUp(0))
                {

                    recipient.SendMessage("OnTouchUp", _hit.point, SendMessageOptions.DontRequireReceiver);
                    Debug.Log("OnTouchUp");
                }


            }
            foreach (GameObject item in _oldTouches)
            {
                if (!_touchList.Contains(item))
                {
                    item.SendMessage("OnTouchExit", _hit.point, SendMessageOptions.DontRequireReceiver);
                    Debug.Log("OnTouchExit");
                }
            }
        }
    }


}
