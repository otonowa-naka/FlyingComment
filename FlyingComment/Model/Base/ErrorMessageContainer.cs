using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingComment.Model
{
    public　class ErrorMessageContainer
    {
        private Dictionary<string, string> _Message = new Dictionary<string, string>();


        public ErrorMessageContainer()
        {

        }

        public ErrorMessageContainer(string propertyName, string message)
        {
            Add(propertyName, message);
        }

        public void Add(string propertyName, string message)
        {
            _Message[propertyName] = message;
        }

        public void Remove(string propertyName)
        {
            _Message.Remove(propertyName);
            
        }

        public bool HasError()
        {
            return _Message.Count > 0;
        }

        public string this[string propertyName]
        {
            get
            {
                string ret = null;
                _Message.TryGetValue(propertyName, out ret);

                return ret;
            }

        }

    }
}
