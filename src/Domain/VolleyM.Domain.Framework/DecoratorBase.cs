namespace VolleyM.Domain.Framework
{
    public abstract class DecoratorBase<T>
    {
        /// <summary>
        /// Instance wrapped by decorator, might be another decorator
        /// </summary>
        protected T Decoratee { get; }

        /// <summary>
        /// Original non-decorator instance wrapped
        /// </summary>
        protected T RootInstance
        {
            get
            {
                var result = Decoratee;

                while (result is DecoratorBase<T> wrapped)
                {
                    result = wrapped.Decoratee;
                }

                return result;
            }
        }

        protected DecoratorBase(T decoratee)
        {
            Decoratee = decoratee;
        }
    }
}