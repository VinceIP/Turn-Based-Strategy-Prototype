using UnityEngine;

namespace Busted
{

    /// <summary>
    /// All units in-editor will be in this state
    /// </summary>
    public class UnitStateEditor : UnitState
    {
        public UnitStateEditor(Unit unit) : base(unit)
        {
            stateName = "UnitStateEditor";
        }

    }
}
