using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SerializeToJson : MonoBehaviour {

    public GameObject PlayerInfo;
    Player playerCode;
    PlayerJson myPlayer;


    // Use this for initialization
    void Start () {
        playerCode = PlayerInfo.GetComponent<Player>();
        

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Save()
    {
        myPlayer = new PlayerJson(PlayerInfo.transform.position.x, PlayerInfo.transform.position.y, PlayerInfo.transform.position.z, playerCode.getHP(), playerCode.getBullet());

        string json = JsonUtility.ToJson(myPlayer);

        File.WriteAllText(Application.dataPath + "/MyProject/FinalProject/Player.json", json.ToString());

        print("Saved");
    }
}

class PlayerJson
{
    public float xAxis;
    public float yAxis;
    public float zAxis;
    public int hp;
    public int bullet;

    public PlayerJson()
    {
        xAxis = 0;
        yAxis = 0;
        zAxis = 0;
        hp = 0;
        bullet = 0;
    }

    public PlayerJson(float x, float y, float z, int _hp, int _bullet)
    {
        xAxis = x;
        yAxis = y;
        zAxis = z;
        hp = _hp;
        bullet = _bullet;
    }
}