

namespace wfaCards
{
    internal class ImageBox
    {
        private Bitmap[] images;

        public int Rows { get; }
        public int Cols { get; }
        public int Count { get; }
        public int CardWidth { get; private set; }
        public int CardHeight { get; private set; }

        public ImageBox(Bitmap image, int rows, int columns) : this(image, rows, columns, rows * columns) { }
        public ImageBox(Bitmap image, int rows, int columns, int count)
        {
            Rows = rows;
            Cols = columns;
            Count = count;
            LoadImage(image);
        }

        private void LoadImage(Bitmap image)
        {
            CardWidth = image.Width / Cols;
            CardHeight = image.Height / Rows;
            var n = 0;
            images = new Bitmap[Count];

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols && n < Count; c++, n++)
                {
                    images[n] = new Bitmap(CardWidth, CardHeight);
                    var g = Graphics.FromImage(images[n]);
                    g.DrawImage(image, 0, 0, 
                        new Rectangle(c*CardWidth, r*CardHeight, CardWidth, CardHeight), GraphicsUnit.Pixel);
                    g.Dispose();
                }
            }
        }

        public Bitmap this[int index]
        {
            get { return images[index]; }
        }
    }
}