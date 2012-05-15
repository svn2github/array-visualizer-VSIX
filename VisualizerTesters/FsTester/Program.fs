open System
open System.Diagnostics
open LinqLib.Sequence
open LinqLib.Array


Debugger.Break()

type TestType = { a : int;  
                  b : int }
                  override m.ToString() = String.Format("{0} of {1}", m.a , m.b)

let GetNewType x = { a = x ; b = x * x} 

let arr1 = Enumerator.Generate<int>(1, 1, 100).Shuffle().ToArray(5, 5, 4)
let arr2 = Enumerator.Generate<int>(1, 1, 49).Shuffle().ToArray(7, 7)
let arr3 = Enumerator.Generate<TestType>(15, GetNewType).Shuffle().ToArray(5, 3)
let arr4 = Enumerator.Generate<TestType>(27, GetNewType).Shuffle().ToArray(9, 3)

Debugger.Break()

