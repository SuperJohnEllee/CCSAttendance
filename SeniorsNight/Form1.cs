using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Deployment.Application;
using MySql.Data.MySqlClient;
using MetroFramework.Forms;

namespace SeniorsNight
{
    public partial class Form1 : MetroForm
    {
        MySqlConnection conn = new MySqlConnection(@"server=localhost;port=3306;username=root;password='';database=ccsregistration");
        MySqlCommand cmd;
        MySqlDataReader rdr;

        string id, idnum, name, course, year, status, date;

        public Form1()
        {
            InitializeComponent();
        }

        public void CountStudents()
        {
            conn.Open();

            cmd = new MySqlCommand(@"SELECT COUNT(ID) FROM students", conn);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                lblStudents.Text = rdr[0].ToString();
            }
            conn.Close();
        }

        public void CountStudentsRegistered()
        {
            conn.Open();

            cmd = new MySqlCommand(@"SELECT COUNT(ID) FROM students WHERE Status = 'Registered'", conn);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                lblStudReg.Text = rdr[0].ToString();
            }

            conn.Close();
        }

        public void CountStudentsUnRegistered()
        {
            conn.Open();

            cmd = new MySqlCommand(@"SELECT COUNT(ID) FROM students WHERE Status = 'Unregistered'", conn);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                lblStudUnreg.Text = rdr[0].ToString();
            }

            conn.Close();
        }

        public void LoadStudents()
        {
            conn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(@"SELECT * FROM students ORDER BY Name ASC", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataStudents.DataSource = dt;
            conn.Close();
        }

        public void LoadStudentsRegistered()
        {
            conn.Open();
            MySqlDataAdapter da2 = new MySqlDataAdapter(@"SELECT * FROM students WHERE Status = 'Registered' ORDER BY RegDate DESC", conn);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataStudRegistered.DataSource = dt2;
            conn.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadStudents();
            LoadStudentsRegistered();
            CountStudents();
            CountStudentsRegistered();
            CountStudentsUnRegistered();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //Nothing
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            conn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(@"SELECT * FROM students WHERE IDNumber LIKE '%"
                +txtSearch.Text+"%' OR Name LIKE '%"+txtSearch.Text+"%'", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataStudents.DataSource = dt;
            conn.Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            conn.Open();

            cmd = new MySqlCommand(@"UPDATE students SET Status = 'Registered', RegDate = NOW() WHERE ID = " + dataStudents.SelectedCells[0].Value.ToString() + " ", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void dataStudents_Click(object sender, EventArgs e)
        {
           /* if (dataStudents.SelectedRows.Count == 1)
            {
                id = dataStudents.SelectedRows[0].Cells[0].Value.ToString();
                idnum = dataStudents.SelectedRows[0].Cells[1].Value.ToString();
                name = dataStudents.SelectedRows[0].Cells[2].Value.ToString();
                course = dataStudents.SelectedRows[0].Cells[3].Value.ToString();
                year = dataStudents.SelectedRows[0].Cells[4].Value.ToString();
                status = dataStudents.SelectedRows[0].Cells[5].Value.ToString();
                date = dataStudents.SelectedRows[0].Cells[6].Value.ToString();
            } */
        }

        private void btnAddStud_Click(object sender, EventArgs e)
        {

            if (txtIDNum.Text != "" || txtName.Text != "" || cmbCourse.Text != "")
            {
                conn.Open();
                cmd = new MySqlCommand(@"INSERT INTO students(IDNumber, Name, Course, Year, Status, RegDate)
                       VALUES('" + txtIDNum.Text + "', '" + txtName.Text + "','" + cmbCourse.Text + "', '4', 'Registered', NOW())", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Add Students Success", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtIDNum.Clear();
                txtName.Clear();
                cmbCourse.ResetText();
            }
            else 
            {
                MessageBox.Show("Input all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataStudents_SelectionChanged(object sender, EventArgs e)
        {
             //Nothing
        }

        private void dataStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           if (dataStudents.SelectedRows.Count == 1)
            {
                id = dataStudents.SelectedRows[0].Cells[0].Value.ToString();
                idnum = dataStudents.SelectedRows[0].Cells[1].Value.ToString();
                name = dataStudents.SelectedRows[0].Cells[2].Value.ToString();
                course = dataStudents.SelectedRows[0].Cells[3].Value.ToString();
                year = dataStudents.SelectedRows[0].Cells[4].Value.ToString();
                status = dataStudents.SelectedRows[0].Cells[5].Value.ToString();
                date = dataStudents.SelectedRows[0].Cells[6].Value.ToString();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Close the Application?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Application.ExitThread();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStudentsRegistered();
            LoadStudents();
            CountStudentsRegistered();
            CountStudentsUnRegistered();
        }
    }
}
