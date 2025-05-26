using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog_korny_wpf_beadando
{
    public interface IPrototype<T>
    {
        T Clone();
    }

    public class Book : IPrototype<Book>
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Condition { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int UserId { get; set; }

        public Book Clone()
        {
            return new Book
            {
                Title = this.Title,
                Author = this.Author,
                Condition = this.Condition,
                Description = this.Description,
                ImagePath = this.ImagePath,
                UserId = this.UserId
            };
        }

    }
}
