// -------------------------------- 6. Global Helper Functions -------------------------------- //

// This is demonstrated very well by an example of a helper function from issie that is used many times throughout the codebase.
// For greater context please look at the issie codebase and observe where the function is used.
module issieFunctionExample
// This function is defined in src/Renderer/Common/DrawHelpers.fs
// Even though it is such a small function, it is very useful. Look up how many times this simole function is used in issie.

/// makes a line segment offset dx,dy
let makeLineAttr dx dy =
    $"l %.3f{dx} %.3f{dy}"