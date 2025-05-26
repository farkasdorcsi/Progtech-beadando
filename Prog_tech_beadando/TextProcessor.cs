using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog_korny_wpf_beadando
{
    internal class TextProcessor
    {
        private StringBuilder sb = new StringBuilder();
        private IListStrategy listStrategy;

        public void SetOutputFormat(Strategy format) 
        {
            switch (format)
            {
                case Strategy.html:
                    listStrategy = new HtmlListStrategy();
                    break;
                case Strategy.md:
                    listStrategy = new MarkdownListStrategy();
                    break;
                default:
                    break;
            }
        }
        public void AppendList(IEnumerable<string> items) 
        {
            listStrategy.Start(sb);
            foreach (var item in items)
                listStrategy.AddListItem(sb, item);
            listStrategy.End(sb);
        }

        public StringBuilder Clear() {
            return sb.Clear();
        }
        public override string ToString()
        {
            return sb.ToString();
        }
    }
}
