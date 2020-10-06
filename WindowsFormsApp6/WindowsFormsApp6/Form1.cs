using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        int rowNum = 0;
        public Form1()
        {
            InitializeComponent();
            fillDrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sqlString = "";
            if (rowNum == 0)
            {


                sqlString = @"INSERT INTO [dbo].[Fornt_Store]
           ([store_name]
           ,[phone]
           ,[email]
           ,[street]
           ,[city]
           ,[state]
           ,[zip_code]
           ,[photo])
            VALUES
           (@store_name
           ,@phone
           ,@email
           ,@street
           ,@city
           ,@state
           ,@zip_code
           ,@photo)";
            }
            else
            {

                sqlString = @"UPDATE [dbo].[Fornt_Store]
   SET [store_name] = @store_name
      ,[phone] = @phone 
      ,[email] = @email
      ,[street] = @street
      ,[city] = @city
      ,[state] = @state
      ,[zip_code] = @zip_code
      ,[photo] = @photo
 WHERE  store_id =@store_id ";


            }



            using (OpenFileDialog ofd = new OpenFileDialog() { })
            {
                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    SqlConnection scon = new SqlConnection(ConfigurationManager.ConnectionStrings[0].ConnectionString);
                    scon.Open();
                    SqlCommand cmd = new SqlCommand(sqlString, scon);
                    cmd.CommandType = CommandType.Text;
                   
                    cmd.Parameters.Add("@store_name", SqlDbType.VarChar).Value = txtStoreName.Text;
                    cmd.Parameters.Add("@phone", SqlDbType.VarChar).Value = txtPhone.Text;
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = txtEmail.Text;
                    cmd.Parameters.Add("@street", SqlDbType.VarChar).Value = txtstreet.Text;
                    cmd.Parameters.Add("@city", SqlDbType.VarChar).Value = txtCity.Text;
                    cmd.Parameters.Add("@state", SqlDbType.VarChar).Value = txtstate.Text;
                    cmd.Parameters.Add("@zip_code", SqlDbType.VarChar).Value = txtZip_code.Text;
                    if(rowNum != 0)
                    {
                        cmd.Parameters.Add("@store_id", SqlDbType.Int).Value = rowNum;
                    }
                   

                    pictureBox1.Image = new Bitmap(ofd.FileName);
                    ImageConverter ic = new ImageConverter();
                    byte[] imagebyte = (byte[])ic.ConvertTo(pictureBox1.Image, typeof(byte[]));
                    cmd.Parameters.Add("@photo", SqlDbType.VarBinary).Value = imagebyte;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    fillDrid();

                    rowNum = 0;


                    //DataGridViewImageColumn icCol = new DataGridViewImageColumn();
                    //icCol.HeaderText = "Img";
                    //icCol.Image = null;
                    //icCol.Name = "cImg";
                    //icCol.Width = 100;
                    //dataGridView1.Columns.Add(icCol);

                    //foreach (DataGridViewRow row in dataGridView1.Rows)
                    //{
                    //    try
                    //    {
                    //        DataGridViewImageCell cell = row.Cells[row.Cells.Count - 2] as DataGridViewImageCell;
                    //        cell.Value = getImageFromByte(row);
                    //    }catch(Exception ex)
                    //    {

                    //    }
                    //}

                }

            }


        }

        private void fillDrid()
        {
            SqlConnection scon1 = new SqlConnection(ConfigurationManager.ConnectionStrings[0].ConnectionString);
            scon1.Open();
            SqlCommand cmd1 = new SqlCommand(@"SELECT [store_id]
                   ,[store_name]
                   ,[phone]
                  ,[email]
                   ,[street]
                   ,[city]
                  ,[state]
                  ,[zip_code]
                   ,[photo]
                   FROM [dbo].[Fornt_Store]", scon1);
            cmd1.CommandType = CommandType.Text;

            SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
            ds.Clear();
            sda1.Fill(ds);

            dataGridView1.DataSource = ds.Tables[0];
        }

        //private Image getImageFromByte(DataGridViewRow dr)
        //{


        //    var byteArray =  (byte[])dr.Cells["photo"].Value;
        //    MemoryStream ms = new MemoryStream(byteArray);
        //    return Image.FromStream(ms);

        //}

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;
            try
            {
                rowNum = Convert.ToInt32(dt.Rows[e.RowIndex]["store_id"].ToString());
            }
            catch (Exception ex)
            {
                rowNum = 0;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sqlString = "";
            if (rowNum != 0)
            {
                sqlString = @"delete from  [dbo].[Fornt_Store] where store_id=" + rowNum.ToString();

                SqlConnection scon1 = new SqlConnection(ConfigurationManager.ConnectionStrings[0].ConnectionString);
                scon1.Open();
                SqlCommand cmd1 = new SqlCommand(sqlString, scon1);
                cmd1.CommandType = CommandType.Text;

                SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                ds.Clear();
                sda1.Fill(ds);

                fillDrid();
                rowNum = 0;

            }
        }
    }
}
