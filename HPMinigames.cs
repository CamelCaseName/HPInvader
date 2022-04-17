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
using Il2CppSystem.Collections.Generic;

namespace HPInvader
{
    public class HPMinigames : MelonMod
    {
        private Scene scene = SceneManager.GetActiveScene();
        public static bool IsLoading = false;
        public static bool inMainMenu = false;
        public static bool inGameMain = false;
        private bool inMinigame = false;
        private Freecam freecam;
        private HousePartyPlayerCharacter player = null;
        public Rect windowRect = new Rect(20, 20, 400, 200);
        private readonly float speed = 2f;
        private readonly float sideSpeed = 0.5f;
        private readonly float forwardSpeed = 0.05f;
        AmyCharacter amy;
        float amyTimer = 1;
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
                    MinigameFaceInvader();
                }
            }
        }

        public void InitializeGameObjects()
        {
            scene = SceneManager.GetActiveScene();
            MelonLogger.Msg("Scene loaded: " + scene.name);
            inMainMenu = scene.name == "MainMenu";
            inGameMain = scene.name == "GameMain";
            IsLoading = scene.name == "LoadingScreen";
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
                //amy._activePositionOverride = false;
                amy.Mecanim.OGBKIDBHJDB(); //end pose
                amy.Clothes.ACMHNFDKNLL(EHFOEJADICB.Shoes, true, 0);
                amy.Clothes.ACMHNFDKNLL(EHFOEJADICB.Bottom, true, 0);
                amy.Clothes.ACMHNFDKNLL(EHFOEJADICB.Top, true, 0);
                amy.Mecanim._animationEventQueueTimer = amyTimer;
            }
            player.Incapacitated = false;
            player.IsImmobile = false;
            //player._activePositionOverride = false;
            player.Intimacy.LockPenis = false;
            //player.Intimacy._penisDB.enabled = true;
            player.Clothes.ACMHNFDKNLL(EHFOEJADICB.Shoes, true, 0); //set clothing item
            player.Clothes.ACMHNFDKNLL(EHFOEJADICB.Bottom, true, 0);
            player.Clothes.ACMHNFDKNLL(EHFOEJADICB.Top, true, 0);
            //GameManager.ILMGIKEOAPM().UnPause();//get current game manager
        }

        private void MinigameInit()
        {
            amy = Object.FindObjectOfType<AmyCharacter>();
            if (amy != null)
            {
                amy.Mecanim.HPCPOABEPPO(10); //set pose 10, blowjobready
                amy.eventQueue.Clear();
                //amy.SetPositionOverride(new Vector3(0.5f, 0.3f, -6));
                amy.transform.position = new Vector3(0.5f, 0.3f, -6);
                amy.IsImmobile = true;
                amy.Clothes.AMODMPBOFOK(false, 0);
                amyTimer = amy.Mecanim._animationEventQueueTimer;
                amy.Mecanim._jawOpenAmount = 10;
                amy.Mecanim._animationEventQueueTimer = 0;
            }
            player.Incapacitated = true;
            player.IsImmobile = true;
            player.SetPositionOverride(new Vector3(0.5f, 0.3f, -5));//todo add random offset when starting the minigame
            //player.FPInput.AllowCrouch = false;
            //player.FPInput.Enabled = false;
            //player.transform.position = new Vector3(0.5f, 0.3f, -5);
            player.Clothes.AMODMPBOFOK(false, 0); //set all clothing
            //else do targeting by myselfusing _penisBone and _penisDB (dynamic bone)
            //player.Intimacy._currentPenisAngle = 90;
            //player.Intimacy.iHelper.BOCNBFJCMKI(OGHGPAGLEBD.Masturbating, player);//start sex event, masturbating. works for the dick but i need the hand to not be there...
            //player.Intimacy.FIBHPFAHJDI(1);//release left hand
            player.Intimacy.BlowJobLookTarget = amy.gameObject;
            //player.Intimacy.AMCJJPGDNIE(OGHGPAGLEBD.BlowJob, amy, LKLCNNABKHF.Primary); //initiate sex event
            player.Intimacy.LockPenis = true;
            player._myColliders.Clear();
            player.Motion.ApplyGravity = false;
            player.FPInput.AllowFlying = true;
            amy.Motion.ApplyGravity = false;
            //amy.breast
            //todo disable gravity/floor ik for the player and amy
            //remove all colliders from the player

            //GameManager.ILMGIKEOAPM().Pause();
        }

        private void MinigameFaceInvader()
        {
            player.Intimacy.FIBHPFAHJDI(1);
            amy = Object.FindObjectOfType<AmyCharacter>();
            if (amy != null)
            {
                amy.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            player.transform.rotation = Quaternion.Euler(0, 180, 0);
            player.Intimacy._penisBone.rotation = Quaternion.Euler(0, 180, 0);//move dick to the right orientation
            player.Intimacy._penisDB.m_Stiffness = 1000; //make the penis more stiff
            player.Intimacy._penisDB.m_Force += new Vector3(0, 0, -10);//add forward dick force so we elongate the cock
            player.Intimacy._penisDB.m_Elasticity = 0; //make it less elastic
            player.Intimacy._penisDB.m_FreezeAxis = DynamicBone.IIJOFOGIFHD.Y; //freeze zhe y axis
            player.Intimacy._penisDB.m_Gravity = Vector3.zero; //remove gravity so it stays up
            player.Intimacy._penisDB.m_Inert = 100;
            amy.BlendShapes.EmoteList.Clear();
            amy.BlendShapes.PCHBGDPOPMB(true);//reset blend shapes (important ones true)
            //player.BlendShapes.EmoteList.Clear();
            //player.BlendShapes.PCHBGDPOPMB(true);

            amy.BlendShapes.BPNJFBDBHDD(IKHHCHDDNCI.OpenMouth, true, 1); //blendshapes emote (emote, important, emotestr??)
            //player.BlendShapes.BPNJFBDBHDD(IKHHCHDDNCI.Erection, true, 1);

            player.BlendShapes.FADECPNLCAE();//set penis erect

            camera.transform.position = player.puppetHip.position + player.puppetHip.forward * 0.127f - player.puppetHip.up * 0.08f;

            //move player forward with constant time
            player._positionOverride += player.transform.forward * forwardSpeed * Time.deltaTime;

            if (Keyboard.current[Key.LeftArrow].isPressed)
            {
                player._positionOverride -= player.transform.right * sideSpeed * Time.deltaTime;
            }
            else if (Keyboard.current[Key.RightArrow].isPressed)
            {
                player._positionOverride += player.transform.right * sideSpeed * Time.deltaTime;
            }
            if (Keyboard.current[Key.UpArrow].isPressed)
            {
                player._positionOverride += player.transform.up * sideSpeed * Time.deltaTime;
            }
            else if (Keyboard.current[Key.DownArrow].isPressed)
            {
                player._positionOverride -= player.transform.up * sideSpeed * Time.deltaTime;
            }
        }

        public void Test()
        {
            amy = Object.FindObjectsOfType<AmyCharacter>()[0];

            int d = 0;
            foreach (int state in amy.LKIFNHMMHAK)
            {
                MelonLogger.Msg($"at pos {d}: {state}");
                d++;
            }
        }
    }
}
