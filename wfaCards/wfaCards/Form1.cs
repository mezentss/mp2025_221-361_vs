namespace wfaCards
{
    public partial class Form1 : Form
    {
        private ImageBox imageBox;
        private Random rnd = new Random();
        private readonly Graphics g;


        public Form1()
        {
            InitializeComponent();

            Bitmap im = new Bitmap(new MemoryStream(Properties.Resources.card_3));

            pictureBox1.Image = new Bitmap(
                Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            g = Graphics.FromImage(pictureBox1.Image);


            imageBox = new ImageBox(im, rows: 5, columns: 13, count: 58);
            this.KeyDown += Form1_KeyDown;
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    DrawRandomCards();
                    break;
                case Keys.F2:
                    DrawVeerCards();
                    break;
                case Keys.F3:
                    break;
            }
        }

        private void DrawVeerCards(int count = 10)
        {
            var _max = Math.Max(imageBox.CardWidth, imageBox.CardHeight);
            var bb = new Bitmap(_max * 3, _max * 3);
            var gg =  Graphics.FromImage(bb);
            gg.TranslateTransform(pictureBox1.Width/2, pictureBox1.Height/2);
            gg.RotateTransform(-15 * (count - 1));
            for (int i = 0; i < count; i++)
            {
                var cardIndex = rnd.Next(imageBox.Count);

                
                gg.DrawImage(imageBox[cardIndex],
                   -imageBox.CardWidth/2,
                   -imageBox.CardHeight);
                gg.RotateTransform(+15);
            }
            bb.RotateFlip(RotateFlipType.Rotate90FlipNone);
            gg.Dispose();

            g.Clear(SystemColors.Control);
            g.DrawImage(bb, new Point(0, 0));


            pictureBox1.Invalidate();
        }

        private void DrawRandomCards(int count = 20)
        {
            g.Clear(SystemColors.Control);
            for (int i = 0; i < count; i++)
            {
                var cardIndex = rnd.Next(imageBox.Count);
                g.DrawImage(imageBox[cardIndex],
                    rnd.Next(this.ClientSize.Width - imageBox[cardIndex].Width),
                    rnd.Next(this.ClientSize.Height - imageBox[cardIndex].Height));
            }
            pictureBox1.Invalidate();
        }
    }
}
