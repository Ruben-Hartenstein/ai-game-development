using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumEffectExecutionState
    {
        public Dictionary<int , object > values;

        public MGPumEffectExecutionState()
        {
            values = new Dictionary<int, object>();

        }


        public void set<T>(int key, T value)
        {
            if(!values.ContainsKey(key))
            {
                values.Add(key, value);
            }
            else
            {
                values[key] = value;
            }
            
        }

        public T get<T>(int key)
        {
            if (!values.ContainsKey(key))
            {
                return default(T);
            }
            else
            {
                return (T)values[key];
            }
        }

        public bool contains(int key)
        {
            return values.ContainsKey(key);
        }

    }
}
