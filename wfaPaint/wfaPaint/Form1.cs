using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using wfaPaint.Managers;
using wfaPaint.Drawing;
using wfaPaint.Shape;

namespace wfaPaint
{
    public partial class Form1 : Form
    {
        private DrawingManager drawingManager;
        private ColorManager colorManager;
        private ShapeManager shapeManager;
        private ImageManager imageManager;

        public Form1()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;

            colorManager = new ColorManager(panel2, panel3, panel4, panel5, panel6, panel7, panel8, panel9);
            colorManager.ColorChanged += (color) => pnlSelectedColor.BackColor = color;

            drawingManager = new DrawingManager(pxImage, colorManager);
            shapeManager = new ShapeManager(drawingManager);
            imageManager = new ImageManager(pxImage);

            InitializeControls();
            UpdateShapeToolStatus(DrawingTool.Pencil);

            pxImage.MouseMove += PxImage_MouseMove;
        }

        private void InitializeControls()
        {
            button1.Click += (s, e) =>
            {
                shapeManager.CurrentTool = DrawingTool.Pencil;
                UpdateShapeToolStatus(DrawingTool.Pencil);
            };
            button2.Click += (s, e) =>
            {
                shapeManager.CurrentTool = DrawingTool.Line;
                UpdateShapeToolStatus(DrawingTool.Line);
            };
            button3.Click += (s, e) =>
            {
                shapeManager.CurrentTool = DrawingTool.Ellipse;
                UpdateShapeToolStatus(DrawingTool.Ellipse);
            };
            button4.Click += (s, e) =>
            {
                shapeManager.CurrentTool = DrawingTool.Rectangle;
                UpdateShapeToolStatus(DrawingTool.Rectangle);
            };
            button5.Click += (s, e) =>
            {
                shapeManager.CurrentTool = DrawingTool.Triangle;
                UpdateShapeToolStatus(DrawingTool.Triangle);
            };
            buttonArrow.Click += (s, e) =>
            {
                shapeManager.CurrentTool = DrawingTool.Arrow;
                UpdateShapeToolStatus(DrawingTool.Arrow);
            };
            buttonStar.Click += (s, e) =>
            {
                shapeManager.CurrentTool = DrawingTool.Star;
                UpdateShapeToolStatus(DrawingTool.Star);
            };
            buttonHexagon.Click += (s, e) =>
            {
                shapeManager.CurrentTool = DrawingTool.Hexagon;
                UpdateShapeToolStatus(DrawingTool.Hexagon);
            };
            buttonDiamond.Click += (s, e) =>
            {
                shapeManager.CurrentTool = DrawingTool.Diamond;
                UpdateShapeToolStatus(DrawingTool.Diamond);
            };
            buttonPentagon.Click += (s, e) =>
            {
                shapeManager.CurrentTool = DrawingTool.Pentagon;
                UpdateShapeToolStatus(DrawingTool.Pentagon);
            };
            buttonHeart.Click += (s, e) =>
            {
                shapeManager.CurrentTool = DrawingTool.Heart;
                UpdateShapeToolStatus(DrawingTool.Heart);
            };
            buttonSpeech.Click += (s, e) =>
            {
                shapeManager.CurrentTool = DrawingTool.SpeechBubble;
                UpdateShapeToolStatus(DrawingTool.SpeechBubble);
            };
            buttonText.Click += (s, e) =>
            {
                shapeManager.CurrentTool = DrawingTool.Text;
                UpdateShapeToolStatus(DrawingTool.Text);
                ShowTextInputDialog();
            };
            buttonSelection.Click += (s, e) =>
            {
                shapeManager.CurrentTool = DrawingTool.Selection;
                UpdateShapeToolStatus(DrawingTool.Selection);
            };

            btnEraser.Click += (s, e) =>
            {
                shapeManager.CurrentTool = DrawingTool.Eraser;
                UpdateShapeToolStatus(DrawingTool.Eraser);
            };

            trPenWidth.Minimum = 1;
            trPenWidth.Maximum = 12;
            trPenWidth.Value = 5;
            trPenWidth.ValueChanged += (s, e) =>
            {
                drawingManager.SetPenWidth(trPenWidth.Value);
                if (shapeManager.CurrentTool == DrawingTool.Eraser)
                    shapeManager.UpdateEraserWidth(trPenWidth.Value);
            };

            btnColorPicker.Click += (s, e) =>
            {
                colorPicker.FullOpen = true;
                colorPicker.AnyColor = true;

                if (colorPicker.ShowDialog() == DialogResult.OK)
                {
                    colorManager.SetCustomColor(colorPicker.Color);
                    pnlSelectedColor.BackColor = colorPicker.Color;
                }
            };

            saveAsMenuItem.Click += (s, e) => imageManager.SaveImageAs();

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
        }

