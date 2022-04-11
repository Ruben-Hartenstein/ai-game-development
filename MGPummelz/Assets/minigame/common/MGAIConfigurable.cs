using System.Collections.Generic;

public interface MGAIConfigurable
{
    List<string> getAITypes();

    void setAIType(int playerID, string aiType);
}
