using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace BT01
{
    public partial class Detail_Form : Form
    {
        public Detail_Form()
        {
            InitializeComponent();
        }

        private DataRow MovieChosen = null;
        private string strFolder = Application.StartupPath + "\\" ;
        private string pathVideo = "";
        private DataTable dataComment = createDataComment();

        public static DataTable createDataComment()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("username", typeof(string));
            dt.Columns.Add("time", typeof(string));
            dt.Columns.Add("comment", typeof(string));

            return dt;
        }
        
        public Detail_Form(DataRow dr)
        {
            InitializeComponent();
            this.MovieChosen = dr;
            Display_Star();
            Display_Detail();
            Disable_FlatAppearance();

            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
        }
        public void Disable_FlatAppearance()
        {
            this.button_like.FlatAppearance.MouseOverBackColor = button_like.BackColor;
            this.button_view.FlatAppearance.MouseOverBackColor = button_view.BackColor;
            this.button_like.FlatAppearance.MouseDownBackColor = button_like.BackColor;
            this.button_view.FlatAppearance.MouseDownBackColor = button_view.BackColor;
        }
        private void Display_Star()
        {
            var index = 1;
            int star = Convert.ToInt32(MovieChosen["star"].ToString());
            System.Drawing.Bitmap starFilled = Properties.Resources._2c4PrbW;
            System.Drawing.Bitmap starHalf = Properties.Resources.tHlBqst;
            foreach (var pictureBox in votesPanel.Controls.OfType<PictureBox>())
            {
                pictureBox.Visible = false;
            }
            foreach (var pictureBox in votesPanel.Controls.OfType<PictureBox>())
            {
                if (index <= star)
                {
                    pictureBox.Image = starFilled;
                }
                else if (star % 10 != 0 && index < star + 1)
                {
                    pictureBox.Image = starHalf;
                }
                else
                {
                    break;
                }
                pictureBox.Visible = true;
                index++;
            }
        }

        private void Display_Detail()
        {
            DataRow dr = MovieChosen;
            string path_img =  dr.Field<string>("poster"); 
            pictureBox_Poster.ImageLocation = Path.Combine(strFolder, path_img);
            string[] name = dr["name"].ToString().Split(new string[] { " - " }, StringSplitOptions.None);

            label_MovieName.Text = name[0];
            label_EnglishName.Text = name[1];

            string newLine = Environment.NewLine;
            textBox_Detail.Text = "Năm sản xuất : " + dr["year"].ToString() + newLine;
            textBox_Detail.Text += "Trạng thái : " + dr["status"].ToString() + newLine;
            textBox_Detail.Text += "Đạo diễn : " + dr["director"].ToString() + newLine;
            textBox_Detail.Text += "Thời gian : " + dr["duration"].ToString() + " phút " + newLine;
            string[] gernes = dr["gernes"].ToString().Split('_');
            textBox_Detail.Text += "Thể loại: " + newLine;
            for (int i = 0; i < gernes.Length - 1; i++)
            {
                textBox_Detail.Text += gernes[i] + ',' + ' ';
            }
            textBox_Detail.Text += gernes[gernes.Length - 1] + newLine;

            textBox_Descrip.Text = File.ReadAllText(Path.Combine(strFolder, dr.Field<string>("decrip")));

            button_like.Text = dr["like"].ToString();
            button_view.Text = dr["view"].ToString();

            pathVideo = Path.Combine(strFolder, dr.Field<string>("trailer"));
        }
        
        private void Stop_media(object sender, FormClosingEventArgs e)
        {
            // axWindowsMediaPlayer1.close();
        }

        private void Show_Comment()
        {
            int index = 0;
            foreach (var pan in tableLayoutPanel1.Controls.OfType<Panel>())
            {
                pan.Visible = false;

                if (index + 1 > dataComment.Select().Length)
                {
                    continue;
                }

                pan.Visible = true;

                DataRow dr = dataComment.Rows[index];
                Label name = pan.Controls.OfType<Label>().Last();
                Label time = pan.Controls.OfType<Label>().First();
                Console.WriteLine(dr["comment"].ToString());
                RichTextBox comment = pan.Controls.OfType<RichTextBox>().First();
                
                name.Text = dr["username"].ToString();
                time.Text = dr["time"].ToString();
                comment.Text = dr["comment"].ToString();
                index++;
            }
        }

        private void button_play_Click(object sender, EventArgs e)
        {
            Movie_Player movie_Player = new Movie_Player(pathVideo);
            movie_Player.ShowDialog();
        }

        private void commentBtn_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(username.Text) && !String.IsNullOrEmpty(commentBox.Text))
            {
                string name = username.Text;
                string comment = commentBox.Text;
                string date = DateTime.Now.ToString("dd/MM/yy");
                string time = DateTime.Now.ToString("HH:mm");

                string timeComment = time + " - " + date;

                dataComment.Rows.Add(name, timeComment, comment);

                Show_Comment();
            } else
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
            }
        }
    }
}
