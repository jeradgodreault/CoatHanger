namespace CoatHanger.Core
{
    public interface IProduct
    {
        string ProductID { get; }
        string Title { get; }
        string Summary { get; }
    }

    public interface IProductFeature
    {
        string FeatureID { get; }
        string Title { get; }
        string Summary { get; }
        IProduct Product { get; }
    }

    public interface IFeatureFunction
    {
        string FunctionID { get; }
        string Title { get; }
        string Summary { get; }
        IProductFeature Feature { get; }
    }
}
