using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Busted.AI
{
    public class AI : MonoBehaviour
    {
        protected Coroutine _currentCoroutine;
        protected AIState _aiState;

        protected float _score;     //Tells AI if it's doing well or not; higher is better
                                    //protected float _wits;      //Likelihood that the AI will make smart moves?
    }

    public class AIState
    {

    }
}
