namespace Inventory.Infraestructure
{
  public static class PrivateReflectionDynamicObjectExtensions
  {
    public static dynamic AsDynamic(this object o)
    {
      return PrivateReflectionDynamicObject.WrapObjectIfNeeded(o);
    }
  }
}