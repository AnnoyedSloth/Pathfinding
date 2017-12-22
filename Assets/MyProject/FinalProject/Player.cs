using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Player : MonoBehaviour {

    enum PlayerState { Idle, WalkForward, RunForward, LeftTurnRun, RightTurnRun , RaiseGun, DownGun, ShootGun};
    [SerializeField] PlayerState pState;
    Animator playerAni;
    CameraFollow myCamera;
    public Rigidbody bulletObj;
    public Transform bulletOutPos;
    System.Diagnostics.Stopwatch timer;

    float walkSpeed;
    float runSpeed;

    int hp;
    int bullet;


	// Use this for initialization
	void Start () {
        DataFromJson();
        myCamera = Camera.main.GetComponent<CameraFollow>();
        pState = PlayerState.Idle;
        playerAni = GetComponent<Animator>();
        walkSpeed = 1.0f;
        runSpeed = 2.0f;
        timer = new System.Diagnostics.Stopwatch();
	}
	
	// Update is called once per frame
	void Update () {
        if (myCamera.CheckPlayerMode())
        {
#region FSM Model
            switch (pState)
            {
                case PlayerState.Idle:
                    {
                        if (Input.GetKey(KeyCode.W))
                        {
                            //playerAni.SetBool("isIdle", false);
                            //playerAni.SetBool("isWalk", true);
                            //pState = PlayerState.WalkForward;
                            ChangeAnim("isIdle", "isWalk", PlayerState.WalkForward);
                        }
                        if (Input.GetKeyDown(KeyCode.R))
                        {
                            //StartCoroutine(CameraZoom(Camera.main.transform.position, Camera.main.transform.position + Vector3.right * 3));
                            ChangeAnim("isIdle", "isWeaponRaise", PlayerState.RaiseGun);
                        }
                        if (Input.GetKey(KeyCode.A))
                        {
                            transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * 50.0f);
                        }
                        if (Input.GetKey(KeyCode.D))
                        {
                            transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * 50.0f);
                        }
                    }
                    break;

                case PlayerState.WalkForward:
                    {
                        transform.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
                        if (Input.GetKeyUp(KeyCode.W))
                        {
                            //playerAni.SetBool("isWalk", false);
                            //playerAni.SetBool("isIdle", true);
                            //pState = PlayerState.Idle;
                            ChangeAnim("isWalk", "isIdle", PlayerState.Idle);
                        }
                        if (Input.GetKeyDown(KeyCode.LeftShift))
                        {
                            //playerAni.SetBool("isWalk", false);
                            //playerAni.SetBool("isRun", true);
                            //pState = PlayerState.RunForward;
                            ChangeAnim("isWalk", "isRun", PlayerState.RunForward);
                        }
                        if (Input.GetKeyDown(KeyCode.R))
                        {
                            ChangeAnim("isWalk", "isWeaponRaise", PlayerState.RaiseGun);
                        }
                        if (Input.GetKey(KeyCode.A))
                        {
                            transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * 50.0f);
                        }
                        if (Input.GetKey(KeyCode.D))
                        {
                            transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * 50.0f);
                        }
                    }
                    break;
                case PlayerState.RunForward:
                    {
                        transform.Translate(Vector3.forward * Time.deltaTime * runSpeed);
                        if (Input.GetKeyUp(KeyCode.LeftShift))
                        {
                            //playerAni.SetBool("isRun", false);
                            //playerAni.SetBool("isWalk", true);
                            //pState = PlayerState.WalkForward;
                            ChangeAnim("isRun", "isWalk", PlayerState.WalkForward);
                        }
                        if (Input.GetKeyUp(KeyCode.W))
                        {
                            //playerAni.SetBool("isRun", false);
                            //playerAni.SetBool("isIdle", true);
                            //pState = PlayerState.Idle;
                            ChangeAnim("isRun", "isIdle", PlayerState.Idle);
                        }
                        if (Input.GetKeyDown(KeyCode.A))
                        {
                            //playerAni.SetBool("isRun", false);
                            //playerAni.SetBool("isLeftRun", true);
                            //pState = PlayerState.LeftTurnRun;
                            ChangeAnim("isRun", "isLeftRun", PlayerState.LeftTurnRun);
                        }
                        if (Input.GetKeyDown(KeyCode.D))
                        {
                            //playerAni.SetBool("isRun", false);
                            //playerAni.SetBool("isRightRun", true);
                            //pState = PlayerState.RightTurnRun;
                            ChangeAnim("isRun", "isRightRun", PlayerState.RightTurnRun);
                        }
                        if (Input.GetKeyDown(KeyCode.R))
                        {
                            ChangeAnim("isRun", "isWeaponRaise", PlayerState.RaiseGun);
                        }
                    }
                    break;
                case PlayerState.LeftTurnRun:
                    {
                        transform.Translate(Vector3.forward * Time.deltaTime * runSpeed);
                        transform.Rotate(new Vector3(-1, 0, 0) * Time.deltaTime);
                        if (Input.GetKeyUp(KeyCode.LeftShift))
                        {
                            //playerAni.SetBool("isLeftRun", false);
                            //playerAni.SetBool("isWalk", true);
                            //pState = PlayerState.WalkForward;
                            ChangeAnim("isLeftRun", "isWalk", PlayerState.WalkForward);
                        }
                        if (Input.GetKeyUp(KeyCode.W))
                        {
                            //playerAni.SetBool("isLeftRun", false);
                            //playerAni.SetBool("isIdle", true);
                            //pState = PlayerState.Idle;
                            ChangeAnim("isLeftRun", "isIdle", PlayerState.Idle);
                        }
                        if (Input.GetKeyUp(KeyCode.A))
                        {
                            //playerAni.SetBool("isLeftRun", false);
                            //playerAni.SetBool("isRun", true);
                            //pState = PlayerState.RunForward;
                            ChangeAnim("isLeftRun", "isRun", PlayerState.RunForward);
                        }
                        if (Input.GetKeyDown(KeyCode.R))
                        {
                            ChangeAnim("isLeftRun", "isWeaponRaise", PlayerState.RaiseGun);
                        }
                    }
                    break;
                case PlayerState.RightTurnRun:
                    {
                        transform.Translate(Vector3.forward * Time.deltaTime * runSpeed);
                        transform.Rotate(new Vector3(1, 0, 0) * Time.deltaTime);
                        if (Input.GetKeyUp(KeyCode.LeftShift))
                        {
                            //playerAni.SetBool("isRightRun", false);
                            //playerAni.SetBool("isWalk", true);
                            //pState = PlayerState.WalkForward;
                            ChangeAnim("isRightRun", "isWalk", PlayerState.WalkForward);
                        }
                        if (Input.GetKeyUp(KeyCode.W))
                        {
                            //playerAni.SetBool("isRightRun", false);
                            //playerAni.SetBool("isIdle", true);
                            //pState = PlayerState.Idle;
                            ChangeAnim("isRightRun", "isIdle", PlayerState.Idle);
                        }
                        if (Input.GetKeyUp(KeyCode.D))
                        {
                            //playerAni.SetBool("isRightRun", false);
                            //playerAni.SetBool("isRun", true);
                            //pState = PlayerState.RunForward;
                            ChangeAnim("isRightRun", "isRun", PlayerState.RunForward);
                        }
                        if (Input.GetKeyDown(KeyCode.R))
                        {
                            StartCoroutine(CameraZoom(Camera.main.transform.position, Camera.main.transform.position + Vector3.forward));
                            ChangeAnim("isRightRun", "isWeaponRaise", PlayerState.RaiseGun);
                        }
                    }
                    break;
                case PlayerState.RaiseGun:
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        Camera.main.transform.localPosition = new Vector3(0.3f, 2.1f, -0.5f);

                        if (Input.GetMouseButtonDown(0))
                        {
                            Rigidbody bulletInstance = Instantiate(bulletObj, bulletOutPos.position, bulletOutPos.rotation);
                            bulletInstance.velocity = 3.0f * bulletOutPos.forward;

                        }

                        if (Input.GetKeyDown(KeyCode.R))
                        {
                            Cursor.lockState = CursorLockMode.None;
                            Camera.main.transform.localPosition = new Vector3(0.0f, 3f, -3f);
                            ChangeAnim("isWeaponRaise", "isIdle", PlayerState.Idle);
                        }
                    }
                    break;

            }
            #endregion

            if (Input.GetKeyDown(KeyCode.S))
            {
                playerAni.SetBool("isWalk", false);
                playerAni.SetBool("isRun", false);
                playerAni.SetBool("isLeftRun", false);
                playerAni.SetBool("isRightRun", false);
                playerAni.SetBool("isWeaponRaise", false);
                pState = PlayerState.Idle;
            }

        }


        //if (Input.GetKey(KeyCode.W))
        //{
        //    playerAni.SetBool("isIdle", false);
        //    playerAni.SetBool("isRun", true);
        //    transform.Translate(Vector3.forward * Time.deltaTime);
        //}

        //if (Input.GetKeyUp(KeyCode.W))
        //{
        //    playerAni.SetBool("isRun", false);
        //    playerAni.SetBool("isIdle", true);
        //}


    }

    void ChangeAnim(string _from, string _to, PlayerState changeState)
    {
        playerAni.SetBool(_from, false);
        playerAni.SetBool(_to, true);
        pState = changeState;
    }
    
    public int getHP()
    {
        return hp;
    }

    public int getBullet()
    {
        return bullet;
    }

    public void DataFromJson()
    {
        string filePath = Path.Combine(Application.dataPath + "/MyProject/FinalProject", "Player.json");

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            PlayerJson savedData = JsonUtility.FromJson<PlayerJson>(dataAsJson);
            transform.position = new Vector3(savedData.xAxis, savedData.yAxis, savedData.zAxis);
            hp = savedData.hp;
            bullet = savedData.bullet;
            print("Json Loaded");
        }
        else
            print("File not found");
        
        //savedData = new PlayerJson();
        //savedData = JsonUtility.FromJson<PlayerJson>()
    }

    IEnumerator CameraZoom(Vector3 _from, Vector3 _to)
    {
        while(true)
        {
            //print("Coroutine Started");
            if (Vector3.Distance(_from, _to) > 0.1f)
            {
                //print("Lerp");
                Camera.main.transform.position = Vector3.Lerp(_from, _to, Time.deltaTime);
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield break;
            }
        }
    }
}
