using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class celltalog : MonoBehaviour
{
    [SerializeField] GameObject cell;
    bool doTick = false;
    int tickRate = 200;
    float lastTime;
    [SerializeField] Text buttonText;
    public Dictionary<int, Dictionary<int, GameObject>> celldict_new;
    public Dictionary<int, Dictionary<int, GameObject>> celldict_old;
    // Start is called before the first frame update
    void Start()
    {
        celldict_new = new Dictionary<int, Dictionary<int, GameObject>>();
        celldict_old = new Dictionary<int, Dictionary<int, GameObject>>();
        lastTime = Time.realtimeSinceStartup;
    }

    public bool exists(int x, int y, Dictionary<int, Dictionary<int, GameObject>> dic)
    {
        try
        {

            var a = dic[y][x];
            return true;
        }
        catch (KeyNotFoundException)
        {
            return false;
        }

    }

    public bool remove(int x, int y, Dictionary<int, Dictionary<int, GameObject>> dic)
    {
        if (dic.ContainsKey(y))
        {
            if (dic[y].ContainsKey(x))
            {
                Destroy(dic[y][x]);
                dic[y].Remove(x);
                return true;
            }
        }
        return false;
    }

    public bool addCell(GameObject gobj, Dictionary<int, Dictionary<int, GameObject>> dic)
    {
        Dictionary<int, GameObject> dic2;
        try
        {
            dic2 = dic[(int)Mathf.Floor(gobj.transform.position.y)];
            {
                try
                {
                    dic2.Add((int)Mathf.Floor(gobj.transform.position.x), gobj);
                    Debug.Log(string.Format("Successfully Added Cell x:{0} y:{1}", (int)Mathf.Floor(gobj.transform.position.x), (int)Mathf.Floor(gobj.transform.position.y)));
                    return true;
                }
                catch
                {
                    Debug.Log("Cell is already there. Wont Add!");
                    return false;
                }
            }

        }
        catch (KeyNotFoundException)
        {

            dic2 = new Dictionary<int, GameObject>();
            dic2.Add((int)Mathf.Floor(gobj.transform.position.x), gobj);
            dic.Add((int)Mathf.Floor(gobj.transform.position.y), dic2);
            Debug.Log(string.Format("Cell line wasn't in primary dictionary. Added new line for cell of coordinates x:{0} , y:{1}.", (int)gobj.transform.position.x, (int)gobj.transform.position.y));
            return true;
        }

    }



    public bool evaluate(int x, int y, Dictionary<int, Dictionary<int, GameObject>> dic, Dictionary<int, Dictionary<int, GameObject>> dic2)
    {

        bool doesThisCellExist = exists(x, y, dic);
        int nieghbour_count = 0;

        //right

        if (exists(x + 1, y, dic))
        {
            nieghbour_count++;
        }
        else if (doesThisCellExist)
            evaluate(x + 1, y, dic, dic2);

        //topright

        if (exists(x + 1, y + 1, dic))
        {
            nieghbour_count++;
        }
        else if (doesThisCellExist)
            evaluate(x + 1, y + 1, dic, dic2);

        //bottomright

        if (exists(x + 1, y - 1, dic))
        {
            nieghbour_count++;
        }
        else if (doesThisCellExist)
            evaluate(x + 1, y - 1, dic, dic2);

        //top

        if (exists(x, y + 1, dic))
        {
            nieghbour_count++;
        }
        else if (doesThisCellExist)
            evaluate(x, y + 1, dic, dic2);

        //bottom
        if (exists(x, y - 1, dic))
        {
            nieghbour_count++;
        }
        else if (doesThisCellExist)
            evaluate(x, y - 1, dic, dic2);

        //topleft

        if (exists(x - 1, y + 1, dic))
        {
            nieghbour_count++;
        }
        else if (doesThisCellExist)
            evaluate(x - 1, y + 1, dic, dic2);

        //left

        if (exists(x - 1, y, dic))
        {
            nieghbour_count++;
        }
        else if (doesThisCellExist)
            evaluate(x - 1, y, dic, dic2);

        //bottomleft

        if (exists(x - 1, y - 1, dic))
        {
            nieghbour_count++;
        }
        else if (doesThisCellExist)
            evaluate(x - 1, y - 1, dic, dic2);

        if (((nieghbour_count == 2 || nieghbour_count == 3) && doesThisCellExist) || (nieghbour_count == 3 && !doesThisCellExist))
        {
            GameObject stans = GameObject.Instantiate(cell, new Vector3(x + 0.5f, y + 0.5f), new Quaternion());
            if (addCell(stans, dic2))
            {
                stans.SetActive(true);
                return true;
            }
            else
            {
                Destroy(stans);
            }
        }

        return false;

    }
    public void tick()
    {


        Dictionary<int, Dictionary<int, GameObject>>.KeyCollection keycolY = celldict_new.Keys;
        Dictionary<int, GameObject>.KeyCollection keycolX;

        foreach (int i in keycolY)
        {

            keycolX = celldict_new[i].Keys;
            foreach (int j in keycolX)
            {
                evaluate(j, i, celldict_new, celldict_old);
                Destroy(celldict_new[i][j]);
            }
        }
        celldict_new = new Dictionary<int, Dictionary<int, GameObject>>(celldict_old);
        celldict_old = new Dictionary<int, Dictionary<int, GameObject>>();
    }


    public void toggleTicking()
    {
        doTick = !doTick;
        if (buttonText.text == "START")
        {
            buttonText.text = "STOP";
        }
        else
            buttonText.text = "START";

    }
    public void setTicking(bool ticka)
    {

        doTick = ticka;
        if (ticka)
        {
            buttonText.text = "STOP";
        }
        else
            buttonText.text = "START";
    }

    public void SetTickRate(string s)
    {
        int.TryParse(s, out tickRate);
    }
    public void Update()
    {

        if (lastTime + tickRate / 1000.0f <= Time.realtimeSinceStartup && doTick)
        {
            lastTime = Time.realtimeSinceStartup;
            tick();
        }

    }
}
