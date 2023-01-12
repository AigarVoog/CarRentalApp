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
    public partial class ManageUsers : Form
    {
        private readonly CarRentalEntities _db;
        public ManageUsers()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            if (!Utilities.FormIsOpen("AddUser"))
            {
                var addUser = new AddUser(this); // pass in this, so we can call functions made here in AddUser form
                addUser.MdiParent = this.MdiParent;
                addUser.Show();
            }            
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                // get id of the selected user
                var id = (int)gvUsersList.SelectedRows[0].Cells["id"].Value;

                // query database for the user with the id
                var user = _db.Users.FirstOrDefault(q => q.id == id);

                // give the user a generic default password that is already hashed
                var newPassword = Utilities.DefaultHashedPassword();
                user.Password = newPassword;

                _db.SaveChanges();

                MessageBox.Show($"{user.Username}'s password has been reset.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }
        public void PopulateGrid()
        {
            var users = _db.Users
                .Select(q => new
                {
                    q.id,
                    q.Username,
                    q.UserRoles.FirstOrDefault().Role.name, // get the role of the user from the roles table
                    q.Password,
                    q.isActive
                })
                .ToList();

            gvUsersList.DataSource = users;
            gvUsersList.Columns["name"].HeaderText = "Role";
            gvUsersList.Columns["isActive"].HeaderText = "Active";
            gvUsersList.Columns["id"].Visible = false;
        }

        private void btnToggleUserActivation_Click(object sender, EventArgs e)
        {
            try
            {
                // get id of the selected user
                var id = (int)gvUsersList.SelectedRows[0].Cells["id"].Value;

                // query database for the user with the id
                var user = _db.Users.FirstOrDefault(q => q.id == id);

                // toggle user active status
                // user.isActive = user.isActive == true ? false : true;
                if (user.isActive == true)
                {
                    user.isActive = false;
                    MessageBox.Show($"{user.Username}'s account has been deactivated.");
                }
                else
                {
                    user.isActive = true;
                    MessageBox.Show($"{user.Username}'s account has been activated.");
                }

                _db.SaveChanges();

                PopulateGrid();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
    }
}
