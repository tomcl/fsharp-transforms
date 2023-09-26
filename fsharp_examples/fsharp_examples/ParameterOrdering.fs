// ----------------------------------- 7. Ordering ----------------------------------- //

// Order parameters so that Currying can be used nicely to implement partial evaluation.
// Generally this means the more static ones firts. Note that in some cases there are no good orders,
// and an anonymous adapter function is needed so that the correct argument can be fed in from a pipeline.

// Suppose you want to create a function addThenMultiply and use currying to partially apply the function and create different variations.

module ordering

// Here is a function with inputs ordered in a useful way. 
let addThenMultiply addBy multiplyBy x = (x + addBy) * multiplyBy

// Partial application is useful here.
let addTwoThenMultiplyByThree = addThenMultiply 2 3

// The results will not be as desired
let result1 = addTwoThenMultiplyByThree 4  // (4 + 2) * 3 = 18
let result2 = addTwoThenMultiplyByThree 5  // (5 + 2) * 3 = 21

printfn "result1 = %A, result2 = %A" result1 result2



// Here is the same function but without proper input ordering.
let addThenMultiply2 x addBy multiplyBy = (x + addBy) * multiplyBy

// Now, partial application no longer produces the desired results.
let addTwoThenMultiplyByThree2 = addThenMultiply2 2 3

// Usage: Using the partially applied function.
let result3 = addTwoThenMultiplyByThree2 4  // (2 + 3) * 4 = 20
let result4 = addTwoThenMultiplyByThree2 5  // (2 + 3) * 5 = 25

printfn "result3 = %A, result4 = %A" result3 result4