using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMP123_S2016_Lesson11
{
    public partial class StudentDetailsForm : Form
    {
        // Public Properties
        public StudentListForm studentListForm { get; set; } // references previous form
        public int FormType { get; set; } // what type of form do I need?
        public int StudentID { get; set; } // what is the studentID of the row clicked?
        public bool HasEdits { get; set; }


        public StudentDetailsForm()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            // creates a reference to the database
            StudentDataContext db = new StudentDataContext();

            Student newStudent = new Student();

            // check if the form type is Details, Edit or Delete
            if (this.FormType > 3)
            {
                newStudent = (from student in db.Students
                              where student.StudentID == this.StudentID
                              select student).FirstOrDefault();
            }

            // copy data into Student Object from form Text Boxes
            newStudent.FirstName = FirstNameTextBox.Text;
            newStudent.LastName = LastNameTextBox.Text;
            newStudent.Number = StudentNumberTextBox.Text;

            // check if form Type is "Add Student"
            if (this.FormType < 4)
            {
                // Insert the new Student Object into the SQL Database
                db.GetTable<Student>().InsertOnSubmit(newStudent);
            }

            // Delete Record
            if (this.FormType == (int)ColumnButton.Delete)
            {
                // confirm if the user wants to delete the record
                DialogResult result = MessageBox.Show("Are you sure?", "Confirm Deletion", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    db.GetTable<Student>().DeleteOnSubmit(newStudent);
                }
            }

            // Save changes / update record
            db.SubmitChanges();

            // show the Student List Form
            this.studentListForm.Show();

            // close this form
            this.Close();
        }

        private DialogResult CancelConfirmation()
        {
            if (this.FormType == 5 && this.HasEdits)
            {
                return MessageBox.Show("Are you sure you want to cancel your edits?", "Cancel Confirmation", MessageBoxButtons.OKCancel);
            }
            else
            {
                return DialogResult.OK;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StudentDetailsForm_Load(object sender, EventArgs e)
        {
            // create db object
            StudentDataContext db = new StudentDataContext();

            // check to ensure that you are asking for Details Form, Edit Form or a Delete Form
            if (this.FormType > 3)
            {
                Student studentDetails = (from student in db.Students
                                          where student.StudentID == this.StudentID
                                          select student).FirstOrDefault();

                // Display details in the Text Boxes of the Form
                FirstNameTextBox.Text = studentDetails.FirstName;
                LastNameTextBox.Text = studentDetails.LastName;
                StudentNumberTextBox.Text = studentDetails.Number;

                // only detect FormTextBox being edited after this
                this.HasEdits = false;
            }

            switch (this.FormType)
            {
                case (int)ColumnButton.Details:
                    NewStudentLabel.Text = "Student Details";
                    this.Text = "Student Details";
                    SubmitButton.Visible = false;
                    FirstNameTextBox.ReadOnly = true;
                    LastNameTextBox.ReadOnly = true;
                    StudentNumberTextBox.ReadOnly = true;
                    CancelButton.Text = "Back";
                    break;
                case (int)ColumnButton.Edit:
                    NewStudentLabel.Text = "Update Student";
                    this.Text = "Update Student";
                    SubmitButton.Visible = true;
                    SubmitButton.Text = "Update";
                    break;
                case (int)ColumnButton.Delete:
                    NewStudentLabel.Text = "Delete Student";
                    this.Text = "Delete Student";
                    FirstNameTextBox.ReadOnly = true;
                    LastNameTextBox.ReadOnly = true;
                    StudentNumberTextBox.ReadOnly = true;
                    SubmitButton.Text = "Delete";
                    SubmitButton.BackColor = Color.Red;
                    break;
            }
        }

        private void StudentDetailsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CancelConfirmation() == DialogResult.OK)
            {
                this.studentListForm.Show();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void FormTextBox_TextChanged(object sender, EventArgs e)
        {
            this.HasEdits = true;
        }
    }
}
