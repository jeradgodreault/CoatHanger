namespace CoatHanger.Core
{
    public interface IProduct
    {
        string ID { get; }
        string Title { get; }
        string Summary { get; }
    }

    /// <summary>
    /// Features are the "tools" you use within a system to complete a set of actions. 
    /// </summary>
    public abstract class FeatureArea : IAreaPath<IProduct>
    {
        public abstract string ID { get; }
        public abstract string Title { get; }
        public abstract string Summary { get; }        
        public abstract IProduct Parent { get; }        
        public virtual ITheme Theme => new NoTheme();
        public virtual string Area => "Feature";
    }

    /// <summary>
    /// Functionality is how those features actually work to provide you with a desired outcome.
    /// </summary>
    public abstract class FunctionArea : IAreaPath<FeatureArea>
    {
        public abstract string ID { get; }
        public abstract string Title { get; }
        public abstract string Summary { get; }
        public virtual ITheme Theme => new NoTheme();
        public abstract FeatureArea Parent { get; }
        public virtual string Area => "Function";
    }

    public abstract class ComponentArea : IAreaPath<FunctionArea>
    {
        public abstract string ID { get; }
        public abstract string Title { get; }
        public abstract string Summary { get; }        
        public abstract FunctionArea Parent { get; }
        public virtual ITheme Theme => new NoTheme();
        public virtual string Area => "Component";
    }

    public abstract class TaskArea : IAreaPath<ComponentArea>
    {
        public abstract string ID { get; }
        public abstract string Title { get; }
        public abstract string Summary { get; }
        public abstract ComponentArea Parent { get; }
        public virtual ITheme Theme => new NoTheme();
        public virtual string Area => "Task";
    }


    public interface IAreaPath<T>
    {
        public string ID { get; }
        public string Title { get; }
        public string Summary { get; }
        public ITheme Theme { get; }
        public T Parent { get; }
        public string Area { get; }
    }

    public interface ITheme
    {
        string ID { get; }
        string Title { get; }
        string Summary { get; }
    }

    public class NoTheme : ITheme
    {
        public string ID => "";

        public string Title => "";

        public string Summary => "";
    }
}
