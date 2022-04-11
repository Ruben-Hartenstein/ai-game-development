
using UnityEngine.EventSystems;

using System.Collections.Generic;
using UnityEngine.UI;

using System;
using UnityEngine;

//public class ForeachTest
//{
//    public ForeachTest()
//    {
//        List<int> list = new List<int>();
//        for(int i = 0; i <10; i++)
//        {
//            list.Add(i);
//        }
//        foreach(int item in list)
//        {
//            Debug.Log(item);
//        }
//    }
//}

//public class Executor13
//{
//    public void execute(Action<int> function)
//    {
//        function(13);
//    }

//    private void example(int input)
//    {
//        Debug.LogError(input);
//    }

//    public void test()
//    {
//        Action<int> testAction = this.example;
//        execute(testAction);
//    }
//}



//public class Cycle
//{
//    public virtual int getWheels()
//    {
//        return 2;
//    }
//}

//public class Unicycle : Cycle
//{
//    public override int getWheels()
//    {
//        return 1;
//    }
//}

//public class GetSetTest
//{
//    public string name { get; protected set; } = "Humphrey";

//    private int _boundedVal;

//    public int boundedVal
//    {
//        get { return _boundedVal; }
//        set
//        {
//            if (value > 100)
//            {
//                boundedVal = 100;
//            }
//            else
//            {
//                boundedVal = value;
//            }
//        }
//    }
//}


//    public class TextChanger : MonoBehaviour
//{
//    public Text textRender;
//    internal string content;
//    private int length;
    
//    public TextChanger() : base()
//    {
//        updateText("");
//    }
//    public void updateText(string newContent)
//    {
//        this.content = newContent;
//        this.length = newContent.Length;
//        this.textRender.text = this.content;
//    }
//}

//public class CollectionsTest
//{
//    private string courseID;
//    private bool finished;
//    internal IList<int> matrikel;
//    internal IDictionary<int, float> gradesForMatrikel;

//    public CollectionsTest() : base()
//    {
//        this.courseID = "GAME_AI";
//        this.finished = false;
//        this.matrikel = new List<int>();
//        this.gradesForMatrikel = new Dictionary<int, float>();
//    }
//}

//namespace ai.gamedev
//{
//    public class HelloWorld : MonoBehaviour
//    {
//        public void Start()
//        {
//            Debug.Log("Hello World!");
//        }
//    }

   
//}


//public class TestClass : MonoBehaviour, IPointerClickHandler
//{
//    internal int objVal;
//    internal static int baseVal = 23;

//    public TestClass() : base()
//    {
//        this.objVal = TestClass.baseVal;
//    }

//    public void OnPointerClick(PointerEventData eventData)
//    {
//        throw new System.NotImplementedException();
//    }
//}