        private void PxImage_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateCursorInfo(e.Location);
        }

        private void UpdateShapeToolStatus(DrawingTool tool)
        {
            statusImageInfo.Text = $"Выбранная фигура: {GetToolName(tool)}";
        }

        private string GetToolName(DrawingTool tool)
        {
            return tool switch
            {
                DrawingTool.Pencil => "Карандаш",
                DrawingTool.Line => "Линия",
                DrawingTool.Ellipse => "Элипс",
                DrawingTool.Rectangle => "Прямоугольник",
                DrawingTool.Triangle => "Треугольник",
                DrawingTool.Arrow => "Стрелка",
                DrawingTool.Star => "Звезда",
                DrawingTool.Hexagon => "Хексагон",
                DrawingTool.Diamond => "Ромб",
                DrawingTool.Pentagon => "Шестиугольник",
                DrawingTool.Heart => "Сердце",
                DrawingTool.SpeechBubble => "Диалог",
                DrawingTool.Text => "Текст",
                DrawingTool.Selection => "Выделение",
                DrawingTool.Eraser => "Ластик",
                _ => "Неизвестно"
            };
        }

        private void UpdateCursorInfo(Point location)
        {
            Color pixelColor = drawingManager.GetPixelColor(location);
            statusCursorColor.Text = $"Координаты: X:{location.X}, Y:{location.Y} | Цвет: R:{pixelColor.R} G:{pixelColor.G} B:{pixelColor.B}";
        }

        private void ShowTextInputDialog()
        {
            Font selectedFont = new Font("Arial", 16);

            Form textInputForm = new Form
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Введите текст",
                StartPosition = FormStartPosition.CenterParent
            };

            TextBox textBox = new TextBox
            {
                Left = 10,
                Top = 10,
                Width = 465,
                Height = 50,
                Multiline = true
            };

            Button fontSettingsButton = new Button
            {
                Text = "Настройки шрифта",
                Left = 10,
                Top = 70,
                Width = 150,
                Height = 30
            };

            Button confirmButton = new Button
            {
                Text = "ОК",
                Left = 320,
                Top = 70,
                Width = 50,
                Height = 30,
                DialogResult = DialogResult.OK
            };

            Button cancelButton = new Button
            {
                Text = "Отмена",
                Left = 375,
                Top = 70,
                Width = 100,
                Height = 30,
                DialogResult = DialogResult.Cancel
            };

            fontSettingsButton.Click += (s, e) =>
            {
                fontDialog.ShowColor = false;
                fontDialog.ShowEffects = true;
                fontDialog.ShowApply = false;
                fontDialog.ShowHelp = false;
                fontDialog.Font = selectedFont;

                if (fontDialog.ShowDialog(textInputForm) == DialogResult.OK)
                {
                    selectedFont = fontDialog.Font;
                }
            };

            textInputForm.Controls.Add(textBox);
            textInputForm.Controls.Add(fontSettingsButton);
            textInputForm.Controls.Add(confirmButton);
            textInputForm.Controls.Add(cancelButton);
            textInputForm.AcceptButton = confirmButton;
            textInputForm.CancelButton = cancelButton;

            if (textInputForm.ShowDialog() == DialogResult.OK)
            {
                string text = textBox.Text;
                if (!string.IsNullOrEmpty(text))
                {
                    shapeManager.setTextFont(selectedFont);
                    shapeManager.setTextToDraw(text);
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Z:
                        drawingManager.Undo();
                        break;
                    case Keys.Y:
                        drawingManager.Redo();
                        break;
                    case Keys.C:
                        if (shapeManager.CurrentTool == DrawingTool.Selection)
                        {
                            shapeManager.GetCurrentDrawer().ToSelection().CopySelection();
                        }
                        e.Handled = true;
                        break;
                    case Keys.V:

                        Point cursorPosition = pxImage.PointToClient(Cursor.Position);

                        drawingManager.PasteFromClipboard(cursorPosition);

                        drawingManager.SaveTemporaryState();
                        e.Handled = true;
                        break;
                }
            } else
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        if (shapeManager.CurrentTool == DrawingTool.Selection)
                        {
                            SelectionDrawer selection = shapeManager.GetCurrentDrawer().ToSelection();
                            selection.ClearSelection();

                            selection.DrawFillRect(selection.SelectionRect);
                        }
                        break;
                }
            }
            }
    }
}
