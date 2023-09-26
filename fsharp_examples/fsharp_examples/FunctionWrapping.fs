// -------------------------------- 5. Function Wrapping -------------------------------- //

// A function is used multiple times in a particular way – e.g. with some inputs fixed,
// or with a change to its output. Create a local wrapper function which implements the modified function.
// Note that this is similar in effect to input wrapping.
// Note also that if inputs are corrctly ordered Currying does this without the need for a wrapper -
// although a wrapper may still be used to reduce noise and add abstraction.
// Here is an example of function wrapping to calculate the double of a square of a number:

module functionWrapping

// Suppose square is used throughout a codebase but you want to use a slightly different version of it localy
let square x = x * x

// Create a wrapper function that acts as the modified version of square
let squareAndDouble x =
    let squared = square x
    squared * 2

let number = 4
let result = squareAndDouble number