# StableEnum
Serialization of enums as strings in Unity3d.  
Kudos to my colleague [Vasyl Romanets](https://github.com/O1dSeaman) for an idea how to make this solution cleaner

## Problem
Unity3d serializes enums as ints. This causes the problem when new values of enum are inserted before the last value: it "suddenly" turns out that previously serialized enums have wrong values.  
Consider the following situation:
```
enum Something
{
  Zero,    // 0
  One,     // 1
  Several  // 2
  Many     // 3
}

public Something something = Several;
```
Public field `something` is serialized as 2.  
Now, if we insert a new value between other values of `enum Something`:  
```
enum Something
{
  Zero,    // 0
  One,     // 1
  Two,     // 2
  Several  // 3
  Many     // 4
}
```
field `something` will have value `Two` instead of `Several`.  
Usually this behaviour is undesirable.

## Solution
Solution is simple: enum should be serialized as string instead of int.  
**StableEnum** does that for you.

### Usage
Unfortunately Unity3d does not serialize generic classes (and another stuff, like interfaces).  
That's why the first thing you have to do to use StableEnums in your project is to provide concrete derived classes of StableEnum<T> class for each enum you want to make *stable*.  
You can use [ConcreteStableEnums.cs](Scripts/ConcreteStableEnums.cs) as an example of how to do that. You can put all your concrete classes right in this file.  
##### Note
It's not required to provide implicit cast operator.  
If cast operator is not provided, you'll have to alter enum via `value` property. E.g.
```
myEnum.value = MyEnum.SomeValue;
```
However, if cast operator is provided it would be possible to alter enum as easy as normal enum. E.g.
```
myEnum = MyEnum.SomeValue;
```
So, you decide.
##### Why implicit cast operator is not implemented in StableEnum<T>?
That's not possible, because cast operator is static, and therefore can't be inherited. That's why each class derived from StableEnum<T> must implement its own cast operator.
However, StableEnum<T> has implementation of operator which casts from StableEnum<T> to T.  
That's why you can do this with no additional code from your side:
```
MyEnum enumValue = myEnum; // myEnum is an instance of the class derived from StableEnum<MyEnum>
```
