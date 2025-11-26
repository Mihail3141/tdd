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
        validCenter = new Point(1920 / 2, 1080 / 2);
    }


    [TestCase(-1920, 1080)]
    [TestCase(1920, -1080)]
    public void CircularCloudLayouter_ShouldThrowException_WhenInvalidWindowSize(int imageWidth, int imageHeight)
    {
        var act = () => new CircularCloudLayouter(validCenter, imageWidth, imageHeight);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Image width or height must be positive");
    }

    [TestCase(-500, 500)]
    [TestCase(500, -500)]
    public void CircularCloudLayouter_ShouldThrowException_WhenInvalidCenter(int centerX, int centerY)
    {
        var invalidCenter = new Point(centerX, centerY);
        var act = () => new CircularCloudLayouter(invalidCenter);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Center should be greater than 0");
        ;
    }


    [TestCase(1930, 500)]
    [TestCase(500, 1090)]
    public void CircularCloudLayouter_ShouldThrowException_WhenCenterIsOutsideImage(int centerX, int centerY)
    {
        var center = new Point(centerX, centerY);
        var act = () => new CircularCloudLayouter(center);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Center must be inside image bounds");
        ;
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
    public void GetCircularCloudRectangles_ShouldThrowException_WhenCountIsNotPositive(int count)
    {
        var circularCloud = new CircularCloudLayouter(validCenter);
        var rectangleSize = new Size(40, 40);
        var act = () => circularCloud.GetCircularCloudRectangles(count, rectangleSize).ToList();

        act.Should().Throw<ArgumentException>()
            .WithMessage("Count must be positive");
    }

    [Test]
    public void GetCircularCloudRectangles_ShouldNotProduceIntersectingRectangles_WhenCalledMultipleTimes()
    {
        var circularCloud = new CircularCloudLayouter(validCenter);
        var rectangleSize = new Size(20, 10);
        var rectangles = circularCloud
            .GetCircularCloudRectangles(100, rectangleSize)
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
    
    [TestCase(-1)]
    [TestCase(2)]
    public void GetCircularCloudRectangles_ShouldThrowException_WhenInvalidCoefficient(double coefficient)
    {
        var circularCloud = new CircularCloudLayouter(validCenter);
        var rectangleSize = new Size(20, 10);
        var act = () => circularCloud
            .GetCircularCloudRectangles(100, rectangleSize, coefficient)
            .ToList();
        act.Should().Throw<ArgumentException>()
            .WithMessage("Coefficient must be between 0 and 1");
    }
    
    [TestCase(0.1)]
    [TestCase(0.2)]
    [TestCase(0.5)]
    [TestCase(0.9)]
    public void GetCircularCloudRectangles_ShouldNotProduceIntersectingRectangles_WhenValidCoefficient(double coefficient)
    {
        var circularCloud = new CircularCloudLayouter(validCenter);
        var rectangleSize = new Size(20, 10);
        var rectangles = circularCloud
            .GetCircularCloudRectangles(100, rectangleSize)
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