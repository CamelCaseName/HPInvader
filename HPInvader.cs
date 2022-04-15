using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using EekAddOns;
using UnhollowerRuntimeLib;
using EekCharacterEngine.Interaction;
using HPmdk;
using HouseParty;
using EekCharacterEngine;
using EekEvents;

namespace HPInvader
{
    public class HPInvader : MelonMod
    {
        private Scene scene = SceneManager.GetActiveScene();
        public static bool IsLoading = false;
        public static bool inMainMenu = false;
        public static bool inGameMain = false;
        private bool inMinigame = false;
        private Freecam freecam;
        private HousePartyPlayerCharacter player = null;
        public Rect windowRect = new Rect(20, 20, 400, 200);
        private float speed = 2f;
        AmyCharacter amy;
        Camera camera;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            freecam = new Freecam(Object.FindObjectOfType<Camera>());
            camera = Object.FindObjectOfType<Camera>();
            InitializeGameObjects();
        }

        public override void OnUpdate()
        {
            //toggle freecam with alt+f
            if (Keyboard.current[Key.F].wasPressedThisFrame && Keyboard.current[Key.LeftAlt].isPressed)
            {
                if (freecam.Enabled())
                {
                    freecam.SetDisabled();
                    freecam.SetRotationDisabled();
                    if (inGameMain)
                    {
                        player.Incapacitated = false;
                        player.IsImmobile = false;
                        Camera temp_camera = Object.FindObjectOfType<Camera>();
                        if ((temp_camera.transform.position - player.Head.position + new Vector3(0, 0.1f, 0) + Vector3.Scale(player.Head.forward, new Vector3(0.2f, 0.2f, 0.2f))).magnitude >= 0.3)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                temp_camera.transform.position = player.Head.position + new Vector3(0, 0.1f, 0) + Vector3.Scale(player.Head.forward, new Vector3(0.2f, 0.2f, 0.2f));
                            }
                        }
                    }
                }
                else
                {
                    if (inGameMain)
                    {
                        player.Incapacitated = true;
                        player.IsImmobile = true;
                    }
                    else
                    {
                        freecam.SetRotationEnabled();
                    }
                    freecam.SetEnabled();
                }

            }
            else if (Keyboard.current[Key.U].wasPressedThisFrame && Keyboard.current[Key.LeftAlt].isPressed)
            {
                //toggle in minigame
                inMinigame = !inMinigame;
                if (inMinigame)
                {
                    amy = Object.FindObjectOfType<AmyCharacter>();
                    if (amy != null)
                    {
                        amy.Mecanim.HPCPOABEPPO(10);
                    }
                    MinigameInit();
                }
                else
                {
                    ResetMinigame();
                }
            }

            //run freecam
            if (freecam.Enabled())
            {
                freecam.Update();
            }
            if (inGameMain)
            {
                if (inMinigame)
                {
                    Minigame();
                }
            }
        }

        public void InitializeGameObjects()
        {
            scene = SceneManager.GetActiveScene();
            GameObject playerObj = null;
            MelonLogger.Msg("Scene loaded: " + scene.name);
            inMainMenu = scene.name == "MainMenu";
            inGameMain = scene.name == "GameMain";
            IsLoading = scene.name == "LoadingScreen";
            foreach (GameObject item in scene.GetRootGameObjects())
            {
                if (inGameMain)
                {
                    if (item.name == "PlayerMale Root")
                    {
                        playerObj = item;
                    }
                }
            }
            if (inGameMain)
            {
                player = Object.FindObjectOfType<HousePartyPlayerCharacter>();
                amy = Object.FindObjectOfType<AmyCharacter>();
            }
        }

        private void ResetMinigame()
        {
            if (amy != null)
            {
                amy.IsImmobile = false;
                amy._activePositionOverride = false;
                amy.Mecanim.OGBKIDBHJDB();
                amy.Clothes.ACMHNFDKNLL(EHFOEJADICB.Shoes, true, 0);
                amy.Clothes.ACMHNFDKNLL(EHFOEJADICB.Bottom, true, 0);
                amy.Clothes.ACMHNFDKNLL(EHFOEJADICB.Top, true, 0);
            }
            player.Incapacitated = false;
            player.IsImmobile = false;
            player._activePositionOverride = false;
            player.Clothes.ACMHNFDKNLL(EHFOEJADICB.Shoes, true, 0);
            player.Clothes.ACMHNFDKNLL(EHFOEJADICB.Bottom, true, 0);
            player.Clothes.ACMHNFDKNLL(EHFOEJADICB.Top, true, 0);
        }

        private void MinigameInit()
        {
            amy = Object.FindObjectOfType<AmyCharacter>();
            if (amy != null)
            {
                amy.SetPositionOverride(new Vector3(0.5f, 0.3f, -6));
                amy.IsImmobile = true;
                amy.Clothes.AMODMPBOFOK(false, 0);
            }
            player.Incapacitated = true;
            player.IsImmobile = true;
            player.SetPositionOverride(new Vector3(0.5f, 0.3f, -5));
            player.Clothes.AMODMPBOFOK(false, 0);
        }

        private void Minigame()
        {
            amy = Object.FindObjectOfType<AmyCharacter>();
            if (amy != null)
            {
                amy.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            player.transform.rotation = Quaternion.Euler(0, 180, 0);
            camera.transform.position = player.puppetHip.position + player.puppetHip.forward * 0.12f;

            if (Keyboard.current[Key.W].isPressed)
            {
                player._positionOverride += player.transform.forward * speed * Time.deltaTime;
            }
            else if (Keyboard.current[Key.S].isPressed)
            {
                player._positionOverride -= player.transform.forward * speed * Time.deltaTime;
            }
        }

        public void Test()
        {
            amy = Object.FindObjectsOfType<HouseParty.AmyCharacter>()[0];

            int d = 0;
            foreach (int state in amy.LKIFNHMMHAK)
            {
                MelonLogger.Msg($"at pos {d}: {state}");
                d++;
            }
        }
    }
}
