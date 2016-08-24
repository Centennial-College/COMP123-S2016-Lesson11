using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMP123_S2016_Lesson11
{
    public partial class StudentListForm : Form
    {
        public StudentListForm()
        {
            InitializeComponent();
        }

        private void RemoveheaderTextFromButonColumn(string columnString)
        {
            DataGridViewButtonColumn column = (DataGridViewButtonColumn)StudentsDataGridView.Columns[columnString];
            column.HeaderText = string.Empty;
            column.ReadOnly = false;
            if (columnString == "Edit")
            {
                column.CellTemplate.Style.ForeColor = Color.Blue;
            }
            if (columnString == "Delete")
            {
                column.CellTemplate.Style.ForeColor = Color.Red;
            }
        }

        private void StudentListForm_Load(object sender, EventArgs e)
        {
            this.RemoveheaderTextFromButonColumn("Details");
            this.RemoveheaderTextFromButonColumn("Edit");
            this.RemoveheaderTextFromButonColumn("Delete");
        }

        private void AddStudentButton_Click(object sender, EventArgs e)
        {
            StudentDetailsForm addStudentForm = new StudentDetailsForm();
            addStudentForm.studentListForm = this;
            addStudentForm.Show();
            this.Hide();
        }

        private void StudentListForm_Activated(object sender, EventArgs e)
        {
            this.studentsTableAdapter.Fill(this.cOMP123DataSet.Students);

            //StudentDataContext db = new StudentDataContext();
            //List<Student> studentList = (from student in db.Students
            //                             select student).ToList();
            //StudentsDataGridView.DataSource = studentList;
        }

        private void StudentsDataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void StudentsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if header row not clicked and Details, Edit or Delete columns are clicked
            if (e.RowIndex != -1 && e.ColumnIndex > 3)
            {
                // create the new studentDetails form
                StudentDetailsForm StudentDetails = new StudentDetailsForm();
                StudentDetails.studentListForm = this; // make a reference to this form
                StudentDetails.FormType = e.ColumnIndex; // can tell whether clicked on details, edit or delete button

                // get the student id from the StudentsDataGridView
                StudentDetails.StudentID = Convert.ToInt32(StudentsDataGridView.Rows[e.RowIndex].Cells["StudentID"].Value);

                StudentDetails.Show(); // show the studentDetailsForm
                this.Hide(); // hide this form
            }
        }
    }
}
