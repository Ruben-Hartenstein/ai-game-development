namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumApplyStateRulesSEvent : MGPumSuperEvent
    {
        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumGameEvent result = new MGPumApplyStateRulesSEvent();
            this.copyToGameEvent(result);
            return result;
        }
    }
}
