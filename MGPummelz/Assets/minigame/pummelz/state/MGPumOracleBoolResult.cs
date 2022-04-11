using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{

    [System.Serializable]
    public class MGPumOracleBoolResult
    {
        public bool result { get; set; }
        public string reason { get; set; }

        public MGPumOracleBoolResult()
        {
            this.result = true;
            this.reason = null;
        }

        public MGPumOracleBoolResult(bool result)
        {
            this.result = result;
            this.reason = "";
        }
    }
}
