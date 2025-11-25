using System.Drawing;

namespace TagsCloudVisualization;

public class Program
{
    
    static void Main(string[] args)
    {
        var centre = new Point(100/2,100/2);
        var cc = new CircularCloudLayouter(centre);
        var size = new Size(100, 100);
        var r = cc.PutNextRectangle(size);
    }
}