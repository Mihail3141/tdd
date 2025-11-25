using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    public Point Center { get; set; }
    public CircularCloudLayouter(Point center)
    {
        Center = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        var pointGenerator = new PointGenerator(Center);
        var points = pointGenerator.GetPointsOnSpiral(10);
        throw  new NotImplementedException();
    }
}