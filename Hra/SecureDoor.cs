using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecureDoor : MonoBehaviour
{
    public CheckTag GreenStatue;
    public CheckTag YellowStatue;
    public CheckTag BlackStatue;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CloseDoor());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GreenStatue.isRight == true && YellowStatue.isRight == true && BlackStatue.isRight == true)
        {
            StartCoroutine(OpenDoor());
        }
        
    }

    IEnumerator CloseDoor()
    {
        // Zav�en� dve�� po kr�tk� pauze
        yield return new WaitForSeconds(0f);
        Debug.Log("Dve�e se zav�raj�.");

        // Z�sk�n� referenc� na dve�e
        GameObject Door = GameObject.Find("SecureDoor");


        // Pohyb dve�� do zav�en� polohy
        while (Door.transform.localPosition.x > 2.08f)
        {
            Door.transform.localPosition = Vector3.MoveTowards(Door.transform.localPosition, new Vector3(2.08f, 9.33f, -6.38f), Time.deltaTime * 2f);
            yield return null;
        }

    }

    IEnumerator OpenDoor()
    {
        // Z�sk�n� referenc� na dve�e
        GameObject Door = GameObject.Find("SecureDoor");


        // Pohyb dve�� do zav�en� polohy
        while (Door.transform.localPosition.x < 7.76f)
        {
            Door.transform.localPosition = Vector3.MoveTowards(Door.transform.localPosition, new Vector3(7.76f, 9.33f, -6.38f), Time.deltaTime * 2f);
            yield return null;
        }

        // Po�kejte n�kolik sekund, ne� se dve�e zav�ou
        yield return new WaitForSeconds(5f);


    }
}
