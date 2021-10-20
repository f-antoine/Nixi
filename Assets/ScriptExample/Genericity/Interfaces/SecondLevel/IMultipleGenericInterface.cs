namespace Assets.ScriptExample.Genericity.Interfaces.SecondLevel
{
    public interface IMultipleGenericInterface<T, R>
    {
        public T Build(R arg);
    }
}