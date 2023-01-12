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
    public partial class ManageRentalRecords : Form
    {
        private readonly CarRentalEntities _db;
        public ManageRentalRecords()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void btnAddNewRecord_Click(object sender, EventArgs e)
        {
            var addRentalRecord = new AddEditRentalRecord();
            addRentalRecord.MdiParent = this.MdiParent;
            addRentalRecord.Show();

            //new AddRentalRecord
            //{
            //    MdiParent = this.MdiParent
            //}.Show();
        }

        private void btnEditRecord_Click(object sender, EventArgs e)
        {
            try
            {
                // get id of the selected row
                var id = (int)gvRecordList.SelectedRows[0].Cells["id"].Value;

                // query database for record
                var record = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);

                // launch AddRentalRecord form with data
                var addEditRentalRecord = new AddEditRentalRecord(record);
                addEditRentalRecord.MdiParent = this.MdiParent;
                addEditRentalRecord.Show();
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

        private void btnRemoveRecord_Click(object sender, EventArgs e)
        {
            try
            {
                // get id of the selected row
                var id = (int)gvRecordList.SelectedRows[0].Cells["id"].Value;

                // query database for record
                var record = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);

                // Warning prompt to user
                DialogResult dr = MessageBox.Show(
                    "Are you sure you want to DELETE this record permanently?",
                    "Delete Record",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    // delete record from table
                    _db.CarRentalRecords.Remove(record);
                    _db.SaveChanges();

                    PopulateGrid();

                    MessageBox.Show("Record deleted successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ManageRentalRecords_Load(object sender, EventArgs e)
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

        private void PopulateGrid()
        {
            var records = _db.CarRentalRecords
                .Select(q => new
                {
                    q.CustomerName,
                    Car = q.Car.Make + " " + q.Car.Model,
                    q.Cost,
                    q.DateRented,
                    q.DateReturned,
                    q.id // same as id = q.id
                })
                .ToList();

            gvRecordList.DataSource = records;
            gvRecordList.Columns["CustomerName"].HeaderText = "Customer Name";
            gvRecordList.Columns["DateRented"].HeaderText = "Date Rented";
            gvRecordList.Columns["DateReturned"].HeaderText = "Date Returned";
            gvRecordList.Columns["id"].Visible = false;
        }
    }
}
