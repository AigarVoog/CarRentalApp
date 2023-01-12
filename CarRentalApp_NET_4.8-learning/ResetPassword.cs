using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp_NET_4._8_learning
{
    public partial class ResetPassword : Form
    {
        private readonly CarRentalEntities _db;
        private User _user;
        public ResetPassword(User user)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            _user = user;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                var newPassword = tbNewPassword.Text;
                var confirmPassword = tbConfirmPassword.Text;
                var user = _db.Users.FirstOrDefault(q => q.id == _user.id);

                if (newPassword != confirmPassword)
                {
                    MessageBox.Show("Passwords do not match! Please try again.");
                }
                else
                {
                    user.Password = Utilities.HashPassword(newPassword);
                    _db.SaveChanges();
                    MessageBox.Show("Password was reset successfully.");
                    Close();
                }     
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong. Please try again.");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
