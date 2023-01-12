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
    public partial class AddUser : Form
    {
        private readonly CarRentalEntities _db;
        private ManageUsers _manageUsers;
        public AddUser()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }
        public AddUser(ManageUsers manageUsers)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            _manageUsers = manageUsers;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            var roles = _db.Roles.ToList();
            cbRoleList.DataSource = roles;
            cbRoleList.ValueMember = "id";
            cbRoleList.DisplayMember= "name";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var username = tbUsername.Text;
                var roleId = (int)cbRoleList.SelectedValue;
                var password = Utilities.DefaultHashedPassword();

                var user = new User
                {
                    Username = username,
                    Password = password,
                    isActive = true,
                };

                _db.Users.Add(user);
                _db.SaveChanges();

                // userId must be set after the save to database,
                // because the database auto generates the id values
                var userId = user.id;


                var userRole = new UserRole
                {
                    roleid = roleId,
                    userid = userId
                };

                _db.UserRoles.Add(userRole);
                _db.SaveChanges();                

                MessageBox.Show("New user added successfully!");

                // this function is from ManageUsers Class
                _manageUsers.PopulateGrid();

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);                
            }
        }
    }
}
