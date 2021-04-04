using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ralph
{
    public class Parser
    {
        private string src;
        private int pos;

        private char Current { get => src[pos]; }

        public Parser(string src)
        {
            this.src = src;
        }

        public Token[] Parse()
        {
            var toks = new List<Token>();
            while (pos < src.Length)
            {

            }

            return toks.ToArray();
        }
    }
}
