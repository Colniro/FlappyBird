using Microsoft.VisualBasic;
using Timer = System.Windows.Forms.Timer;
namespace _5_FlappyBird;
public partial class Form1 : Form
{
    const int FormHeight = 800;
    const int FormWidth = 1200;
    bool spaceIsPressed = false;
    Bird player = null;
    Timer tmrMain = new Timer() { Interval = 20, Enabled = false };
    public Form1()
    {
        InitGame();
        tmrMain.Tick += TmrMain_Tick;
        KeyDown += Form1_KeyDown;
        KeyUp += Form1_KeyUp;

    }


    public void InitGame()
    {
        Height = FormHeight;
        Width = FormWidth;
        BackColor = Color.WhiteSmoke;
        player = new Bird(0, 5);
        player.Location = new Point(ClientSize.Width / 4, ClientSize.Height / 2 - Bird._size.Height / 2);
        Controls.Add(player);
        tmrMain.Enabled = true;
    }
    private void Form1_KeyUp(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Space)
        {
            spaceIsPressed = false;
        }
    }

    private void Form1_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Space)
        {
            spaceIsPressed = true;
        }
    }

    private void TmrMain_Tick(object? sender, EventArgs e)
    {
        foreach (var item in MoveableGameElement.gameElements)
        {
            if (spaceIsPressed)
            {
                player.Flap();
            }
            item.Move();
        }
        player._yMovingPixel += 2;
    }
}
