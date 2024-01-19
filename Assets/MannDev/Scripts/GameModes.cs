using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModes : MonoBehaviour
{
    public GameObject EasyMode;
    public GameObject MediumMode;
    public GameObject HardMode;

    private Vector3 StartPoz = new Vector3(-1.62f,-1.82000005f,14.8599997f);

    public bool Test_Easy;
    public bool Test_Medium;
    public bool Test_Hard;
    public bool Test_All;

    public void OnselectEasyMode()
    {
        GameObject inst = Instantiate(EasyMode,StartPoz,Quaternion.identity);
        inst.GetComponent<Spawner>().StartGame();
        this.gameObject.SetActive(false);
    }

    public void OnselectMediumMode()
    {
        GameObject inst = Instantiate(MediumMode, StartPoz, Quaternion.identity);
        inst.GetComponent<Spawner>().StartGame();
        this.gameObject.SetActive(false);
    }

    public void OnselectHardMode()
    {
        GameObject inst = Instantiate(HardMode, StartPoz, Quaternion.identity);
        inst.GetComponent<Spawner>().StartGame();
        this.gameObject.SetActive(false);
    }
    void Test_InstantiateAndStartGame(GameObject mode)
    {
        GameObject inst = Instantiate(mode, StartPoz, Quaternion.identity);
        inst.GetComponent<Spawner>().StartGame();
        this.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (this.gameObject.activeSelf)
        {
            if (Test_Easy)
            {
                Test_InstantiateAndStartGame(EasyMode);
            }
            else if (Test_Medium)
            {
                Test_InstantiateAndStartGame(MediumMode);
            }
            else if (Test_Hard)
            {
                Test_InstantiateAndStartGame(HardMode);
            }
            else if (Test_All)
            {
                Test_InstantiateAndStartGame(EasyMode);
                Test_InstantiateAndStartGame(MediumMode);
                Test_InstantiateAndStartGame(HardMode);
            }
        }
    }
}
