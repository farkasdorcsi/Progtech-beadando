using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Prog_korny_wpf_beadando
{
    [TestClass]
    public  class LoginWindowTest
    {
        LoginWindow login = new LoginWindow();

        private static string username = "TestUser";
        private static string password = "TestPass";
        

        [TestMethod]
        public void LoginTest()
        {
            login.UsernameBox.Text.Equals(username);
            login.PasswordBox.Equals(password);


            Assert.IsFalse(login.IsVisible);
            
        }
    }
}
