using UnityEngine.InputSystem;
using UnityEngine;

namespace HPmdk
{
    internal class Freecam
    {
        private bool isEnabled = false;
        private bool rotationEnabled = false;
        private readonly Camera camera = null;
        private float speed = 2.3f;
        private float speedr = 60f;
        private Vector3 pos;
        private Quaternion rot;

        public Freecam(Camera camera)
        {
            this.camera = camera;
        }

        public Freecam(Camera camera, bool isEnabled)
        {
            this.camera = camera;
            this.isEnabled = isEnabled;
        }

        public Freecam(Camera camera, float speed, float speedr)
        {
            this.camera = camera;
            this.speed = speed;
            this.speedr = speedr;
        }

        public Freecam(Camera camera, bool isEnabled, float speed, float speedr)
        {
            this.camera = camera;
            this.isEnabled = isEnabled;
            this.speed = speed;
            this.speedr = speedr;
        }

        public Freecam(Camera camera, bool isEnabled, bool rotationEnabled)
        {
            this.camera = camera;
            this.isEnabled = isEnabled;
            this.rotationEnabled = rotationEnabled;
        }

        public Freecam(Camera camera, float speed, float speedr, bool rotationEnabled)
        {
            this.camera = camera;
            this.speed = speed;
            this.speedr = speedr;
            this.rotationEnabled = rotationEnabled;
        }

        public Freecam(Camera camera, bool isEnabled, float speed, float speedr, bool rotationEnabled)
        {
            this.camera = camera;
            this.isEnabled = isEnabled;
            this.speed = speed;
            this.speedr = speedr;
            this.rotationEnabled = rotationEnabled;
        }

        public void Update()
        {
            if (isEnabled)
            {
                if (Keyboard.current[Key.W].isPressed)
                {
                    camera.transform.position += camera.transform.forward * speed * Time.deltaTime;
                }
                else if (Keyboard.current[Key.S].isPressed)
                {
                    camera.transform.position -= camera.transform.forward * speed * Time.deltaTime;
                }
                if (Keyboard.current[Key.A].isPressed)
                {
                    camera.transform.position -= camera.transform.right * speed * Time.deltaTime;
                }
                else if (Keyboard.current[Key.D].isPressed)
                {
                    camera.transform.position += camera.transform.right * speed * Time.deltaTime;
                }
                if (Keyboard.current[Key.LeftShift].isPressed)
                {
                    camera.transform.position += camera.transform.up * speed * Time.deltaTime;
                }
                else if (Keyboard.current[Key.LeftCtrl].isPressed)
                {
                    camera.transform.position -= camera.transform.up * speed * Time.deltaTime;
                }
                if (rotationEnabled)
                {
                    if (Keyboard.current[Key.E].isPressed) //right
                    {
                        camera.transform.Rotate(Vector3.up * speedr * Time.deltaTime);
                    }
                    else if (Keyboard.current[Key.Q].isPressed) //left
                    {
                        camera.transform.Rotate(Vector3.down * speedr * Time.deltaTime);
                    }
                    if (Keyboard.current[Key.C].isPressed) //down
                    {
                        camera.transform.Rotate(Vector3.right * speedr * Time.deltaTime);
                    }
                    else if (Keyboard.current[Key.R].isPressed) //up
                    {
                        camera.transform.Rotate(Vector3.left * speedr * Time.deltaTime);
                    }
                }
            }
        }

        public void SetEnabled()
        {
            pos = camera.transform.position;
            rot = camera.transform.rotation;
            isEnabled = true;
        }

        public void SetDisabled()
        {
            camera.transform.position = pos;
            camera.transform.rotation = rot;
            isEnabled = false;
        }

        public bool Enabled()
        {
            return isEnabled;
        }

        public void SetRotationEnabled()
        {
            rotationEnabled = true;
        }

        public void SetRotationDisabled()
        {
            rotationEnabled = false;
        }

        public bool RotationEnabled()
        {
            return rotationEnabled;
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }

        public void SetSpeedr(float speedr)
        {
            this.speedr = speedr;
        }
    }
}
