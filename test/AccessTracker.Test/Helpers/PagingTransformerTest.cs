using AccessTracker.Helpers;
using FluentAssertions;
using Moq;

namespace AccessTracker.Test.Helpers;

public class PagingTransformerTest : TestBase<PagingTransformer>
{
    [Fact(DisplayName = "Возвращает 0, если page null")]
    public void Should_Return_Skip_Zero_When_Page_Is_Null()
    {
        // Act
        var (skip, _) = Target.GetSkipTake(null, It.IsAny<int>(), It.IsAny<int>());
        
        //Assert
        skip.Should().Be(0);
    }
    
    [Fact(DisplayName = "Возвращает take = defaultPageSize, если page null")]
    public void Should_Return_DefaultPageSize_When_PageSize_Is_Null()
    {
        // Arrange
        const int defaultPageSize = 10;
        
        // Act
        var (_, take) = Target.GetSkipTake(
            It.IsAny<int>(),
            null,
            defaultPageSize);
        
        //Assert
        take.Should().Be(defaultPageSize);
    }
    
    [Fact(DisplayName = "Возвращает take = pageSize, если page != null")]
    public void Should_Return_PageSize_When_PageSize_Is_Not_Null()
    {
        // Arrange
        const int pageSize = 123;
        
        // Act
        var (_, take) = Target.GetSkipTake(
            It.IsAny<int>(),
            pageSize,
            It.IsAny<int>());
        
        //Assert
        take.Should().Be(pageSize);
    }
}