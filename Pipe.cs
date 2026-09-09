using System;
using System.Collections.Generic;
using System.Text;

namespace _5_FlappyBird;

public class Pipe : MoveableGameElement
{
    public Pipe(int xMovingPixel, int yMovingPixel, Color color, Size size) : base(xMovingPixel, yMovingPixel, color, size)
    {
        _backColor = color;
        _size = size;
        this.BackColor = _backColor;
        this.Size = _size;

        _xMovingPixel = xMovingPixel;
        _yMovingPixel = 0;
    }
}
