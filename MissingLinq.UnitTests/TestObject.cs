using System;

namespace MissingLinq.UnitTests
{
  class TestObject
  {
    public TestObject(int id, string name)
    {
      Name = name;
      Id = id;
    }

    public readonly string Name;
    public readonly int Id;

    protected bool Equals(TestObject other)
    {
      return String.Equals(Name, other.Name) && Id == other.Id;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((TestObject) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return ((Name != null ? Name.GetHashCode() : 0)*397) ^ Id;
      }
    }
  }
}