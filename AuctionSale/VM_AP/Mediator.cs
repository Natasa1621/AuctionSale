using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM_AP
{
    public class Mediator
    {
        static readonly Mediator instance = new Mediator();

        public static Mediator Instance
        {
            get
            {
                return instance;
            }
        }

        private Mediator()
        { }

        private static Dictionary<string, List<Action<object>>> subscribers = new Dictionary<string, List<Action<object>>>();

        public void Register(string message, Action<object> action)
        {
            if (!subscribers.ContainsKey(message))                       
                subscribers.Add(message, new List<Action<object>> { action });          
            else           
                subscribers[message].Add(action);          
        }
        
        public void Notify(string message, Object param)
        {
            foreach (var item in subscribers)
            {
                if (item.Key.Equals(message))
                {
                    foreach (var itemValue in item.Value)
                    {
                        Action<object> method = (Action<object>)itemValue;
                        method.Invoke(param);
                    }
                }
            }
        }
    }
}