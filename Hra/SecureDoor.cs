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
        // Zavøení dveøí po krátké pauze
        yield return new WaitForSeconds(0f);
        Debug.Log("Dveøe se zavírají.");

        // Získání referencí na dveøe
        GameObject Door = GameObject.Find("SecureDoor");


        // Pohyb dveøí do zavøené polohy
        while (Door.transform.localPosition.x > 2.08f)
        {
            Door.transform.localPosition = Vector3.MoveTowards(Door.transform.localPosition, new Vector3(2.08f, 9.33f, -6.38f), Time.deltaTime * 2f);
            yield return null;
        }

    }

    IEnumerator OpenDoor()
    {
        // Získání referencí na dveøe
        GameObject Door = GameObject.Find("SecureDoor");


        // Pohyb dveøí do zavøené polohy
        while (Door.transform.localPosition.x < 7.76f)
        {
            Door.transform.localPosition = Vector3.MoveTowards(Door.transform.localPosition, new Vector3(7.76f, 9.33f, -6.38f), Time.deltaTime * 2f);
            yield return null;
        }

        // Poèkejte nìkolik sekund, než se dveøe zavøou
        yield return new WaitForSeconds(5f);


    }
}
