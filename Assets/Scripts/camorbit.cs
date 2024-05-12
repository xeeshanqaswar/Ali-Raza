using UnityEngine;
using System.Collections;

public class camorbit : MonoBehaviour {

    public GameObject target;//the target object
    public float speedMod = 0.9f;//a speed modifier
    private Vector3 point;//the coord to the point where the camera looks at
    public float x, y = 0;
    public float cameraRotateSpeed = 5;
    public bool rotatio = false;
    public GameObject[] campos;
    bool rotateon;
    void Start() {//Set up things on the start method
        point = target.transform.position;//get target's coords
        transform.LookAt(point);//makes the camera look to it
        rotateon = true;
    }

    void Update() {

        if (rotateon)
        {
            //makes the camera rotate around "point" coords, rotating around its Y axis, 20 degrees per second times the speed modifier
            // transform.RotateAround (point,new Vector3(0.0f,1.0f,0.0f),20 * Time.deltaTime * speedMod);
            if (Input.GetMouseButton(0))
            {
                if (rotatio == true)
                {
                    x = Mathf.Lerp(x, Mathf.Clamp(Input.GetAxis("Mouse X"), -2, 2) * cameraRotateSpeed, Time.deltaTime * 5.0f);
                    Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 50, 60);
                    Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 50, Time.deltaTime);
                }
                else
                {
                    x = Mathf.Lerp(x, cameraRotateSpeed * 0.02f, Time.deltaTime * 10.0f);
                    Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, Time.deltaTime);
                }


            }
            else
            {
                x = Mathf.Lerp(x, cameraRotateSpeed * 0.02f, Time.deltaTime * 10.0f);
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, Time.deltaTime);
                //colorPicker.clickDown = false;
            }
            transform.RotateAround(point, Vector3.up, x);
        }
    }

        public void camchange (int no)
        {
        this.gameObject.transform.position = campos[no].transform.position;
        this.gameObject.transform.rotation = campos[no].transform.rotation;
        if (no == 0)
        {
            rotateon = true;
        }
        else
        {
            rotateon = false;
        }
    }
}
 