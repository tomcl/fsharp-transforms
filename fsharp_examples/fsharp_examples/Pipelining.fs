// -------------------------------------- 2. Pipelining -------------------------------------- //

// Pipelining allows you to chain functions together in a readable way.
// Special and common combination of pipelining and structural abstraction where a function
// is split into multiple pipelined subfunctions each of which does part of the work.
// This corresponds to transforming the input data through a number of stages. 
// This is an example of pipelining in F# to calculate the square of the sum of a list of numbers:
module pipelining

let square x = x * x
let numbers = [1; 2; 3; 4; 5]

// Before Pipelining
let result1 =
    let sum = List.sum numbers
    let sq = square sum
    sq

// After Pipelining
let result2 =
    numbers |> List.sum |> square

// It makes it easier to follow the data transformation.