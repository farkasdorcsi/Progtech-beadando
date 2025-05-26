using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog_korny_wpf_beadando
{
    internal class MarkdownListStrategy : IListStrategy
    {
        public void AddListItem(StringBuilder sb, string item)
        {
            sb.AppendLine($" *{item}");
        }

        public void End(StringBuilder sb)
        {
            
        }

        public void Start(StringBuilder sb)
        {
            
        }
    }
}
