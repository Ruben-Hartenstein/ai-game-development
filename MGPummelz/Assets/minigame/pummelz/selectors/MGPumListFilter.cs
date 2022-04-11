using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    public class MGPumListFilter : MGPumFilter
    {
        private string[] units;

        private string i18nKeySingular;
        private string i18nKeyPlural;

        public MGPumListFilter(string i18nKeySingular, string i18nKeyPlural, params string[] units) : base()
        {
            this.units = units;
            this.i18nKeySingular = i18nKeySingular;
            this.i18nKeyPlural = i18nKeyPlural;
        }

        protected override List<MGPumEntity> applyBaseFilter(MGPumGameState state, MGPumEntity source)
        {
            
            List<MGPumEntity> result = new List<MGPumEntity>(units.Length);
            foreach (string s in units)
            {
                result.Add(MGPumSetHandler.getInstance().getUnitByID(s));
            }
            
            
            return result;
        }

        protected override List<MGPumEntity> applyBaseFilter(List<MGPumEntity> entities, MGPumEntity source)
        {
            
            List<MGPumEntity> result = new List<MGPumEntity>(units.Length);
            foreach (string s in units)
            {
                result.Add(MGPumSetHandler.getInstance().getUnitByID(s));
            }
            

            return result;
        }
    }
}
