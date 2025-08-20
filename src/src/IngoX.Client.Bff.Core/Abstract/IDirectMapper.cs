namespace IngoX.Client.Bff.Core.Abstract;

public interface IDirectMapper<TFrom, TTo>
{
    TTo Map(TFrom from);
}
