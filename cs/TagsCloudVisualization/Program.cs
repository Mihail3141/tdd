using System.Drawing;


namespace TagsCloudVisualization;

public static class Program
{
    static void Main()
    {
        var imageCenter = new Point(1920/2, 1080/2);
        var rectangleSize = new Size(200, 85);
        var imageSize = new Size(1920, 1080);
        
        var spiralPointProvider = new SpiralPointProvider(imageCenter);
        var rectangles = spiralPointProvider.GetCircularCloudRectangles(40,rectangleSize, 0.5);
        var visualizer = new Render(imageSize);
        var image = visualizer.CreateRectangleCloud(rectangles);
        ImageSaver.SaveImage(image, "cloud.png");
    }
}