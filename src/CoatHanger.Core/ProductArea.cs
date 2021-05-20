namespace CoatHanger.Core
{
    public abstract class ProductArea : IAreaPath
    {
        public abstract string ID { get; }

        public abstract string Title { get; }

        public abstract string Summary { get; }

        public ITheme Theme => throw new System.NotImplementedException();

        public virtual IAreaPath ParentArea => null;

        public virtual string Area => "Product";
    }

    /// <summary>
    /// Features are the "tools" you use within a system to complete a set of actions. 
    /// </summary>
    public abstract class FeatureArea<T> : IAreaPath where T : ProductArea
    {
        public abstract string ID { get; }
        public abstract string Title { get; }
        public abstract string Summary { get; }
        public abstract T Parent { get; }
        public virtual IAreaPath ParentArea => (IAreaPath)Parent;
        public virtual ITheme Theme => new NoTheme();
        public virtual string Area => "Feature";
    }

    /// <summary>
    /// Functionality is how those features actually work to provide you with a desired outcome.
    /// </summary>
    public abstract class FunctionArea<T> : IAreaPath where T : IAreaPath
    {
        public abstract string ID { get; }
        public abstract string Title { get; }
        public abstract string Summary { get; }
        public virtual ITheme Theme => new NoTheme();
        public virtual IAreaPath ParentArea => (IAreaPath)Parent;
        public abstract T Parent { get; }
        public virtual string Area => "Function";
    }

    public abstract class ComponentArea<T> : IAreaPath where T : IAreaPath
    {
        public abstract string ID { get; }
        public abstract string Title { get; }
        public abstract string Summary { get; }
        public abstract T Parent { get; }
        public virtual ITheme Theme => new NoTheme();
        public virtual IAreaPath ParentArea => (IAreaPath)Parent;
        public virtual string Area => "Component";
    }

    public abstract class TaskArea<T> : IAreaPath where T : IAreaPath
    {
        public abstract string ID { get; }
        public abstract string Title { get; }
        public abstract string Summary { get; }
        public abstract T Parent { get; }
        public virtual IAreaPath ParentArea => (IAreaPath)Parent;
        public virtual ITheme Theme => new NoTheme();
        public virtual string Area => "Task";
    }

    public interface IAreaPath
    {
        public string ID { get; }
        public string Title { get; }
        public string Summary { get; }
        public ITheme Theme { get; }
        public IAreaPath ParentArea { get; }
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


    /// <summary>
    /// They’re statements that describe how you must carry out 
    /// certain operations and if there is some limit that you need to apply.
    /// 
    /// They guide behaviors and define what, where, when, why and how 
    /// something should be done in a company.
    /// </summary>
    public abstract class BusinessRule
    {
        public abstract string ID { get; }
        public abstract string Title { get; }
        public abstract string Summary { get;  }
        public abstract RuleType RuleType { get; }
        public abstract BusinessRule Parent { get; }
    }


    public enum RuleType
    {
        /// <summary>
        /// Constraint rules specify policies or conditions that restrict object structure and behavior. 
        /// </summary>
        Constraint,

        /// <summary>
        /// Inference rules specify that if certain facts are true, 
        /// a conclusion can be inferred. 
        /// </summary>
        Inference,

        /// <summary>
        /// Computation rules derive their results by way of processing algorithms, 
        /// a more sophisticated variant of inference rules. 
        /// </summary>
        Computation,

        /// <summary>
        /// A decision rule is used when a subject needs to be evaluated and assigned the next step. 
        /// (e.g. approved, rejected, sent back for more information).
        /// </summary>
        Decision
    }

}