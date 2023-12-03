using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSDuckHelper.Events
{
    public class ParserEventArgs<T>
    {
        private readonly T data;

        public T Data { get => data; }

        public ParserEventArgs(T value)
        {
            data = value;
        }
    }
}
