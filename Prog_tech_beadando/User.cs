using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Prog_korny_wpf_beadando
{
    public class User
    {
        public int Id;
        public string Name;
        public string Email;
        public string Password;
        public User(int id,  string name, string email, string password) 
        {
            SetId(id);
            SetUsername(name);
            SetPassword(password);
            SetEmail(email);

        }

        private void SetId(int id) {
            this.Id = id;
        }

        private void SetUsername(string name) {
            this.Name = name;
        }

        public string GetUsername() 
        {
            return this.Name;
        }

        private void SetPassword(string password) {
           this.Password = password;
        }

        public string GetPassword() { 
            return this.Password;
        }

        public string GetEmail() { 
            return this.Email;
        }

        private void SetEmail(string email) {
            this.Email = email;
        }
    }
}
