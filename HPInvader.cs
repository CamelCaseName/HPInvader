using Il2CppSystem.Reflection;
using MelonLoader;
using System.Threading.Tasks;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project
{
    public class HPInvader : MelonMod
    {
        private Scene scene = SceneManager.GetActiveScene();
        private Vector3 vec = new Vector3();
        private static readonly Vector3 offset = new Vector3(0, 1.943712f, -0.2156f);
        private static readonly Vector3 mainMenuPos = new Vector3(0, 0.635f, -10);
        private static readonly Quaternion mainMenuRot = new Quaternion(-0.04361941f, 0, 0, 0.9990483f);
        private Component camera = new Component();
        private Component playerRoot = new Component(); //wird nur bei warpto geupdated??? 
        private float speed = 2.3f;
        private float speedr = 60f;
        private bool inFreecam = false;
        private MethodInfo inputManagerSubscribeEvents;
        private MethodInfo inputManagerUnsubscribeEvents;
        private Il2CppSystem.Object inputManager;


        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            //ListMethodsOfGameObjects();
            InitializeGameObjects();
        }

        public override void OnUpdate()
        {
            //toggle freecam with alt+f
            if (Input.GetKeyDown(KeyCode.F) && Input.GetKey(KeyCode.LeftAlt))
            {
                if (camera != null)
                {
                    if (inFreecam)
                    {
                        inFreecam = false;

                        if (scene.name == "GameMain")
                        {
                            camera.transform.position = playerRoot.transform.position + offset;

                            inputManagerSubscribeEvents.Invoke(inputManager, new Il2CppReferenceArray<Il2CppSystem.Object>(new Il2CppSystem.Object[] { }));
                        }
                        else if (scene.name == "MainMenu")
                        {
                            camera.transform.position = mainMenuPos;
                            camera.transform.rotation = mainMenuRot;
                        }
                    }
                    else
                    {
                        inFreecam = true;
                        if (scene.name == "GameMain")
                        {
                            inputManagerUnsubscribeEvents.Invoke(inputManager, new Il2CppReferenceArray<Il2CppSystem.Object>(new Il2CppSystem.Object[] { }));
                        }
                    }
                }

            }
            else if (Input.GetKeyDown(KeyCode.M) && Input.GetKey(KeyCode.LeftAlt)) //list methods of gameobjects
            {
                ListMethodsOfGameObjects();
            }
            else if (Input.GetKeyDown(KeyCode.P) && Input.GetKey(KeyCode.LeftAlt)) //list properties of gameobjects
            {
                ListPropertiesOfGameObjects();
            }
            else if (Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.LeftAlt)) //list fields of gameobjects
            {
                ListFieldsOfGameObjects();
            }
            else if (Input.GetKeyDown(KeyCode.T) && Input.GetKey(KeyCode.LeftAlt)) //test gameobjects
            {
                _ = TestGameObjects();
            }

            //run freecam
            if (inFreecam)
            {
                Freecam();
            }
        }

        public void InitializeGameObjects()
        {
            scene = SceneManager.GetActiveScene();
            GameObject[] objects = scene.GetRootGameObjects();
            MelonLogger.Msg("Scene loaded: " + scene.name);
            foreach (GameObject item in objects)
            {
                Component[] comps = item.GetComponents(Il2CppType.Of<Component>());
                foreach (Component comp in comps)
                {
                    if (comp.ToString() == "Input Manager (Rewired.InputManager)")
                    {
                        Il2CppReferenceArray<MethodInfo> methodInfos = comp.GetIl2CppType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                        inputManager = comp.GetIl2CppType().TryCast<Il2CppSystem.Object>();
                        foreach (MethodInfo method in methodInfos)
                        {
                            //getting methods
                            if (method.Name == "OnInitialized")
                            {
                                inputManagerSubscribeEvents = method;
                            }
                            else if (method.Name == "ResetAll")
                            {
                                inputManagerUnsubscribeEvents = method;
                            }
                        }
                    }
                }
                if (item.name == "Camera")
                {
                    camera = item.GetComponent(Il2CppType.Of<Transform>());
                    MelonLogger.Msg("got " + camera.ToString());
                }
                else if (item.name == "PlayerMale Root")
                {
                    playerRoot = item.GetComponent(Il2CppType.Of<Transform>());
                    MelonLogger.Msg("got " + playerRoot.ToString());
                }
            }
        }

        public void ListMethodsOfGameObjects()
        {
            scene = SceneManager.GetActiveScene();
            GameObject[] objects = scene.GetRootGameObjects();
            MelonLogger.Msg("Current scene: " + scene.name);
            foreach (GameObject item in objects)
            {
                Component[] items = item.GetComponents(Il2CppType.Of<Component>());
                foreach (Component comp in items)
                {
                    MelonLogger.Msg(comp.ToString() + " from " + item.ToString());
                    if (comp.ToString() == "Input Manager (Rewired.Data.UserDataStore_PlayerPrefs)")
                    {
                        Il2CppReferenceArray<MethodInfo> methodInfos = comp.GetIl2CppType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                        Il2CppSystem.Object inputManager2 = comp.GetIl2CppType().TryCast<Il2CppSystem.Object>();
                        foreach (MethodInfo method in methodInfos)
                        {
                            MelonLogger.Msg("M:" + comp.name + "." + method.Name + "()");
                            /*
                            MelonLogger.Msg("Trying to run" + comp.name + "." + method.Name + "()");
                            Il2CppSystem.Object result = method.Invoke(inputManager2, new Il2CppReferenceArray<Il2CppSystem.Object>(new Il2CppSystem.Object[] { }));
                            if (result != null)
                            {
                                MelonLogger.Msg("Result: " + result.ToString());
                            }*/
                        }
                    }
                    else if (comp.GetIl2CppType() != Il2CppType.Of<Transform>())
                    {
                        Il2CppReferenceArray<MethodInfo> methodInfos = comp.GetIl2CppType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                        foreach (MethodInfo method in methodInfos)
                        {
                            MelonLogger.Msg("M:" + comp.name + "." + method.Name + "()");
                        }
                    }
                }
            }
        }

        //memo: can't set properties
        public void ListPropertiesOfGameObjects()
        {
            scene = SceneManager.GetActiveScene();
            GameObject[] objects = scene.GetRootGameObjects();
            MelonLogger.Msg("Scene loaded: " + scene.name);
            foreach (GameObject item in objects)
            {
                Component[] items = item.GetComponents(Il2CppType.Of<Component>());
                foreach (Component comp in items)
                {
                    MelonLogger.Msg(comp.ToString() + " from " + item.ToString());
                    Il2CppReferenceArray<PropertyInfo> propertyInfos = comp.GetIl2CppType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (PropertyInfo property in propertyInfos)
                    {
                        MelonLogger.Msg("P:" + comp.name + "." + property.Name);
                    }
                }
            }
        }

        //memo: can't set fields
        public void ListFieldsOfGameObjects()
        {
            scene = SceneManager.GetActiveScene();
            GameObject[] objects = scene.GetRootGameObjects();
            MelonLogger.Msg("Scene loaded: " + scene.name);
            foreach (GameObject item in objects)
            {
                Component[] items = item.GetComponents(Il2CppType.Of<Component>());
                foreach (Component comp in items)
                {
                    MelonLogger.Msg(comp.ToString() + " from " + item.ToString());
                    Il2CppReferenceArray<FieldInfo> fieldInfos = comp.GetIl2CppType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (FieldInfo property in fieldInfos)
                    {
                        MelonLogger.Msg("F:" + comp.name + "." + property.Name);
                    }
                }
            }
        }

        public async Task TestGameObjects()
        {
            scene = SceneManager.GetActiveScene();
            GameObject[] objects = scene.GetRootGameObjects();
            MelonLogger.Msg("temporarily disabling each gameobject in " + scene.name);
            foreach (GameObject item in objects)
            {
                if (item.name != "Camera" || item.name != "Interactive Items")
                {
                    item.SetActive(false);
                    MelonLogger.Msg("deactivated " + item.ToString());
                    await Task.Delay(5000);
                    item.SetActive(true);
                    MelonLogger.Msg("activated " + item.ToString());
                    await Task.Delay(5000);
                }
                else MelonLogger.Msg("can't reactivate " + item.name + ", so won't deactivate in the first place :)");
            }
        }

        public void Freecam()
        {
            if (Input.GetKey(KeyCode.W))
            {
                camera.transform.get_position_Injected(out vec);
                vec += camera.transform.forward * speed * Time.deltaTime;
                camera.transform.set_position_Injected(ref vec);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                camera.transform.get_position_Injected(out vec);
                vec -= camera.transform.forward * speed * Time.deltaTime;
                camera.transform.set_position_Injected(ref vec);
            }
            if (Input.GetKey(KeyCode.A))
            {
                camera.transform.get_position_Injected(out vec);
                vec -= camera.transform.right * speed * Time.deltaTime;
                camera.transform.set_position_Injected(ref vec);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                camera.transform.get_position_Injected(out vec);
                vec += camera.transform.right * speed * Time.deltaTime;
                camera.transform.set_position_Injected(ref vec);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                camera.transform.get_position_Injected(out vec);
                vec += camera.transform.up * speed * Time.deltaTime;
                camera.transform.set_position_Injected(ref vec);
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                camera.transform.get_position_Injected(out vec);
                vec -= camera.transform.up * speed * Time.deltaTime; ;
                camera.transform.set_position_Injected(ref vec);
            }
            if (Input.GetKey(KeyCode.E))
            {
                camera.transform.Rotate(Vector3.up * speedr * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                camera.transform.Rotate(Vector3.down * speedr * Time.deltaTime);
            }
        }
    }
}
