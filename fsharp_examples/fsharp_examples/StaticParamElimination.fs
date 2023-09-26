// -------------------------------- 10. Static Parameter Elimination -------------------------------- //

// This is about chooisng function scope. One motivation for having a function as a subfunction
// is that static information it needs available in the outer function can be fed in directly
// as in-scope identifiers without the overhead of passing it through function parameters.
// This overhead is typically not about performance. Compilers can optimsie it. It is conceptual overhead.

// Suppose we have a function that filters between good and bad loans and a function that calculates interest.

module parameterElimination2

// Consider the scope of this helper function. If it is defined outside, it needs to have these input parameters.
let calculateInterest rate years principal =
    principal * (1.0 + rate / 100.0) ** float years

let goodLoan interest years principal budget =
    calculateInterest interest years principal < budget



// Consider defining calculateInterest inside goodLoan.
let goodLoan2 rate years principal budget =
    // Here we are able to eliminate all parameters.
    // Of course this is not always the case but it demonstrates that
    // scope can make a difference in what inputs your function needs.
    let calculateInterest =
        principal * (1.0 + rate / 100.0) ** float years

    calculateInterest < budget
