using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prog_korny_wpf_beadando;

namespace Prog_korny_wpf_beadando.Test
{
    [TestClass]
    public class RegisterWindowTest
    {
        RegisterWindow reg = new RegisterWindow();

        [TestMethod]
        public void ValidUserTest() 
        {
            reg.UsernameBox.Equals("valid");
            reg.PasswordBox.Password.Equals("pass123");
            reg.EmailBox.Equals("user@email.com");

            reg.RegisterUser();
            

            Assert.IsFalse(reg.IsVisible);
        }
    }
}
