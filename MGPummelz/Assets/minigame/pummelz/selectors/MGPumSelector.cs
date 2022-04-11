using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    public interface MGPumSelector
    {
       
        MGPumSelection getSelection();

        MGPumSelector deepCopySelector(MGPumGameState state);
    }
}
