using System;
using System.Collections.Generic;
using System.Text;

namespace _5_FlappyBird;

public class MoveableGameElement : Panel
{
    public static List<MoveableGameElement> gameElements = new List<MoveableGameElement>();
    public int _xMovingPixel;
    public int _yMovingPixel;
    public static Color _backColor;
    public static Size _size;

    public MoveableGameElement(int xMovingPixel, int yMovingPixel, Color color, Size size)
    {
        _xMovingPixel = xMovingPixel;
        _yMovingPixel = yMovingPixel;

        _backColor = color;
        _size = size;

        gameElements.Add(this);
    }

    public event EventHandler? GameOver;

    public virtual void OnGameOver()
    {
        GameOver?.Invoke(this, EventArgs.Empty);
    }

    public void Move()
    {
        this.Left += _xMovingPixel;
        this.Top += _yMovingPixel;
    }
}