using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp_NET_4._8_learning
{
    public partial class Login : Form
    {
        private readonly CarRentalEntities _db;
        public Login()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                SHA256 sha = SHA256.Create();

                var username = tbUsername.Text.Trim();
                var password = tbPassword.Text;

                var hashedPassword = Utilities.HashPassword(password);

                var user = _db.Users.FirstOrDefault(q => 
                    q.Username == username && 
                    q.Password == hashedPassword &&
                    q.isActive == true);

                if (user == null)
                {
                    MessageBox.Show("Please provide valid credentials.");
                }
                else
                {
                    // login successful. Launch mainwindow                    
                    var mainWindow = new MainWindow(this, user);
                    mainWindow.Show();
                    Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong! Please try again.");
            }
        }
    }
}
