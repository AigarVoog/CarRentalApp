using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp_NET_4._8_learning
{
    public partial class AddEditRentalRecord : Form
    {
        private bool isEditMode;
        private readonly CarRentalEntities _db;
        public AddEditRentalRecord()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            isEditMode = false;
            lblTitle.Text = "Add New Rental Record";
            this.Text = lblTitle.Text;
        }
        public AddEditRentalRecord(CarRentalRecord recordToEdit)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            isEditMode = true;
            lblTitle.Text = "Edit Rental Record";
            this.Text = lblTitle.Text;
            if (recordToEdit == null)
            {
                MessageBox.Show("Please select the whole line to edit");
            }
            else
            {
                PopulateFields(recordToEdit);
            }            
        }

        private void PopulateFields(CarRentalRecord record)
        {            
            lblRecordId.Text = record.id.ToString();
            tbCustomerName.Text = record.CustomerName;
            tbCost.Text = record.Cost.ToString();
            dtpRentalStart.Value = record.DateRented.Value;
            dtpRentalReturn.Value = record.DateReturned.Value;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string customerName = tbCustomerName.Text;
                DateTime rentalDate = dtpRentalStart.Value;
                DateTime returnDate = dtpRentalReturn.Value;
                string car = cbAvailableCars.Text;
                double cost = Convert.ToDouble(tbCost.Text);
                bool isValid = true;
                int number = 0;
                string errorMessage = "Error!\n";

                // form validation
                if (string.IsNullOrWhiteSpace(customerName))
                {
                    isValid = false;
                    errorMessage += "Please enter the customer name.\n";
                }

                if (!int.TryParse(tbCost.Text, out number))
                {
                    isValid = false;
                    errorMessage += "Cost can only be a number.\n";
                }

                if (string.IsNullOrWhiteSpace(car))
                {
                    isValid = false;
                    errorMessage += "Please select a car.\n";
                }

                if (rentalDate > returnDate)
                {
                    isValid = false;
                    errorMessage += "Return date has to be after the rental date.";
                }

                if (isValid) // same as (isvalid == true)
                {
                    // Declare an object of the record to be worked with
                    var rentalRecord = new CarRentalRecord();

                    // edit existing record
                    if (isEditMode)
                    {
                        // If in edit mode, then get the ID and retrieve the record from the database
                        // and place the result in the record object
                        var id = int.Parse(lblRecordId.Text);
                        rentalRecord = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);
                    }

                    // Populate record object with values from the form
                    rentalRecord.CustomerName = customerName;
                    rentalRecord.DateRented = rentalDate;
                    rentalRecord.DateReturned = returnDate;
                    rentalRecord.Cost = (decimal)cost; // Convert.ToDecimal(cost)
                    rentalRecord.CarID = (int)cbAvailableCars.SelectedValue;
                    
                    // If not in edit mode, then add the record object to the database
                    if (!isEditMode)
                    {
                        // pass the form to the database
                        _db.CarRentalRecords.Add(rentalRecord);
                    }

                    // save changes to database
                    _db.SaveChanges();

                    // display message
                    MessageBox.Show($"Customer name is {customerName}! \n" +
                        $"Starting date of rental is {rentalDate}.\n" +
                        $"Return date of rental is {returnDate}.\n" +
                        $"Total cost is {cost}.\n" +
                        $"Rented car is {car}.");

                    Close();
                }
                else
                {
                    MessageBox.Show(errorMessage);
                    //errorMessage = "Error!\n";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // SELECT * FROM Cars
            // var cars = _db.Cars.ToList();

            // Get car id 
            var cars = _db.Cars
                .Select( q => new
                {
                    id = q.id,
                    Name = q.Make + " " + q.Model
                })
                .ToList();

            cbAvailableCars.DisplayMember = "Name";
            cbAvailableCars.ValueMember = "id";
            cbAvailableCars.DataSource = cars;
        }
    }
}
