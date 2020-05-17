namespace Kata
{
    public interface IKataStepProvider
    {
        IKataStep GetStep(string kataStepIdentifier);
    }
}