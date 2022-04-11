using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RelegatiaCCG.rccg.util
{
    public class ReturnableCoroutine<T>
    {

        private T _result;
        private Exception exception;

        internal MonoBehaviour owner;
        internal Func<IEnumerator> coroutineAction;
        

        public T result
        {
            get
            {
                if (exception != null)
                {
                    throw exception;
                }
                else
                {
                    return _result;
                }

            }
        }

        public ReturnableCoroutine(MonoBehaviour owner, Func<IEnumerator> coroutine)
        {
            this.owner = owner;
            this.coroutineAction = coroutine;

            this.exception = null;
            this._result = default(T);
            

        }

        public Coroutine run()
        {
            Coroutine routine = owner.StartCoroutine(InternalRoutine());
            return routine;
        }

        //private IEnumerator InternalRoutine(IEnumerator coroutine)
        //{
        //    try
        //    {
        //        if (!coroutine.MoveNext())
        //        {
        //            yield break;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogError("Exception in Coroutine:" + e.Message + "\n" + e.StackTrace);
        //        this.exception = e;
        //        yield break;
        //    }

        //    object yieldedValue = coroutine.Current;
        //    if (yieldedValue != null && yieldedValue is T)
        //    {
        //        _result = (T)yieldedValue;
        //        yield break;
        //    }
        //    else
        //    {
        //        yield return coroutine.Current;
        //    }
        //}

        private IEnumerator InternalRoutine()
        {
            IEnumerator enumerator = coroutineAction();
            while(true)
            {
                try
                {
                    if (!enumerator.MoveNext())
                    {
                        yield break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception in Coroutine:" + e.Message + "\n" + e.StackTrace);
                    this.exception = e;
                    yield break;
                }

                object yieldedValue = enumerator.Current;
                if (yieldedValue != null && yieldedValue is T)
                {
                    _result = (T)yieldedValue;
                    yield break;
                }
                else
                {
                    yield return enumerator.Current;
                }
            }
            
        }

       
    }
}

