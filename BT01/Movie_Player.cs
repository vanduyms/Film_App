using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT01
{
    public partial class Movie_Player : Form
    {
        public Movie_Player()
        {
            InitializeComponent();
        }

        public Movie_Player(string path_movie)
        {
            InitializeComponent();
            axWindowsMediaPlayer1.URL = path_movie;
        }

        private void Stop_media(object sender, FormClosingEventArgs e)
        {
            axWindowsMediaPlayer1.close();
        }

        private void Movie_Player_Load(object sender, EventArgs e)
        {

        }
    }
}
