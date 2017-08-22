using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;

namespace Parser
{

    public partial class Form1 : Form
    {
        public Point ptbeg, ptend;
        public bool draw = false;
        public string selectedDir;
        List<Point> p;
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Paint += new PaintEventHandler(this.pictureBox1_Paint);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            ptbeg = ptend = Point.Empty;
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].HeaderText = "x1";
            dataGridView1.Columns[1].HeaderText = "y1";
            dataGridView1.Columns[2].HeaderText = "x2";
            dataGridView1.Columns[3].HeaderText = "y2";
            dataGridView1.Columns[4].HeaderText = "o";

            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                c.Width = 32;
            }
            dataGridView1.Columns[4].Width = 25;



            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridView1.RowsRemoved += (o, e) => { pictureBox1.Invalidate(); };
            //dataGridView1.DataSource = p;
            string file = "classes.txt";
            comboBox1.Items.Clear();
            if (File.Exists(file))
            {
                var lines = File.ReadAllLines(file);
                foreach (string s in lines)
                {
                    comboBox1.Items.Add(s);
                }
            }
            comboBox1.SelectedIndex = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // dataGridView1.Rows.Add(row6);

            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
                pictureBox1.Image = Image.FromFile(ofd.FileName);

        }

        private void pictureBox1_Paint1(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            // use g to do your drawing
            Color cl = new Color();
            Brush br = new SolidBrush(ForeColor);
            // g.DrawRectangle(new Pen(br),0,0,100,100);
            //alert("aaaa");
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                //   MessageBox.Show(row.Cells[0].Value.ToString());
                int index = Int32.Parse(row.Cells[4].Value.ToString());
                Array colorsArray = Enum.GetValues(typeof(KnownColor));
                KnownColor[] allColors = new KnownColor[colorsArray.Length];

                Array.Copy(colorsArray, allColors, colorsArray.Length);
                //br = new SolidBrush(Color.FromArgb(index == 0 ? 255 : 0, index == 1 ? 255 : 0, index == 2 ? 255 : 0));
                br = new SolidBrush(Color.FromName(allColors[index*3].ToString()));
                float x = Int32.Parse(row.Cells[0].Value.ToString());
                float y = Int32.Parse(row.Cells[1].Value.ToString());
                float w = Int32.Parse(row.Cells[2].Value.ToString());
                float h = Int32.Parse(row.Cells[3].Value.ToString());
                g.DrawRectangle(new Pen(br, 2), x, y, Math.Abs(w - x), Math.Abs(h - y));
                g.DrawString(comboBox1.Items[index].ToString(), new Font(FontFamily.GenericSansSerif,
            12.0F, FontStyle.Bold), br, x, y - 20);
            }

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (keyData == Keys.Enter)
            {
                createFile();
            }
            //else if(keyData == Keys.Down)
            //{
            //    if(comboBox1.Items.Count - 2 > comboBox1.SelectedIndex)
            //        comboBox1.SelectedIndex = comboBox1.SelectedIndex + 1;
            //}
            //else if (keyData == Keys.Up)
            //{
            //    if (0 < comboBox1.SelectedIndex)
            //        comboBox1.SelectedIndex = comboBox1.SelectedIndex - 1;
            //}
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Down(Point a)
        {
            ptbeg = ptend = a;
        }



        private void pictureBox1_MouseMove1(object sender, MouseEventArgs e)
        {
            if (!draw) return;
            Graphics grfx = pictureBox1.CreateGraphics();
            grfx.DrawRectangle(new Pen(BackColor), ptbeg.X, ptbeg.Y,
                ptend.X - ptbeg.X, ptend.Y - ptbeg.Y);
            ptend = new Point(e.X, e.Y);
            pictureBox1.Invalidate();
            grfx.DrawRectangle(new Pen(ForeColor), ptbeg.X, ptbeg.Y,
                ptend.X - ptbeg.X, ptend.Y - ptbeg.Y);
            grfx.Dispose();
        }
        public void alert(object Show)
        {
            MessageBox.Show(Show.ToString());
        }
        public void createFile()
        {
            string fileName = lstFiles.SelectedItem.ToString();
            fileName = fileName.Substring(0, fileName.Length - 3);
            string path = selectedDir + "\\" + fileName + "txt";
            //  MessageBox.Show(path);
            //  return;
            
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(path))
            {
                MessageBox.Show(dataGridView1.Rows.Count.ToString());
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewRow row = dataGridView1.Rows[i];
                    //   MessageBox.Show(row.Cells[0].Value.ToString());
                    double x = Int32.Parse(row.Cells[0].Value.ToString());
                    double y = Int32.Parse(row.Cells[1].Value.ToString());
                    double w = Int32.Parse(row.Cells[2].Value.ToString());
                    double h = Int32.Parse(row.Cells[3].Value.ToString());
                    if (x > w)
                    {
                        double temp = x;
                        x = w;
                        w = temp;
                    }
                    if (y > h)
                    {
                        double temp = y;
                        y = h;
                        h = temp;
                    }
                    // int full_w = pictureBox1.Image.Size.Width;
                    // int full_h = pictureBox1.Image.Size.Height;
                    int full_w = pictureBox1.ClientSize.Width;
                    int full_h = pictureBox1.ClientSize.Height;
                    string line = row.Cells[4].Value.ToString() + " " +
                        ((x + w) / (2 * full_w)).ToString().Replace(",", ".") + " " +
                        ((y + h) / (2 * full_h)).ToString().Replace(",", ".") + " " +
                        ((w - x) / full_w).ToString().Replace(",", ".") + " " +
                        ((h - y) / full_h).ToString().Replace(",", ".")
                        ;
                    // MessageBox.Show(line);
                    //len += info.Length;
                    file.WriteLine(line);
                    // Add some information to the file.
                    //fs.WriteLine(info, len, info.Length);
                    //fs.Write
                }
            }
            if (lstFiles.SelectedIndex < lstFiles.Items.Count)
                lstFiles.SelectedIndex = lstFiles.SelectedIndex + 1;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            createFile();


        }

        private void btnOpenDir_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    selectedDir = fbd.SelectedPath;
                    label1.Text += selectedDir;
                    DirectoryInfo di = new DirectoryInfo(fbd.SelectedPath);
                    foreach (FileInfo fileInfo in di.EnumerateFiles())
                    {
                        if (fileInfo.Extension == ".png" || fileInfo.Extension == ".jpg" || fileInfo.Extension == ".JPG"
                            || fileInfo.Extension == ".jpeg" || fileInfo.Extension == ".JPEG" || fileInfo.Extension == ".PNG")
                        {
                            lstFiles.Items.Add(fileInfo.Name);
                        }
                    }
                }
            }
        }

        private float GetValueFromString(string s)
        {
            return float.Parse(s, System.Globalization.NumberStyles.Number, System.Globalization.NumberFormatInfo.InvariantInfo);
        }


        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            var filePath = selectedDir + "\\" + lstFiles.SelectedItem;

            pictureBox1.Image = Image.FromFile(filePath);
            //pictureBox1.Size = pictureBox1.Image.Size;
            //alert(pictureBox1.Image.Size.Width);
            //alert(pictureBox1.ClientSize.Width);
            dataGridView1.Rows.Clear();
            comboBox1.SelectedIndex = 0;
            
            try {


                string fileName = lstFiles.SelectedItem.ToString();
                fileName = fileName.Substring(0, fileName.Length - 3);
                string path = selectedDir + "\\" + fileName + "txt";

                if (File.Exists(path)) {

                    int full_w = pictureBox1.ClientSize.Width;
                    int full_h = pictureBox1.ClientSize.Height;


                    var lines = File.ReadAllLines(path);
                    foreach (string s in lines) {
                        var items = s.Split(' ');
                        float c1 = GetValueFromString(items[1]);
                        float c2 = GetValueFromString(items[2]);
                        float c3 = GetValueFromString(items[3]);
                        float c4 = GetValueFromString(items[4]);


                        int w = (int)Math.Round((2 * c1 + c3 ) * full_w/2 );
                        int x = (int)Math.Round((2 * c1 - c3 ) * full_w/2);
                        int h = (int)Math.Round((2 * c2 + c4 ) * full_h/2 );
                        int y = (int)Math.Round((2 * c2  - c4 ) * full_h/2 );

                        dataGridView1.Rows.Add(x, y, w, h, Convert.ToInt32(items[0]));

                        if ((((x > full_w) || (w > full_w)) || ((x < 0) || (w < 0)))||
                                (((y > full_h) || (h > full_h)) || ((y < 0) || (h < 0))))
                        {
                            MessageBox.Show("Your selection does not fit picture area.");
                            pictureBox1.Invalidate();
                        }
                         /*
                         x + w = c1 * 2 * full_w
                         w - x = c3*full_w
                         y + h  =2 *c2 * full_h
                         h - y  =c4*full_h
                         */
                    };
                }
            }
            catch(Exception ex)  { MessageBox.Show(ex.Message); }

            label2.Text ="w:" + pictureBox1.ClientSize.Width+
            " x h:" + pictureBox1.ClientSize.Height;

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_MouseUp1(object sender, MouseEventArgs e)
        {
            draw = false;
            
            string[] column = {
                    ptbeg.X.ToString(),
                    ptbeg.Y.ToString(),
                    ptend.X.ToString(),
                    ptend.Y.ToString(),
                    comboBox1.SelectedIndex.ToString()
                };
            dataGridView1.Rows.Add(column);

            pictureBox1.Invalidate();
            ptbeg = new Point(e.X, e.Y);
        }


        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            return;
            if (draw == true)
            {
                string[] column = {
                    ptbeg.X.ToString(),
                    ptbeg.Y.ToString(),
                    ptend.X.ToString(),
                    ptend.Y.ToString(),
                    comboBox1.SelectedIndex.ToString()
                };
                dataGridView1.Rows.Add(column);
            }
            draw = !draw;
            pictureBox1.Invalidate();
            ptbeg = new Point(e.X, e.Y);
        }


        /*****************************************/

        private Point RectStartPoint;
        private Rectangle Rect = new Rectangle();
        private Brush selectionBrush = new SolidBrush(Color.FromArgb(128, 72, 145, 220));

        // Start Rectangle
        //
        private void pictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (pictureBox1.Image == null) return;
            // Determine the initial rectangle coordinates...
            RectStartPoint = e.Location;
            //Invalidate();
        }

        // Draw Rectangle
        //
        private void pictureBox1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (pictureBox1.Image == null) return;
            if (e.Button != MouseButtons.Left)
                return;
            Point tempEndPoint = e.Location;
            Rect.Location = new Point(
                Math.Min(RectStartPoint.X, tempEndPoint.X),
                Math.Min(RectStartPoint.Y, tempEndPoint.Y));
            Rect.Size = new Size(
                Math.Abs(RectStartPoint.X - tempEndPoint.X),
                Math.Abs(RectStartPoint.Y - tempEndPoint.Y));
            pictureBox1.Invalidate();
        }
        // Draw Area
        //
        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            // Draw the rectangle...
            if (pictureBox1.Image != null)
            {
                if (Rect != null && Rect.Width > 0 && Rect.Height > 0)
                {
                    e.Graphics.FillRectangle(selectionBrush, Rect);
                }
                pictureBox1_Paint1(sender, e);
            }
        }



        private void button1_Click_1(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            pictureBox1.Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string startPath = @"c:\example\start";
            string zipPath = @"c:\example\result.zip";
            string extractPath = @"c:\example\extract";
            
            //ZipFile.CreateFromDirectory(startPath, zipPath);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image == null) return;
            if (e.Button == MouseButtons.Left)
            {
                
                var tempEndPoint = e.Location;
                object[] column = {
                    Math.Max(Math.Min(RectStartPoint.X, tempEndPoint.X),0),
                    Math.Max(Math.Min(RectStartPoint.Y, tempEndPoint.Y),0),
                    Math.Min(Math.Max(RectStartPoint.X, tempEndPoint.X),pictureBox1.ClientSize.Width),
                    Math.Min(Math.Max(RectStartPoint.Y, tempEndPoint.Y), pictureBox1.ClientSize.Height),
                    comboBox1.SelectedIndex
                };
                dataGridView1.Rows.Add(column);
                Rect = new Rectangle();
                pictureBox1.Invalidate();

                if ((int)column[0] == 0 && (int)column[1] == 0)
                    MessageBox.Show("Check rectangle at the left top corner. Is it error?");

            }


            if (e.Button == MouseButtons.Right)
            {
                /*int i = dataGridView1.Rows.Count;
                while (i > 0)
                {


                }*/

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewRow row = dataGridView1.Rows[i];
                    int x = Int32.Parse(row.Cells[0].Value.ToString());
                    int y = Int32.Parse(row.Cells[1].Value.ToString());
                    int w = Int32.Parse(row.Cells[2].Value.ToString());
                    int h = Int32.Parse(row.Cells[3].Value.ToString());

                    Rectangle r = new Rectangle(x, y, Math.Abs(w - x), Math.Abs(h - y));
                    if (r.Contains(e.Location))
                    {
                        dataGridView1.Rows.Remove(row);
                        break;
                    }
                }
            }
        }


    }
}

