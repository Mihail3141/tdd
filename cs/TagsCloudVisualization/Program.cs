using System.Drawing;
using TagsCloudVisualization.PointProvider;
using TagsCloudVisualization.Render;


namespace TagsCloudVisualization;

public static class Program
{
    static void Main()
    {
        var imageCenter = new Point(1920/2, 1080/2);
        var rectangleSize = new Size(200, 80);
        var imageSize = new Size(1920, 1080);
        
        var spiralPointProvider = new SpiralPointProvider(imageCenter);
        var rectangles = spiralPointProvider.GetRandomSizedRectangles(40,rectangleSize, 0.3);
        var visualizer = new TagCloudRenderer(imageSize);
        var image = visualizer.CreateRectangleCloud(rectangles);
        ImageSaver.SaveImage(image, "cloud.png");
    }
}