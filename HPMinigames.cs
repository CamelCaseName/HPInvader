using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using EekAddOns;
using UnhollowerRuntimeLib;
using EekCharacterEngine.Interaction;
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
        private HousePartyPlayerCharacter player = null;
        public Rect windowRect = new Rect(20, 20, 400, 200);
        private readonly float sideSpeed = 0.5f;
        private readonly float forwardSpeed = 0.05f;
        private readonly Vector3 startPos = new Vector3(48.7f, -28.9f, 2);
        AmyCharacter amy;
        float amyTimer = 1;
        Camera camera;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            camera = Object.FindObjectOfType<Camera>();
            InitializeGameObjects();
        }

        public override void OnUpdate()
        {
            if (Keyboard.current[Key.U].wasPressedThisFrame && Keyboard.current[Key.LeftAlt].isPressed)
            {
                //toggle in minigame
                inMinigame = !inMinigame;
                if (inMinigame)
                {
                    FaceInvaderInit();
                }
                else
                {
                    ResetFaceInvader();
                }
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

        private void ResetFaceInvader()
        {
            if (amy != null)
            {
                amy.IsImmobile = false;
                amy._activePositionOverride = false;
                amy.Mecanim.OGBKIDBHJDB(); //end pose
                amy.Clothes.ACMHNFDKNLL(EHFOEJADICB.Shoes, true, 0);
                amy.Clothes.ACMHNFDKNLL(EHFOEJADICB.Bottom, true, 0);
                amy.Clothes.ACMHNFDKNLL(EHFOEJADICB.Top, true, 0);
                amy.Mecanim._animationEventQueueTimer = amyTimer;
            }
            player.Incapacitated = false;
            player.IsImmobile = false;
            player._activePositionOverride = false;
            player.Intimacy.LockPenis = false;
            player.Intimacy.CustomPenisScaleMultiplier = 1f;
            player.Clothes.ACMHNFDKNLL(EHFOEJADICB.Shoes, true, 0); //set clothing item
            player.Clothes.ACMHNFDKNLL(EHFOEJADICB.Bottom, true, 0);
            player.Clothes.ACMHNFDKNLL(EHFOEJADICB.Top, true, 0);
        }

        private void FaceInvaderInit()
        {
            amy = Object.FindObjectOfType<AmyCharacter>();
            if (amy != null)
            {
                amy.Mecanim.HPCPOABEPPO(10); //set pose 10, blowjobready
                amy.eventQueue.Clear();
                amy.SetPositionOverride(startPos);
                amy.IsImmobile = true;
                amy.Clothes.AMODMPBOFOK(false, 0);
                amyTimer = amy.Mecanim._animationEventQueueTimer;
                amy.Mecanim._jawOpenAmount = 10;
                amy.Mecanim._animationEventQueueTimer = 0;
            }
            player.Incapacitated = true;
            player.IsImmobile = true;
            player.SetPositionOverride(startPos + new Vector3(Random.RandomRange(-0.5f, 0.5f), Random.RandomRange(-0.5f, 0.5f), Random.RandomRange(-0.5f, 0.5f) + 2));//start position 2 units in front of amy, plus random offset
            player.FPInput.AllowCrouch = false;
            player.Clothes.AMODMPBOFOK(false, 0); //set all clothing
            player.Intimacy.BlowJobLookTarget = amy.gameObject;
            player.Intimacy.LockPenis = true;

            player.Intimacy.CustomPenisScaleMultiplier = 1.1f;

            Physics.gravity = Vector3.zero;

            //ground layer is 8, walls are 17
            /*
            foreach (var coll in Object.FindObjectsOfType<Collider>())
            {
                coll.enabled = false;
            }
            */
            //things that did not work:
            //#############################
            //player.PuppetMaster.FBPMBCBPMFM(10000);//deactivate pupet for 10000 seconds
            //player.FPInput.Enabled = false;
            //player.Intimacy._currentPenisAngle = 90;
            //player.Intimacy.iHelper.BOCNBFJCMKI(OGHGPAGLEBD.Masturbating, player);//start sex event, masturbating. works for the dick but i need the hand to not be there...
            //player.Intimacy.FIBHPFAHJDI(1);//release left hand
            //player.Intimacy.AMCJJPGDNIE(OGHGPAGLEBD.BlowJob, amy, LKLCNNABKHF.Primary); //initiate sex event (works, but doesnt help)
            //player.BlendShapes.EmoteList.Clear();//clear list of current emotes
            //player.BlendShapes.PCHBGDPOPMB(true);//clear all remotes, also important ones
            //player.BlendShapes.BPNJFBDBHDD(IKHHCHDDNCI.Erection, true, 1);//erection emote
            //player.BlendShapes.FADECPNLCAE();//set penis erect

            //amy.BlendShapes.EmoteList.Clear();
            //#############################

            //amy.breast

            foreach (var item in amy.BlendShapes.EmoteList)
            {
                MelonLogger.Msg($"Emote in Amy's EmoteList: id {item.Id} | blendshapeId {item.BlendShapeId} | is full face? {item.FullFace}");
            }
            foreach (var item in player.BlendShapes.EmoteList)
            {
                MelonLogger.Msg($"Emote in Players's EmoteList: id {item.Id} | blendshapeId {item.BlendShapeId} | is full face? {item.FullFace}");
            }

            //doesnt work :(
            amy.BlendShapes.PCHBGDPOPMB(true);//reset blend shapes (important ones true)
            amy.BlendShapes.PPBOLAJOLMP(true);//reset talking blend shapes (important ones true)
            amy.BlendShapes.currentTargetImportance = AEOLDKJIEHJ.None;
            amy.BlendShapes.Target = player.gameObject;
            amy.BlendShapes.currentTargetImportance = AEOLDKJIEHJ.VeryImportant;
            amy.BlendShapes._currentImportantEmotes.Clear();
            amy.BlendShapes._currentImportantEmotes.Add(amy.BlendShapes.EmoteList[8]);
            amy.BlendShapes.BPNJFBDBHDD(IKHHCHDDNCI.OpenMouth, true, 8); //blendshapes emote (emote, important, emotestr??)

        }

        private void MinigameFaceInvader()
        {
            amy.transform.rotation = Quaternion.Euler(0, 0, 0);

            player.transform.rotation = Quaternion.Euler(0, 180, 0);
            player.Intimacy._penisBone.rotation = Quaternion.Euler(0, 180, 0);//move dick to the right orientation
            player.Intimacy._penisDB.m_Stiffness = 1000; //make the penis more stiff
            player.Intimacy._penisDB.m_Force += new Vector3(0, 0, -10);//add forward dick force so we elongate the cock
            player.Intimacy._penisDB.m_Elasticity = 0; //make it less elastic
            player.Intimacy._penisDB.m_FreezeAxis = DynamicBone.IIJOFOGIFHD.Y; //freeze zhe y axis
            player.Intimacy._penisDB.m_Gravity = Vector3.zero; //remove gravity so it stays up
            player.Intimacy._penisDB.m_Inert = 100;

            camera.transform.position = player.Intimacy._penisBone.position + player.Intimacy._penisBone.up * 0.07f + player.Intimacy._penisBone.forward * 0.02f;

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

            amy.BlendShapes.BPNJFBDBHDD(IKHHCHDDNCI.OpenMouth, true, 8); //blendshapes emote (emote, important, emotestr??)
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
