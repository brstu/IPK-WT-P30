using Moq;
using Xunit;

public class ExpensesControllerTests
{
    private readonly Mock<IExpenseService> _mockExpenseService;
    private readonly ExpensesController _controller;

    public ExpensesControllerTests()
    {
        _mockExpenseService = new Mock<IExpenseService>();
        _controller = new ExpensesController(_mockExpenseService.Object);
    }

    [Fact]
    public async Task GetExpenses_ReturnsOkResult()
    {
        var expenses = new List<Expense> { new() { Id = 1, Amount = 100 } };
        _mockExpenseService.Setup(s => s.GetExpensesAsync()).ReturnsAsync(expenses);

        var result = await _controller.GetExpenses();

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetExpense_ReturnsNotFound_WhenExpenseDoesNotExist()
    {
        _mockExpenseService.Setup(s => s.GetExpenseByIdAsync(1)).ReturnsAsync((Expense?)null);

        var result = await _controller.GetExpense(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateExpense_ReturnsCreatedResult()
    {
        var expense = new Expense { CategoryId = 1, Amount = 100 };

        var result = await _controller.CreateExpense(expense);

        Assert.NotNull(result);
    }
}
