using UnityEngine;

namespace mg.pummelz
{

    public static class MGPumPSTExtensions
    {
        public static bool isSelected(this MGPumPST pst, int playerID)
        {
            if (pst == MGPumPST.Source || pst == MGPumPST.Opponent)
            {
                Debug.LogError("Cannot determine isSelected without source for " + pst.ToString());
            }

            return (pst == MGPumPST.Any || (int)pst == playerID);
        }

        public static bool isSelected(this MGPumPST pst, int playerID, MGPumEntity source)
        {
            return (pst == MGPumPST.Any || (int)pst == playerID) || (pst == MGPumPST.Source && (source.ownerID == playerID) || (pst == MGPumPST.Opponent && (source.ownerID != playerID)) || (pst == MGPumPST.GrantSource && (source.ownerID == playerID)) || (pst == MGPumPST.GrantOpponent && (source.ownerID != playerID)));
             
        }

    }
    public enum MGPumPST
    {
        Player0 = 0,
        Player1 = 1,
        Source = 2,
        Opponent = 3,
        Any = 4,
        GrantSource = 5,
        GrantOpponent = 6
    }
}
