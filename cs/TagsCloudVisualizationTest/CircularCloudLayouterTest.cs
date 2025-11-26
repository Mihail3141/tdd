using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTest;

public class CircularCloudLayouterTest
{
    private Point validCenter;

    [SetUp]
    public void Setup()
    {
        validCenter = new Point(1920/2, 1080/2);
    }

    
    [TestCase(-1920, 1080)]
    [TestCase(1920, -1080)]
    public void CircularCloudLayouter_ShouldThrowException_WhenInvalidWindowSize(int windowWidth, int windowHeight)
    {
        var act = () => new CircularCloudLayouter(validCenter,  windowWidth, windowHeight);
        
        act.Should().Throw<ArgumentException>()
            .WithMessage("Window width or height must be positive");
    }
    
    [TestCase(-500, 500)]
    [TestCase(500, -500)]
    public void CircularCloudLayouter_ShouldThrowException_WhenInvalidCenter(int centerX, int centerY)
    {
        var invalidCenter = new Point(centerX, centerY);
        var act = () => new CircularCloudLayouter(invalidCenter);
        
        act.Should().Throw<ArgumentException>()
            .WithMessage("Center should be greater than 0");;
    }
    

    [TestCase(1930, 500)]
    [TestCase(500, 1090)]
    public void CircularCloudLayouter_ShouldThrowException_WhenCenterIsOutsideWindow(int centerX, int centerY)
    {
        var center = new Point(centerX, centerY);
        var act = () => new CircularCloudLayouter(center);
        
        act.Should().Throw<ArgumentException>()
            .WithMessage("Center must be in window");;
    }
    
    [TestCase(-100, 100)]
    [TestCase(100, -100)]
    [TestCase(0, 100)]
    [TestCase(100, 0)]
    public void PutNextRectangle_ShouldThrowException_WhenInvalidRectangleSize(int rectangleWidth, int rectangleHeight)
    {
        var circularCloud = new CircularCloudLayouter(validCenter);
        var act = () => circularCloud.PutNextRectangle(new Size(rectangleWidth, rectangleHeight));

        act.Should().Throw<ArgumentException>()
            .WithMessage("Rectangle size must be positive");
    }
    
    [Test]
    public void PutNextRectangle_ShouldThrowException_WhenNoPlaceForNextRectangle()
    {
        var center = new Point(200, 200);
        var circularCloud = new CircularCloudLayouter(center, 400, 400);
        var rectangles = new List<Rectangle>();
        var rectangleSize = new Size(40, 40);
        for (var i = 0; i < 111; i++)
        {
            var rect = circularCloud.PutNextRectangle(rectangleSize);
            rectangles.Add(rect);
        }
        var act = () => circularCloud.PutNextRectangle(rectangleSize);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Failed to find place for the * rectangle");
    }

    [Test]
    public void PutNextRectangle_ShouldNotProduceIntersectingRectangles_WhenCalledMultipleTimes()
    {
        var circularCloud = new CircularCloudLayouter(validCenter);
        var rectangles = new List<Rectangle>();
        var rectangleSize = new Size(20, 10);
        for (var i = 0; i < 50; i++)
        {
            var rect = circularCloud.PutNextRectangle(rectangleSize);
            rectangles.Add(rect);
        }

        for (var i = 0; i < rectangles.Count; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                rectangles[i].IntersectsWith(rectangles[j])
                    .Should().BeFalse($"rect #{i} should not intersect rect #{j}");
            }
        }
    }
    
    [TestCase(-1)]
    [TestCase(0)]
    public void GetCircularCloud_ShouldThrowException_WhenCountIsNotPositive(int count)
    {
        var circularCloud = new CircularCloudLayouter(validCenter);
        var rectangleSize = new Size(40, 40);
        var act = () => circularCloud.GetCircularCloud(count, rectangleSize).ToList();
        
        act.Should().Throw<ArgumentException>()
            .WithMessage("Count must be positive");
    }
          
    [Test]
    public void GetCircularCloud_ShouldNotProduceIntersectingRectangles_WhenCalledMultipleTimes()
    {
        var circularCloud = new CircularCloudLayouter(validCenter);
        var rectangleSize = new Size(20, 10);
        var rectangles = circularCloud
            .GetCircularCloud(100, rectangleSize)
            .ToList();

        for (var i = 0; i < rectangles.Count; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                rectangles[i].IntersectsWith(rectangles[j])
                    .Should().BeFalse($"rect #{i} should not intersect rect #{j}");
            }
        }
    }
}