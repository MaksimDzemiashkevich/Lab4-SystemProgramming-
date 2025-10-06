using System.Text;

namespace Lab4_SystemProgramming_;

public partial class Form1 : Form
{
    private Philosopher[] _philosophers;
    private const int RADIUS = 150;
    private const int CENTER_X = 250;
    private const int CENTER_Y = 250;
    private const int PLATE_RADIUS = 30;
    private Label _labelTimers;
    private static bool[] forks = new bool[5];

    public Form1()
    {
        InitializeComponent();
        this.DoubleBuffered = true;
        MainProgram();
    }

    public void MainProgram()
    {
        _philosophers = new Philosopher[5];
        Philosopher.InitializeSemaphores();
        for (int i = 0; i < _philosophers.Length; i++)
        {
            _philosophers[i] = new Philosopher(i, Environment.TickCount + i, forks);
        }

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        timer.Interval = 100;
        timer.Tick += (s, e) => this.Invalidate();
        timer.Start();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;

        // Рисуем вилки
        PointF p1 = new PointF(120, 60);
        PointF p2 = new PointF(170, 110);
        Pen pen = new Pen(forks[0] ? Color.Red : Color.Black, 5);
        g.DrawLine(pen, p1, p2);

        p1 = new PointF(280, 60);
        p2 = new PointF(230, 110);
        pen = new Pen(forks[1] ? Color.Red : Color.Black, 5);
        g.DrawLine(pen, p1, p2);

        p1 = new PointF(330, 205);
        p2 = new PointF(270, 185);
        pen = new Pen(forks[2] ? Color.Red : Color.Black, 5);
        g.DrawLine(pen, p1, p2);

        p1 = new PointF(200, 280);
        p2 = new PointF(200, 220);
        pen = new Pen(forks[3] ? Color.Red : Color.Black, 5);
        g.DrawLine(pen, p1, p2);

        p1 = new PointF(85, 205);
        p2 = new PointF(145, 185);
        pen = new Pen(forks[4] ? Color.Red : Color.Black, 5);
        g.DrawLine(pen, p1, p2);

        // Рисуем тарелки (философов)
        PointF p = new PointF(200, 50);
        Brush brush = new SolidBrush(_philosophers[0].philosopherState);
        g.FillEllipse(brush, p.X - PLATE_RADIUS, p.Y - PLATE_RADIUS, PLATE_RADIUS * 2, PLATE_RADIUS * 2);

        p = new PointF(300, 140);
        brush = new SolidBrush(_philosophers[1].philosopherState);
        g.FillEllipse(brush, p.X - PLATE_RADIUS, p.Y - PLATE_RADIUS, PLATE_RADIUS * 2, PLATE_RADIUS * 2);

        p = new PointF(265, 250);
        brush = new SolidBrush(_philosophers[2].philosopherState);
        g.FillEllipse(brush, p.X - PLATE_RADIUS, p.Y - PLATE_RADIUS, PLATE_RADIUS * 2, PLATE_RADIUS * 2);

        p = new PointF(135, 250);
        brush = new SolidBrush(_philosophers[3].philosopherState);
        g.FillEllipse(brush, p.X - PLATE_RADIUS, p.Y - PLATE_RADIUS, PLATE_RADIUS * 2, PLATE_RADIUS * 2);

        p = new PointF(100, 140);
        brush = new SolidBrush(_philosophers[4].philosopherState);
        g.FillEllipse(brush, p.X - PLATE_RADIUS, p.Y - PLATE_RADIUS, PLATE_RADIUS * 2, PLATE_RADIUS * 2);

        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < _philosophers.Length; i++)
        {
            stringBuilder.AppendLine($"Philosopher {i} ate {_philosophers[i]._timerDuringEating.Elapsed}");
        }
        if (_labelTimers != null)
        {
            Controls.Remove(_labelTimers);
            _labelTimers = null;
        }
        _labelTimers = CreaterLabel(new Size(400, 200), new Point(50, 300), stringBuilder.ToString());
    }

    private Label CreaterLabel(Size size, Point point, string text)
    {
        Label label = new Label();
        label.Size = size;
        label.Text = text;
        label.Location = point;
        Controls.Add(label);
        return label;
    }
}
