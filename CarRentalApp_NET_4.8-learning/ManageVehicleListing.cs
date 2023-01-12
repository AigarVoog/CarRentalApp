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
    public partial class  ManageVehicleListing : Form
    {
        private readonly CarRentalEntities _db;
        public ManageVehicleListing()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void ManageVehicleListing_Load(object sender, EventArgs e)
        {
            // Select * From Cars
            // var cars = _db.Cars.ToList();

            // Select ID as CarId, name as CarName from Cars
            //var cars = _db.Cars
            //    .Select(q => new { CarId = q.id, CarName = q.Make })
            //    .ToList();

            var cars = _db.Cars
                .Select(q => new
                {
                    Make = q.Make,
                    Model = q.Model,
                    VIN = q.VIN,
                    Year = q.Year,
                    LicencePlateNumber = q.LicensePlateNumber,
                    q.id // same as id = q.id
                })
                .ToList();

            gvVehicleList.DataSource = cars;
            gvVehicleList.Columns[4].HeaderText = "Licence Plate Number";
            gvVehicleList.Columns[5].Visible = false;
            //gvVehicleList.Columns[0].HeaderText = "ID";
            //gvVehicleList.Columns[1].HeaderText = "NAME";
        }

        

        private void btnAddNewCar_Click(object sender, EventArgs e)
        {
            AddEditVehicle addEditVehicle = new AddEditVehicle(this);
            addEditVehicle.MdiParent = this.MdiParent;
            addEditVehicle.Show();          
        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
            try
            {
                // get id of the selected row
                var id = (int)gvVehicleList.SelectedRows[0].Cells["id"].Value;

                // query database for record
                var car = _db.Cars.FirstOrDefault(q => q.id == id);

                // launch AddEditvehicle form with data
                var addEditVehicle = new AddEditVehicle(car, this);
                addEditVehicle.MdiParent = this.MdiParent;
                addEditVehicle.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        
        private void btnRemoveCar_Click(object sender, EventArgs e)
        {
            try
            {
                // get id of the selected row
                var id = (int)gvVehicleList.SelectedRows[0].Cells["id"].Value;

                // query database for record
                var car = _db.Cars.FirstOrDefault(q => q.id == id);

                // Warning prompt to user
                DialogResult dr = MessageBox.Show(
                    "Are you sure you want to DELETE this car permanently?",
                    "Delete Car",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    // delete vehicle from table
                    _db.Cars.Remove(car);
                    _db.SaveChanges();

                    PopulateGrid();

                    MessageBox.Show("Car deleted successfully!");
                }                
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
            var cars = _db.Cars
                .Select(q => new
                {
                    Make = q.Make,
                    Model = q.Model,
                    VIN = q.VIN,
                    Year = q.Year,
                    LicencePlateNumber = q.LicensePlateNumber,
                    q.id // same as id = q.id
                })
                .ToList();

            gvVehicleList.DataSource = cars;
            gvVehicleList.Columns[4].HeaderText = "Licence Plate Number";
            gvVehicleList.Columns[5].Visible = false;
        }
    }
}
