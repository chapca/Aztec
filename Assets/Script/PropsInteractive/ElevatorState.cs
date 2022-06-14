using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorState : MonoBehaviour
{
    public bool up, down;

   void StateUp()
   {
        up = true;
        down = false;
   }

    void StateDown()
    {
        down = true;
        up = false;
    }
}
