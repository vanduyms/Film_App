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
    public partial class Form1 : Form
    {
        private string strFolder = Application.StartupPath + "\\";
        private DataTable dt_ListMovie = GetMovieDataTable();
        private int curChosen_idx = -1;
        private bool liked = false;

        private int numOfIndex = 0;
        private int indexT = 0;
        private int indexS = 0;
        public Form1()
        {
            InitializeComponent();
            Disable_FlatAppearance();
            Show_to_ListMovie(indexT, indexS);
            LichSu_Init();
            YeuThich_Init();
        }
        
        public void Disable_FlatAppearance()
        {
            this.button_like.FlatAppearance.MouseOverBackColor = button_like.BackColor;
            this.button_star.FlatAppearance.MouseOverBackColor = button_star.BackColor;
            this.button_view.FlatAppearance.MouseOverBackColor = button_view.BackColor;
            this.button_like.FlatAppearance.MouseDownBackColor = button_like.BackColor;
            this.button_star.FlatAppearance.MouseDownBackColor = button_star.BackColor;
            this.button_view.FlatAppearance.MouseDownBackColor = button_view.BackColor;

            backBtn.Visible = false;
            nextBtn.Visible = false;
        }

        // Get-functions
        public static  DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(new string[] {","}, StringSplitOptions.None);
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(new string[] { "," }, StringSplitOptions.None);
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }

            return dt;
        }
        private static DataTable GetMovieDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = ConvertCSVtoDataTable(Application.StartupPath + "\\Data\\MovieInfo.csv");
            
            return dataTable;
        }

        private void Show_to_ListMovie(int idT, int idS)
        {
            var index = idT;
            numOfIndex = dt_ListMovie.Rows.Count;

            if (numOfIndex > 10)
            {
                nextBtn.Visible = true;
            }
            foreach (var pan in tableLayoutPanel4.Controls.OfType<Panel>()) 
            {
                pan.Visible = false;

                if (index + 1 > dt_ListMovie.Select().Length) 
                {
                    continue; 
                }

                pan.Visible = true; 

                DataRow dr = dt_ListMovie.Rows[index]; 
                pan.Name = dr["ID"].ToString();
                string path_img = Application.StartupPath + "\\" + dr.Field<string>(3); 
                PictureBox pb = pan.Controls.OfType<PictureBox>().First();
                pb.ImageLocation = Path.Combine(strFolder,path_img);
                Label label_name = pan.Controls.OfType<Label>().First();
                label_name.Text = dr["name"].ToString();

                index++;
            }
            indexS = idT + 1;
        }
        private void Show_Finding()
        {
            var index = 0;
            string keyword = textBox1.Text;
            DataRow[] dt_find = dt_ListMovie.Select("name Like '%" + keyword + "%'");

            if (dt_find.Length > 10)
            {
                nextBtn.Visible = true;
            } else
            {
                nextBtn.Visible=false;
                backBtn.Visible = false;
            }
            if (dt_find.Length == 0) { MessageBox.Show("Không tìm thấy phim", "ERROR");}
            foreach (var pan in tableLayoutPanel4.Controls.OfType<Panel>())
            {
                pan.Visible = false; 

                if (index + 1 > dt_find.Length)
                {
                    continue; 
                }

                pan.Visible = true; 

                DataRow dr = dt_find[index]; 
                pan.Name = dr["ID"].ToString();
                string path_img = dr.Field<string>(3); 

                PictureBox pb = pan.Controls.OfType<PictureBox>().First();
                pb.ImageLocation = Path.Combine(strFolder, path_img);
                Label label_name = pan.Controls.OfType<Label>().First();
                label_name.Text = dr["name"].ToString();

                index++;
            }
        }
        private void Show_danhmuc()
        {
            var index = 0;
            
            string keyword = genreFilms.Text;
            string nation = nationFilms.Text;

            if (String.Equals(keyword, "Tất cả")) keyword = "";
            if (String.Equals(nation, "Tất cả")) nation = "";

            if (String.Equals(keyword, "Tất cả") && String.Equals(nation, "Tất cả"))
            {
                nextBtn.Visible = false;
                backBtn.Visible = false;
            } else
            {
                nextBtn.Visible = true;
                backBtn.Visible = true;
            }

            DataRow[] dt_find = dt_ListMovie.Select("gernes Like '%" + keyword + "%' AND nation Like '%" + nation + "%'");

            foreach (var pan in tableLayoutPanel4.Controls.OfType<Panel>()) 
            {
                pan.Visible = false; 

                if (index + 1 > dt_find.Length) 
                {
                    continue; 
                }

                pan.Visible = true;

                DataRow dr = dt_find[index]; 
                pan.Name = dr["ID"].ToString();
                string path_img = dr.Field<string>(3); 

                PictureBox pb = pan.Controls.OfType<PictureBox>().First();
                pb.ImageLocation = Path.Combine(strFolder, path_img);
                Label label_name = pan.Controls.OfType<Label>().First();
                label_name.Text = dr["name"].ToString();

                index++;
            }
        }

        private void Show_lichsu()
        {
            nextBtn.Visible = false;
            backBtn.Visible = false;

            var index = 0;
            DataView dt_view = new DataView(LichSu);
            DataTable dt_ls = dt_view.ToTable(true, "id");
            foreach (var pan in tableLayoutPanel4.Controls.OfType<Panel>()) 
            {
                pan.Visible = false; 

                if (index + 1 > dt_ls.Select().Length) 
                {
                    continue; 
                }

                pan.Visible = true;

                DataRow temp_dr = dt_ls.Rows[index];
                int idx = temp_dr.Field<int>(0);
                DataRow dr = dt_ListMovie.Rows[idx];
                pan.Name = dr["ID"].ToString();
                string path_img = dr.Field<string>(3); 

                PictureBox pb = pan.Controls.OfType<PictureBox>().First();
                pb.ImageLocation = Path.Combine(strFolder, path_img);
                Label label_name = pan.Controls.OfType<Label>().First();
                DateTime timer = LichSu.Select("id = " + idx.ToString()).Last().Field<DateTime>(1);
                label_name.Text = LichSu.Select("id = "+idx.ToString()).Length + "-" + timer.ToString("MM/dd/yy H:mm:ss");

                index++;
            }
        }

        private void Show_Favorite()
        {
            nextBtn.Visible = false;
            backBtn.Visible = false;

            var index = 0;
            DataView dt_view = new DataView(YeuThich);
            DataTable dt_ls = dt_view.ToTable(true, "id");
            foreach (var pan in tableLayoutPanel4.Controls.OfType<Panel>()) 
            {
                pan.Visible = false; 

                if (index + 1 > dt_ls.Select().Length) 
                {
                    continue; 
                }

                pan.Visible = true; 

                DataRow temp_dr = dt_ls.Rows[index];
                int idx = temp_dr.Field<int>(0);
                DataRow dr = dt_ListMovie.Rows[idx]; 
                pan.Name = dr["ID"].ToString();
                string name = dr["Name"].ToString();
                string path_img = dr.Field<string>(3); 

                PictureBox pb = pan.Controls.OfType<PictureBox>().First();
                pb.ImageLocation = Path.Combine(strFolder, path_img);
                Label label_name = pan.Controls.OfType<Label>().First();
                label_name.Text = name;
                index++;
            }
        }

        private void panel_inListMovie_Click(object sender, EventArgs e)
        {
            MoreDetail_update( (Panel)sender );
        }
        private void panelChild_inListMovie_Click(object sender, EventArgs e)
        {
            MoreDetail_update( (Panel)((Control)sender).Parent );
        }

        private void button_play_Click(object sender, EventArgs e)
        {
            if (curChosen_idx == -1) { pop_up_NoMovieMsg(); return; }
            string path_vid =  dt_ListMovie.Rows[curChosen_idx].Field<string>(4);
            string path_full = Path.Combine(strFolder, path_vid);

            int view_count = Int32.Parse(button_view.Text);
            view_count++;
            button_view.Text = view_count.ToString();
            dt_ListMovie.Rows[curChosen_idx]["view"] = view_count.ToString();

            LichSu.Rows.Add(curChosen_idx, DateTime.Now);

            Movie_Player movie_Player = new Movie_Player(path_full);
            movie_Player.ShowDialog();
        }

        private void button_detail_Click(object sender, EventArgs e)
        {
            if (curChosen_idx == -1) { pop_up_NoMovieMsg(); return; }
            Detail_Form detail_form = new Detail_Form(dt_ListMovie.Rows[curChosen_idx]);
            detail_form.ShowDialog();
        }

        private void MoreDetail_update(Panel pan)
        {
            curChosen_idx = Int32.Parse(pan.Name);

            DataRow dr = dt_ListMovie.Rows[curChosen_idx]; 
            string path_img = dr.Field<string>(3); 

            pictureBox_Poster.ImageLocation = Path.Combine(strFolder, path_img);
            label_MovieName.Text = dr["name"].ToString();

            string newLine = Environment.NewLine;
            textBox_Detail.Text = "Năm sản xuất : " + dr["year"].ToString() + newLine;
            textBox_Detail.Text += "Trạng thái : " + dr["status"].ToString() + newLine;
            textBox_Detail.Text += "Đạo diễn : " + dr["director"].ToString() + newLine;
            textBox_Detail.Text += "Thời gian : " + dr["duration"].ToString() + newLine;
            string[] gernes = dr["gernes"].ToString().Split('/');
            textBox_Detail.Text += "Thể loại: ";
            for (int i = 0; i < gernes.Length-1; i++)
            {
                textBox_Detail.Text += gernes[i] + ','+' ';
            }
            textBox_Detail.Text += gernes[gernes.Length - 1] + newLine;

            button_like.Text = dr["like"].ToString();
            button_view.Text = dr["view"].ToString();
            button_star.Text = "Star: " + dr["star"].ToString();
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            string message = "Do you want to close this window?";
            string title = "Close Window";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button_like_Click(object sender, EventArgs e)
        {
            if (curChosen_idx == -1) { pop_up_NoMovieMsg(); return; }
            if (liked == false)
            {
                int like_count = Int32.Parse(button_like.Text);
                like_count++;
                button_like.Text = like_count.ToString();
                liked = true;
            }
            else
            {
                int like_count = Int32.Parse(button_like.Text);
                like_count--;
                button_like.Text = like_count.ToString();
                liked = false;
            }
        }

        private void pop_up_NoMovieMsg()
        {
            string message = "Xin hãy chọn phim bạn muốn";
            string title = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBox.Show(message, title, buttons);
        }

        private void panel_NavBar_Click(object sender, EventArgs e)
        {
        }

        private DataTable LichSu = new DataTable();
        private DataTable YeuThich = new DataTable();

        private void LichSu_Init()
        {
            LichSu.Columns.Add("id", typeof(int));
            LichSu.Columns.Add("last", typeof(DateTime));
        }
        private void YeuThich_Init()
        {
            YeuThich.Columns.Add("id", typeof(int));
        }

        private void LichSu_Click(object sender, EventArgs e)
        {
            Show_lichsu();
        }

        private void button_finding_Click(object sender, EventArgs e)
        {
            Show_Finding();
        }

        private void button_danhmuc_Click(object sender, EventArgs e)
        {
            Show_danhmuc();
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            indexT += 10;
            backBtn.Visible = true;
            if (indexT > numOfIndex)
            {
                indexT = 0;
                nextBtn.Visible = false;
            }
            Show_to_ListMovie(indexT,indexS);
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            indexT -= 10;
            if (indexT < 0)
            {
                indexT = 0;
                backBtn.Visible = false;
            }
            Show_to_ListMovie(indexT, indexS);
        }

        private void addLove_Btn_Click(object sender, EventArgs e)
        {
            if (curChosen_idx == -1) { pop_up_NoMovieMsg(); return; }

            YeuThich.Rows.Add(curChosen_idx);

            string message = "Đã thêm vào danh sách yêu thích";
            MessageBox.Show(message);
        }

        private void favoriteList_Btn_Click(object sender, EventArgs e)
        {
            Show_Favorite();
        }

        private void logo_Click(object sender, EventArgs e)
        {
            Show_to_ListMovie(indexT, indexS);
        }
    }
}
