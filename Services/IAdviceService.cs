namespace TodoApi.Services;

public interface IAdviceService
{
    Task<string> GetRandomAdvice();
}