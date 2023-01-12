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
    public partial class AddEditVehicle : Form
    {
        private bool isEditMode;
        private ManageVehicleListing _manageVehicleListing;
        private readonly CarRentalEntities _db;

        public AddEditVehicle(ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Add New Vehicle";
            this.Text = lblTitle.Text;
            isEditMode = false;
            _db = new CarRentalEntities();
            _manageVehicleListing = manageVehicleListing;
        }

        public AddEditVehicle(Car carToEdit, ManageVehicleListing manageVehicleListing = null) 
        {
            InitializeComponent();
            lblTitle.Text = "Edit Vehicle";
            this.Text = lblTitle.Text;            
            _manageVehicleListing = manageVehicleListing;
            
            if (carToEdit == null)
            {
                MessageBox.Show("Please select the whole line to edit");
            }
            else
            {
                isEditMode = true;
                _db = new CarRentalEntities();
                PopulateFields(carToEdit);
            }                      
        }

        private void PopulateFields(Car car)
        {
            lblId.Text = car.id.ToString();
            tbMake.Text = car.Make;
            tbModel.Text = car.Model;
            tbVIN.Text = car.VIN;
            tbYear.Text = car.Year.ToString();
            tbLicencePlate.Text = car.LicensePlateNumber;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // form validation
            int number = 0;
            bool isValid = true;
            string errorMessage = "Error!\n";
            
            if (string.IsNullOrWhiteSpace(tbMake.Text))
            {
                isValid = false;
                errorMessage += "Please enter car make.\n";
            }

            if (string.IsNullOrWhiteSpace(tbModel.Text))
            {
                isValid = false;
                errorMessage += "Please enter car model.\n";
            }

            if (string.IsNullOrWhiteSpace(tbVIN.Text))
            {
                isValid = false;
                errorMessage += "Please enter car VIN.\n";
            }

            if(string.IsNullOrWhiteSpace(tbLicencePlate.Text))
            {
                isValid = false;
                errorMessage += "Please enter car licence plate number.\n";
            }

            if (!int.TryParse(tbYear.Text, out number))
            {
                isValid = false;
                errorMessage += "Year can only be numbers.\n";
            }

            if (isValid)
            {
                // if (isEditMode == true)
                if (isEditMode)
                {
                    try
                    {
                        // edit car code here
                        var id = int.Parse(lblId.Text);
                        var car = _db.Cars.FirstOrDefault(q => q.id == id);
                        car.Model = tbModel.Text;
                        car.Make = tbMake.Text;
                        car.VIN = tbVIN.Text;
                        car.Year = int.Parse(tbYear.Text);
                        car.LicensePlateNumber = tbLicencePlate.Text;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                else
                {
                    try
                    {
                        // add car code here
                        var newCar = new Car
                        {
                            LicensePlateNumber = tbLicencePlate.Text,
                            Make = tbMake.Text,
                            Model = tbModel.Text,
                            VIN = tbVIN.Text,
                            Year = int.Parse(tbYear.Text),
                        };

                        _db.Cars.Add(newCar);                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }                    
                }

                _db.SaveChanges();
                _manageVehicleListing.PopulateGrid();
                this.Close();
                MessageBox.Show("Operation completed successfully.");
            }
            else
            {
                MessageBox.Show(errorMessage);
            }
        }
    }
}
